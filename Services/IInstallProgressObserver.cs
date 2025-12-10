namespace CmlLib_Minecraft_Launcher.Services;

internal interface IInstallProgressObserver
{
    void OnFileProgress(int progressed, int total, string name);
    void OnByteProgress(long downloaded, long total, float fraction);
}

