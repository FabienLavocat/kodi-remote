namespace KodiRemote.Wp81.Core.Downloads
{
    public enum DownloadRequestState
    {
        Initialized,
        Downloading,
        Paused,
        Copying,
        Removing,
        Completed,
        Error
    }
}
