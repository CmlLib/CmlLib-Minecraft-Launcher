using CmlLib.Core.Auth;
using CmlLib_Minecraft_Launcher.Models;
using CmlLib_Minecraft_Launcher.Services;
using CmlLib_Minecraft_Launcher.UI;
using Terminal.Gui;

namespace CmlLib_Minecraft_Launcher.Controllers;

internal sealed class LauncherController
{
    private readonly MainWindow _view;
    private readonly LauncherService _launcherService;
    private readonly AuthService _authService;
    private readonly LogWindow _logWindow = new();
    private bool _logWindowAdded;

    private readonly List<VersionListItem> _allVersions = new();
    private List<VersionListItem> _filteredVersions = new();
    private MSession? _session;
    private string? _selectedVersion;

    public LauncherController(MainWindow view, LauncherService launcherService, AuthService authService)
    {
        _view = view;
        _launcherService = launcherService;
        _authService = authService;

        _view.SelectionChanged += OnSelectionChanged;
        _view.FilterChanged += ApplyFilter;
        _view.RefreshRequested += OnRefreshRequested;
        _view.LaunchRequested += OnLaunchRequested;
        _view.ExitRequested += OnExitRequested;
    }

    public void Start()
    {
        Task.Run(async () =>
        {
            await LoadVersionsAsync();
            await AuthenticateAsync();
        });
    }

    private async Task LoadVersionsAsync()
    {
        _view.SetRefreshEnabled(false);
        _view.SetStatus("Fetching versions...");

        try
        {
            var versions = await _launcherService.GetAllVersionsAsync();
            _allVersions.Clear();
            _allVersions.AddRange(versions);
            ApplyFilter(_view.CurrentFilters);
        }
        catch (Exception ex)
        {
            _view.ShowError("Error", $"Failed to load versions:\n{ex.Message}");
            _view.SetStatus("Error loading versions");
            _view.SetVersions(Array.Empty<VersionListItem>(), 0);
        }
        finally
        {
            _view.SetRefreshEnabled(true);
        }
    }

    private void ApplyFilter(VersionFilters filters)
    {
        _filteredVersions = _allVersions.Where(v => filters.Allows(v.Type)).ToList();
        _view.SetVersions(_filteredVersions, _allVersions.Count);

        if (_selectedVersion != null && !_filteredVersions.Any(v => v.Name == _selectedVersion))
        {
            _selectedVersion = null;
            _view.SetSelectedVersion(null);
            _view.SetLaunchEnabled(false);
        }
        else
        {
            TryEnableLaunch();
        }
    }

    private void OnSelectionChanged(int index)
    {
        _selectedVersion = _view.ResolveVersionName(index);
        _view.SetSelectedVersion(_selectedVersion);
        TryEnableLaunch();
    }

    private Task OnLaunchRequested()
    {
        if (_selectedVersion is null)
        {
            _view.ShowError("Launch", "Select a version first.");
            return Task.CompletedTask;
        }

        if (_session is null)
        {
            _view.ShowError("Authentication Required", "Please wait for authentication to complete before launching.");
            return Task.CompletedTask;
        }

        _view.SetLaunchEnabled(false);

        var progressDialog = new InstallProgressDialog();
        progressDialog.Show();

        _ = Task.Run(async () =>
        {
            try
            {
                var installResult = await _launcherService.InstallAsync(_selectedVersion, progressDialog, progressDialog.Token);
                progressDialog.Close();

                if (installResult.Cancelled)
                {
                    _view.ShowInfo("Cancelled", "Installation was cancelled.");
                    _view.SetLaunchEnabled(true);
                    return;
                }

                if (!installResult.Success)
                {
                    var errorMessage = installResult.Error?.Message ?? "Unknown error during installation.";
                    _view.ShowError("Error", errorMessage);
                    _view.SetLaunchEnabled(true);
                    return;
                }

                EnsureLogWindow();
                _logWindow.Clear();
                _logWindow.Visible = true;
                _logWindow.SetNeedsDisplay();
                _logWindow.AppendLine($"Launching {_selectedVersion}...");
                _view.ShowLogView(_logWindow);

                await _launcherService.LaunchAsync(_selectedVersion, _session, _logWindow.AppendLine);
                _view.SetLaunchEnabled(true);
            }
            catch (Exception ex)
            {
                _view.ShowError("Error", $"Failed to launch game:\n{ex}");
                _view.SetLaunchEnabled(true);
            }
            finally
            {
                progressDialog.Dispose();
            }
        });

        return Task.CompletedTask;
    }

    private void OnRefreshRequested()
    {
        Task.Run(LoadVersionsAsync);
    }

    private async Task AuthenticateAsync()
    {
        _view.SetStatus("Authenticating...");
        _view.SetLaunchEnabled(false);

        try
        {
            _session = await _authService.AuthenticateAsync();
            _view.SetAccount($"Account: {_session.Username}");
            _view.SetStatus("Authenticated successfully");
        }
        catch (Exception ex)
        {
            _view.ShowError("Authentication Error", $"Failed to authenticate:\n{ex.Message}\n\nFalling back to offline mode.");
            _session = MSession.CreateOfflineSession("Player");
            _view.SetAccount("Account: Offline (Player)");
            _view.SetStatus("Using offline mode");
        }

        TryEnableLaunch();
    }

    private void OnExitRequested()
    {
        Application.RequestStop();
    }

    private void TryEnableLaunch()
    {
        var canLaunch = _session != null && _selectedVersion != null;
        _view.SetLaunchEnabled(canLaunch);
    }

    private void EnsureLogWindow()
    {
        if (!_logWindowAdded)
        {
            _view.Add(_logWindow);
            _logWindowAdded = true;
        }

        if (_logWindow.SuperView == null)
        {
            _view.Add(_logWindow);
        }

        _logWindow.Visible = true;
        _logWindow.SetNeedsDisplay();
    }
}

