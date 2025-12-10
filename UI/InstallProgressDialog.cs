using CmlLib_Minecraft_Launcher.Services;
using Terminal.Gui;

namespace CmlLib_Minecraft_Launcher.UI;

internal sealed class InstallProgressDialog : IInstallProgressObserver, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly Dialog _dialog;
    private readonly Label _progressLabel;
    private readonly ProgressBar _progressBar;
    private readonly Label _progressText;
    private readonly Button _cancelButton;
    private bool _disposed;
    private int _lastFileProgress;
    private int _lastTotalFiles;
    private string _lastFileName = string.Empty;
    private bool _dirty;
    private readonly TimeSpan _uiInterval = TimeSpan.FromMilliseconds(200);
    private object? _uiTimer;

    public InstallProgressDialog()
    {
        _dialog = new Dialog("Progress", 60, 10);
        _progressLabel = new Label("Installing/Checking game files...")
        {
            X = 1,
            Y = 1,
            Width = Dim.Fill() - 2
        };
        _progressBar = new ProgressBar
        {
            X = 1,
            Y = 3,
            Width = Dim.Fill() - 2,
            Height = 1
        };
        _progressText = new Label("")
        {
            X = 1,
            Y = 5,
            Width = Dim.Fill() - 2
        };
        _cancelButton = new Button("Cancel")
        {
            X = Pos.Center() - 5,
            Y = 7
        };

        _cancelButton.Clicked += () =>
        {
            _cancellationTokenSource.Cancel();
            Application.RequestStop(_dialog);
        };

        _dialog.Add(_progressLabel, _progressBar, _progressText, _cancelButton);

        StartUiTimer();
    }

    public CancellationToken Token => _cancellationTokenSource.Token;

    public void Show()
    {
        InvokeOnUi(() =>
        {
            if (_disposed)
                return;

            if (Application.Top != null)
            {
                Application.Top.Add(_dialog);
                _dialog.Visible = true;
                SafeRefresh();
            }
        });
    }

    public void Close()
    {
        InvokeOnUi(() =>
        {
            if (_disposed)
                return;

            if (Application.Top != null)
            {
                Application.Top.Remove(_dialog);
                SafeRefresh();
            }
        });
    }

    public void OnFileProgress(int progressed, int total, string name)
    {
        _lastFileProgress = progressed;
        _lastTotalFiles = total;
        _lastFileName = name;
        _dirty = true;
    }

    public void OnByteProgress(long downloaded, long total, float fraction)
    {
        InvokeOnUi(() =>
        {
            if (_disposed || Application.Top == null)
                return;

            _progressBar.Fraction = fraction;
            var mbDownloaded = downloaded / 1024 / 1024;
            var mbTotal = total / 1024 / 1024;
            _progressText.Text = $"{fraction * 100:F1}% - {mbDownloaded} MB / {mbTotal} MB";
            SafeRefresh();
        });
    }

    public void Dispose()
    {
        _disposed = true;
        StopUiTimer();
        Close();
        _cancellationTokenSource.Dispose();
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

    private void StartUiTimer()
    {
        var loop = Application.MainLoop;
        if (loop == null)
            return;

        _uiTimer = loop.AddTimeout(_uiInterval, _ =>
        {
            if (_disposed)
                return false;

            if (_dirty)
            {
                _dirty = false;
                _progressText.Text = $"{_lastFileProgress}/{_lastTotalFiles} files - {_lastFileName}";
                _progressText.SetNeedsDisplay();
                SafeRefresh();
            }
            return true;
        });
    }

    private void StopUiTimer()
    {
        if (_uiTimer != null)
        {
            Application.MainLoop?.RemoveTimeout(_uiTimer);
            _uiTimer = null;
        }
    }

    private static void SafeRefresh()
    {
        try
        {
            Application.Refresh();
        }
        catch
        {
            // Application may be shutting down; ignore refresh failures
        }
    }
}

