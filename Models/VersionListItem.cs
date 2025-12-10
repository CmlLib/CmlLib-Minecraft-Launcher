using CmlLib.Core.VersionMetadata;

namespace CmlLib_Minecraft_Launcher.Models;

internal sealed record VersionListItem(string Name, MVersionType Type, DateTimeOffset ReleaseTime)
{
    public string DisplayText => $"{Name,-20} [{Type,-10}] {FormatDate()}";

    private string FormatDate()
    {
        try
        {
            return ReleaseTime.ToString("yyyy-MM-dd");
        }
        catch
        {
            return "Unknown";
        }
    }
}

