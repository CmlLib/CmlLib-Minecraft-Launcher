using CmlLib_Minecraft_Launcher.App;

namespace CmlLib_Minecraft_Launcher;

internal static class Program
{
    private const string ClientId = "499c8d36-be2a-4231-9ebd-ef291b7bb64c";

    public static async Task Main()
    {
        var app = new LauncherApp(ClientId);
        await app.RunAsync();
    }
}
