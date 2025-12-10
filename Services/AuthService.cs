using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using XboxAuthNet.Game.Msal;
using XboxAuthNet.Game.Msal.OAuth;

namespace CmlLib_Minecraft_Launcher.Services;

internal sealed class AuthService
{
    private readonly string _clientId;

    public AuthService(string clientId)
    {
        _clientId = clientId;
    }

    public async Task<MSession> AuthenticateAsync()
    {
        if (string.IsNullOrWhiteSpace(_clientId))
            return MSession.CreateOfflineSession("Player");

        var app = await MsalClientHelper.BuildApplicationWithCache(_clientId);
        var loginHandler = new JELoginHandlerBuilder()
            .WithOAuthProvider(new MsalCodeFlowProvider(app))
            .Build();

        try
        {
            return await loginHandler.AuthenticateSilently();
        }
        catch
        {
            return await loginHandler.AuthenticateInteractively();
        }
    }
}

