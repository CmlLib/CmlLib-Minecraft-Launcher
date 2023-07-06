using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Version;
using CmlLib.Utils;

namespace MyCustomLauncher;

public partial class LauncherForm : Form
{
    private readonly MSession _session;
    private readonly CMLauncher _launcher;

    private bool exitOnClose = true;

    public LauncherForm(MSession session)
    {
        _session = session;
        _launcher = new CMLauncher(new MinecraftPath());
        _launcher.FileChanged += _launcher_FileChanged;
        _launcher.ProgressChanged += _launcher_ProgressChanged;

        InitializeComponent();
    }

    private async void LauncherForm_Load(object sender, EventArgs e)
    {
        showAccountControl();
        await listVersions();
    }

    private async Task listVersions()
    {
        var versions = await _launcher.GetAllVersionsAsync();

        cbVersion.Items.Clear();
        foreach (var version in versions)
        {
            if (version.MType == MVersionType.Release || version.MType == MVersionType.Custom)
                cbVersion.Items.Add(version.Name);
        }
        cbVersion.Text = versions.LatestReleaseVersion?.Name;
    }

    private void showAccountControl()
    {
        var control = new AccountControl(_session);
        pAccountHolder.Controls.Clear();
        pAccountHolder.Controls.Add(control);
    }

    private async void btnStart_Click(object sender, EventArgs e)
    {
        this.Enabled = false;
        try
        {
            var process = await _launcher.CreateProcessAsync(cbVersion.Text, new MLaunchOption
            {
                Session = _session
            });

            var logForm = new LogForm();
            logForm.Show();

            var processUtil = new ProcessUtil(process);
            processUtil.OutputReceived += (s, e) => logForm.AppendLog(e);
            processUtil.StartWithEvents();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        this.Enabled = true;
    }

    private void _launcher_ProgressChanged(object? sender, System.ComponentModel.ProgressChangedEventArgs e)
    {
        pbProgress.Maximum = 100;
        pbProgress.Value = e.ProgressPercentage;
    }

    private void _launcher_FileChanged(CmlLib.Core.Downloader.DownloadFileChangedEventArgs e)
    {
        pbFiles.Maximum = e.TotalFileCount;
        pbFiles.Value = e.ProgressedFileCount;

        lbProgress.Text = $"[{e.FileKind}] {e.FileName} - {e.ProgressedFileCount} / {e.TotalFileCount}";
    }

    private void btnSetting_Click(object sender, EventArgs e)
    {
        var form = new SettingForm();
        form.FormClosingRequired += (s, e) =>
        {
            exitOnClose = false;
            this.Close();
        };
        form.ShowDialog();
    }

    private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (exitOnClose)
            Environment.Exit(0);
    }
}
