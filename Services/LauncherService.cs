using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Installers;
using CmlLib.Core.ProcessBuilder;
using CmlLib.Core.VersionMetadata;
using CmlLib_Minecraft_Launcher.Models;

namespace CmlLib_Minecraft_Launcher.Services;

internal sealed class LauncherService
{
    private readonly MinecraftLauncher _launcher;

    public LauncherService(MinecraftLauncher launcher)
    {
        _launcher = launcher;
    }

    public async Task<IReadOnlyList<VersionListItem>> GetAllVersionsAsync()
    {
        var versionCollection = await _launcher.GetAllVersionsAsync();
        var versions = new List<VersionListItem>();

        foreach (var version in versionCollection)
        {
            versions.Add(new VersionListItem(
                version.Name,
                version.GetVersionType(),
                version.ReleaseTime));
        }

        return versions;
    }

    public async Task<InstallResult> InstallAsync(
        string version,
        IInstallProgressObserver observer,
        CancellationToken cancellationToken)
    {
        EventHandler<InstallerProgressChangedEventArgs> fileProgressHandler = (sender, args) =>
        {
            observer.OnFileProgress(args.ProgressedTasks, args.TotalTasks, args.Name ?? string.Empty);
        };

        EventHandler<ByteProgress> byteProgressHandler = (sender, args) =>
        {
            if (args.TotalBytes <= 0)
                return;

            var fraction = (float)args.ToRatio();
            observer.OnByteProgress(args.ProgressedBytes, args.TotalBytes, fraction);
        };

        _launcher.FileProgressChanged += fileProgressHandler;
        _launcher.ByteProgressChanged += byteProgressHandler;

        try
        {
            await _launcher.InstallAsync(version, cancellationToken).AsTask();
            return InstallResult.Successful();
        }
        catch (OperationCanceledException)
        {
            return InstallResult.CancelledResult();
        }
        catch (Exception ex)
        {
            return InstallResult.Failed(ex);
        }
        finally
        {
            _launcher.FileProgressChanged -= fileProgressHandler;
            _launcher.ByteProgressChanged -= byteProgressHandler;
        }
    }

    public async Task<int> LaunchAsync(string version, MSession session, Action<string> logHandler)
    {
        var launchOption = new MLaunchOption
        {
            Session = session,
            MaximumRamMb = 2048,
            MinimumRamMb = 1024,
        };

        var process = await _launcher.BuildProcessAsync(version, launchOption);
        var processWrapper = new ProcessWrapper(process);

        processWrapper.OutputReceived += (_, log) =>
        {
            logHandler(log);
        };

        processWrapper.StartWithEvents();

        logHandler($"\n=== Minecraft Launched (PID: {process.Id}) ===");

        var exitCode = await processWrapper.WaitForExitTaskAsync();
        logHandler($"\n=== Game Exited (Code: {exitCode}) ===");
        return exitCode;
    }
}

