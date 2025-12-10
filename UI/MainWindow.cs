using CmlLib_Minecraft_Launcher.Models;
using Terminal.Gui;

namespace CmlLib_Minecraft_Launcher.UI;

internal sealed class MainWindow : Window
{
    private readonly Label _statusLabel;
    private readonly Label _accountLabel;
    private readonly Label _selectedVersionLabel;
    private readonly ListView _versionList;
    private readonly Button _launchButton;
    private readonly Button _refreshButton;
    private readonly CheckBox _filterCustom;
    private readonly CheckBox _filterRelease;
    private readonly CheckBox _filterSnapshot;
    private readonly CheckBox _filterOldAlpha;
    private readonly CheckBox _filterOldBeta;

    private List<VersionListItem> _visibleVersions = new();

    public event Action<VersionFilters>? FilterChanged;
    public event Action<int>? SelectionChanged;
    public event Func<Task>? LaunchRequested;
    public event Action? RefreshRequested;
    public event Action? ExitRequested;

    public MainWindow() : base("Minecraft Launcher")
    {
        X = 0;
        Y = 0;
        Width = Dim.Fill();
        Height = Dim.Fill();

        _statusLabel = new Label("Initializing...")
        {
            X = 1,
            Y = 1,
            Width = Dim.Fill() - 2
        };

        var filterFrame = new FrameView("Filter by Type")
        {
            X = 1,
            Y = 2,
            Width = 40,
            Height = 8
        };

        _filterCustom = new CheckBox("Custom", true) { X = 1, Y = 0 };
        _filterRelease = new CheckBox("Release", true) { X = 1, Y = 1 };
        _filterSnapshot = new CheckBox("Snapshot", false) { X = 1, Y = 2 };
        _filterOldAlpha = new CheckBox("Old Alpha", false) { X = 1, Y = 3 };
        _filterOldBeta = new CheckBox("Old Beta", false) { X = 1, Y = 4 };

        filterFrame.Add(_filterCustom, _filterRelease, _filterSnapshot, _filterOldAlpha, _filterOldBeta);

        var versionLabel = new Label("Versions:")
        {
            X = 42,
            Y = 2
        };

        _versionList = new ListView(new List<string> { "Loading versions..." })
        {
            X = 42,
            Y = 3,
            Width = Dim.Fill() - 43,
            Height = Dim.Fill() - 8,
            CanFocus = true
        };

        _selectedVersionLabel = new Label("Selected: None")
        {
            X = 1,
            Y = Pos.Bottom(_versionList) + 1,
            Width = Dim.Fill() - 2
        };

        _launchButton = new Button("Launch")
        {
            X = 1,
            Y = Pos.Bottom(_selectedVersionLabel) + 1,
            Enabled = false
        };

        _refreshButton = new Button("Refresh")
        {
            X = Pos.Right(_launchButton) + 1,
            Y = Pos.Bottom(_selectedVersionLabel) + 1
        };

        var exitButton = new Button("Exit")
        {
            X = Pos.AnchorEnd(10),
            Y = Pos.Bottom(_selectedVersionLabel) + 1
        };

        _accountLabel = new Label("Account: Not logged in")
        {
            X = 1,
            Y = Pos.Bottom(_launchButton) + 1,
            Width = Dim.Fill() - 2
        };

        Add(
            _statusLabel,
            filterFrame,
            versionLabel,
            _versionList,
            _selectedVersionLabel,
            _launchButton,
            _refreshButton,
            exitButton,
            _accountLabel
        );

        WireEvents(exitButton);
    }

    public VersionFilters CurrentFilters => new(
        _filterCustom.Checked,
        _filterRelease.Checked,
        _filterSnapshot.Checked,
        _filterOldAlpha.Checked,
        _filterOldBeta.Checked);

    public string? ResolveVersionName(int index)
    {
        if (index < 0 || index >= _visibleVersions.Count)
            return null;

        return _visibleVersions[index].Name;
    }

    public void SetStatus(string text) => InvokeOnUi(() => _statusLabel.Text = text);

    public void SetAccount(string text) => InvokeOnUi(() => _accountLabel.Text = text);

    public void SetSelectedVersion(string? versionName)
    {
        InvokeOnUi(() =>
        {
            _selectedVersionLabel.Text = versionName is null
                ? "Selected: None"
                : $"Selected: {versionName}";
        });
    }

    public void SetLaunchEnabled(bool enabled) => InvokeOnUi(() => _launchButton.Enabled = enabled);

    public void SetRefreshEnabled(bool enabled) => InvokeOnUi(() => _refreshButton.Enabled = enabled);

    public void SetVersions(IReadOnlyList<VersionListItem> versions, int totalCount)
    {
        var displayTexts = versions.Select(v => v.DisplayText).ToList();

        InvokeOnUi(() =>
        {
            _visibleVersions = versions.ToList();
            _versionList.SetSource(displayTexts);
            _statusLabel.Text = $"Showing {_visibleVersions.Count} of {totalCount} versions";
        });
    }

    public void ShowError(string title, string message) =>
        InvokeOnUi(() => MessageBox.ErrorQuery(title, message, "OK"));

    public void ShowInfo(string title, string message) =>
        InvokeOnUi(() => MessageBox.Query(title, message, "OK"));

    public void ShowLogView(View logView)
    {
        InvokeOnUi(() =>
        {
            RemoveAll();
            logView.X = 0;
            logView.Y = 0;
            logView.Width = Dim.Fill();
            logView.Height = Dim.Fill();
            Add(logView);
            BringSubviewToFront(logView);
            SetNeedsDisplay();
            Application.Refresh();
        });
    }

    private void WireEvents(Button exitButton)
    {
        _filterCustom.Toggled += (_) => FilterChanged?.Invoke(CurrentFilters);
        _filterRelease.Toggled += (_) => FilterChanged?.Invoke(CurrentFilters);
        _filterSnapshot.Toggled += (_) => FilterChanged?.Invoke(CurrentFilters);
        _filterOldAlpha.Toggled += (_) => FilterChanged?.Invoke(CurrentFilters);
        _filterOldBeta.Toggled += (_) => FilterChanged?.Invoke(CurrentFilters);

        _versionList.SelectedItemChanged += args => SelectionChanged?.Invoke(args.Item);

        _launchButton.Clicked += async () =>
        {
            if (LaunchRequested != null)
                await LaunchRequested.Invoke();
        };

        _refreshButton.Clicked += () => RefreshRequested?.Invoke();
        exitButton.Clicked += () => ExitRequested?.Invoke();
    }

    private static void InvokeOnUi(Action action)
    {
        var loop = Application.MainLoop;
        if (loop == null)
        {
            action();
            return;
        }

        loop.Invoke(action);
    }
}

