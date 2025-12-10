using CmlLib.Core;
using CmlLib_Minecraft_Launcher.Controllers;
using CmlLib_Minecraft_Launcher.Services;
using CmlLib_Minecraft_Launcher.UI;
using Terminal.Gui;

namespace CmlLib_Minecraft_Launcher.App;

internal sealed class LauncherApp
{
    private readonly string _clientId;

    public LauncherApp(string clientId)
    {
        _clientId = clientId;
    }

    public Task RunAsync()
    {
        Application.Init();
        Colors.Base.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black);
        Colors.Base.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Cyan);

        var top = Application.Top;
        var path = new MinecraftPath();
        var launcher = new MinecraftLauncher(path);

        var mainWindow = new MainWindow();
        var controller = new LauncherController(mainWindow, new LauncherService(launcher), new AuthService(_clientId));

        top.Add(mainWindow);
        top.Loaded += () => controller.Start();

        Application.Run(top);
        Application.Shutdown();

        return Task.CompletedTask;
    }
}

