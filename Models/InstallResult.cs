namespace CmlLib_Minecraft_Launcher.Models;

internal sealed record InstallResult(bool Success, bool Cancelled, Exception? Error)
{
    public static InstallResult Successful() => new(true, false, null);
    public static InstallResult CancelledResult() => new(false, true, null);
    public static InstallResult Failed(Exception error) => new(false, false, error);
}

