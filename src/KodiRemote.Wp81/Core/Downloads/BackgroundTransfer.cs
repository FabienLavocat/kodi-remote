using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.BackgroundTransfer;
using Windows.Storage;
using KodiRemote.Core.Model;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.Core.Downloads
{
    internal static class BackgroundTransfer
    {
        public static event DownloadStateEventHandler BackgroundTransferRequestStateChanged;

        public static async Task InitiateBackgroundTransferAsync(string path)
        {
            try
            {
                string filename = path.Substring(path.LastIndexOf("/") + 1);
                PrepareDownload download = await App.Context.Connection.Xbmc.Files.PrepareDownloadAsync(path);
                Uri sourceUri = App.Context.Connection.Xbmc.GetFileUri(download.Details.Path);

                const string targetDirectory = "/shared/transfers/";

                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isoStore.DirectoryExists(targetDirectory))
                        isoStore.CreateDirectory(targetDirectory);
                }

                Uri downloadUri = new Uri(targetDirectory + filename, UriKind.RelativeOrAbsolute);

                var request = new BackgroundTransferRequest(sourceUri, downloadUri);
                request.Tag = filename;
                request.TransferProgressChanged += OnTransferProgressChanged;
                request.TransferStatusChanged += OnTransferStatusChanged;

                BackgroundTransferService.Add(request);
                BackgroundTransferRequestStateChanged?.Raise(request, DownloadRequestState.Initialized);

                string message = string.Format(AppResources.Global_Downloads_Started_Format, filename);
                MessageBox.Show(message, AppResources.Global_Downloads, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                App.TrackException(ex);
            }
        }

        public static void InitializeEvents()
        {
            foreach (BackgroundTransferRequest request in BackgroundTransferService.Requests)
            {
                request.TransferProgressChanged += OnTransferProgressChanged;
                request.TransferStatusChanged += OnTransferStatusChanged;
            }
        }

        private static void OnTransferProgressChanged(object sender, BackgroundTransferEventArgs e)
        {
            var request = sender as BackgroundTransferRequest;
            if (request == null) return;

            BackgroundTransferRequestStateChanged?.Raise(request, DownloadRequestState.Downloading);
        }

        private static async void OnTransferStatusChanged(object sender, BackgroundTransferEventArgs e)
        {
            switch (e.Request.TransferStatus)
            {
                case TransferStatus.Completed:
                    if (e.Request.TransferError == null)
                    {
                        bool success = await CopyFileAsync(e.Request);
                        if (success)
                            RemoveTransferRequest(e.Request.RequestId);
                    }
                    else
                        BackgroundTransferRequestStateChanged?.Raise(e.Request, DownloadRequestState.Error);
                    break;

                case TransferStatus.WaitingForExternalPower:
                case TransferStatus.WaitingForExternalPowerDueToBatterySaverMode:
                case TransferStatus.WaitingForNonVoiceBlockingNetwork:
                case TransferStatus.WaitingForWiFi:
                    BackgroundTransferRequestStateChanged?.Raise(e.Request, DownloadRequestState.Paused);
                    break;
            }
        }

        private static async Task<bool> CopyFileAsync(BackgroundTransferRequest transfer)
        {
            BackgroundTransferRequestStateChanged?.Raise(transfer, DownloadRequestState.Copying);

            try
            {
                StorageFile destinationFile = await GetUniqueFileAsync(transfer.Tag);

                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    // Original file
                    IsolatedStorageFileStream sourceStream = isoStore.OpenFile(transfer.DownloadLocation.OriginalString, FileMode.Open);

                    // Target file
                    Stream targetStream = await destinationFile.OpenStreamForWriteAsync();

                    using (StreamReader reader = new StreamReader(sourceStream))
                    using (StreamWriter writer = new StreamWriter(targetStream))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            await writer.WriteLineAsync(line);
                        }
                    }

                    BackgroundTransferRequestStateChanged?.Raise(transfer, DownloadRequestState.Removing);
                    isoStore.DeleteFile(transfer.DownloadLocation.OriginalString);
                }

                return true;
            }
            catch (Exception ex)
            {
                App.TrackException(ex);
                BackgroundTransferRequestStateChanged?.Raise(transfer, DownloadRequestState.Error);
                return false;
            }
        }

        private static async Task<StorageFile> GetUniqueFileAsync(string originalFilename, int count = 0)
        {
            string newFilename = originalFilename;
            if (count > 0)
            {
                string ext = originalFilename.Substring(originalFilename.LastIndexOf('.'));
                string file = originalFilename.Substring(0, originalFilename.LastIndexOf('.'));
                newFilename = $"{file} ({++count}){ext}";
            }

            var files = await KnownFolders.VideosLibrary.GetFilesAsync();
            if (!files.Any(f => f.Path.EndsWith(newFilename)))
                return await KnownFolders.VideosLibrary.CreateFileAsync(newFilename);

            return await GetUniqueFileAsync(originalFilename, ++count);
        }

        private static void RemoveTransferRequest(string requestId)
        {
            BackgroundTransferRequest transferToRemove = null;

            try
            {
                // Use Find to retrieve the transfer request with the specified ID.
                transferToRemove = BackgroundTransferService.Find(requestId);
                if (transferToRemove == null) return;
                
                // Try to remove the transfer from the background transfer service.
                BackgroundTransferService.Remove(transferToRemove);

                BackgroundTransferRequestStateChanged?.Raise(transferToRemove, DownloadRequestState.Completed);
            }
            catch (Exception ex)
            {
                App.TrackException(ex);
                BackgroundTransferRequestStateChanged?.Raise(transferToRemove, DownloadRequestState.Error);
            }
        }

        private static void Raise(this DownloadStateEventHandler handler, BackgroundTransferRequest request, DownloadRequestState state)
        {
            if (request != null && handler != null)
                handler(request, new EventArgsState(state));
        }
    }
}
