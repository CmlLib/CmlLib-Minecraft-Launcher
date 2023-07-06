using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft.Sessions;

namespace MyCustomLauncher;

public partial class AccountControl : UserControl
{
    public JEGameAccount? Account { get; }

    private bool _readonly = false;
    public bool ReadOnly
    {
        get => _readonly;
        set => _readonly = updateReadOnly(value);
    }

    public event EventHandler? LoginClicked;
    public event EventHandler? RemoveClicked;

    public AccountControl(JEGameAccount account)
    {
        Account = account;
        InitializeComponent();

        lbUsername.Text = account.Profile?.Username;
        lbIdentifier.Text = account.Identifier;
        pbAvatar.ImageLocation = getAvatarUrl(account.Profile?.UUID);
    }

    public AccountControl(MSession session)
    {
        InitializeComponent();

        lbUsername.Text = session.Username;
        lbIdentifier.Text = session.UUID;
        pbAvatar.ImageLocation = getAvatarUrl(session.UUID);
        ReadOnly = true;
    }

    private string getAvatarUrl(string? uuid) => $"https://mc-heads.net/avatar/{uuid}";

    private bool updateReadOnly(bool value)
    {
        btnLogin.Visible = !value;
        btnRemove.Visible = !value;
        return value;
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
        LoginClicked?.Invoke(this, e);
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
        RemoveClicked?.Invoke(this, e);
    }
}
