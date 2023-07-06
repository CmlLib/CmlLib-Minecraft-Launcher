using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Sessions;
using XboxAuthNet.OAuth;

namespace MyCustomLauncher;

public partial class AccountForm : Form
{
    JELoginHandler _loginHandler;

    public AccountForm()
    {
        _loginHandler = JELoginHandlerBuilder.BuildDefault();
        InitializeComponent();
    }

    private bool exitOnClose = true;
    public bool AutoLogin { get; set; } = true;

    private async void AccountForm_Load(object sender, EventArgs e)
    {
        this.Enabled = false;
        listAccounts();
        if (AutoLogin)
            await tryAutoLogin();
        this.Enabled = true;
    }

    private void listAccounts()
    {
        flAccounts.Controls.Clear();
        var accounts = _loginHandler.AccountManager.GetAccounts();
        foreach (var account in accounts)
        {
            if (account is not JEGameAccount jeGameAccount)
                continue;

            var control = new AccountControl(jeGameAccount);
            control.LoginClicked += Control_LoginClicked;
            control.RemoveClicked += Control_RemoveClicked;
            flAccounts.Controls.Add(control);
        }

        lbNoAccountInfo.Visible = (flAccounts.Controls.Count == 0);
    }

    private async Task tryAutoLogin()
    {
        try
        {
            var result = await _loginHandler.AuthenticateSilently();
            showLauncherForm(result);
        }
        catch (MicrosoftOAuthException)
        {
            // auto login failed
        }
    }

    private async void btnNewAccount_Click(object sender, EventArgs e)
    {
        this.Enabled = false;
        try
        {
            var result = await _loginHandler.AuthenticateInteractively();
            showLauncherForm(result);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }

        this.Enabled = true;
    }

    private async void Control_LoginClicked(object? sender, EventArgs e)
    {
        if (sender is not AccountControl control)
            return;
        this.Enabled = false;
        try
        {
            var selectedAccount = control.Account ?? throw new InvalidOperationException();
            var result = await _loginHandler.Authenticate(selectedAccount);
            showLauncherForm(result);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        this.Enabled = true;
    }

    private void Control_RemoveClicked(object? sender, EventArgs e)
    {
        if (sender is not AccountControl control)
            return;

        this.Enabled = false;
        try
        {
            var selectedAccount = control.Account ?? throw new InvalidOperationException();
            _loginHandler.Signout(selectedAccount);
            listAccounts();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        this.Enabled = true;
    }

    private void showLauncherForm(MSession session)
    {
        var form = new LauncherForm(session);
        form.Show();

        exitOnClose = false;
        this.Close();
    }

    private void AccountForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (exitOnClose)
            Environment.Exit(0);
    }
}