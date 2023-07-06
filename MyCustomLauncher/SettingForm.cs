using CmlLib.Core;
using System.Diagnostics;

namespace MyCustomLauncher;

public partial class SettingForm : Form
{
    public SettingForm()
    {
        InitializeComponent();
    }

    public event EventHandler? FormClosingRequired;

    private void SettingForm_Load(object sender, EventArgs e)
    {
        txtGamePath.Text = MinecraftPath.GetOSDefaultPath();
    }

    private void btnChangeGamePath_Click(object sender, EventArgs e)
    {
        MessageBox.Show("not yet");
    }

    private void btnChangeAccount_Click(object sender, EventArgs e)
    {
        var form = new AccountForm();
        form.AutoLogin = false;
        form.Show();
        this.Close();
        FormClosingRequired?.Invoke(this, e);
    }

    private void btnLicense_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "https://github.com/CmlLib/CmlLib.Core",
            UseShellExecute = true,
        });
    }
}
