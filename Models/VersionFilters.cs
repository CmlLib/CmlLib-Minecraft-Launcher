using CmlLib.Core.VersionMetadata;

namespace CmlLib_Minecraft_Launcher.Models;

internal readonly record struct VersionFilters(
    bool Custom,
    bool Release,
    bool Snapshot,
    bool OldAlpha,
    bool OldBeta)
{
    public bool Allows(MVersionType type) =>
        (Custom && type == MVersionType.Custom) ||
        (Release && type == MVersionType.Release) ||
        (Snapshot && type == MVersionType.Snapshot) ||
        (OldAlpha && type == MVersionType.OldAlpha) ||
        (OldBeta && type == MVersionType.OldBeta);
}

