using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.BackgroundTransfer;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Core.Downloads;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.Settings
{
    public partial class PageDownloads
    {
        #region Requests

        public ObservableCollection<BackgroundDownload> Requests
        {
            get { return (ObservableCollection<BackgroundDownload>) GetValue(RequestsProperty); }
            private set { SetValue(RequestsProperty, value); }
        }

        public static readonly DependencyProperty RequestsProperty =
            DependencyProperty.Register("Requests", typeof(ObservableCollection<BackgroundDownload>), typeof(PageDownloads), new PropertyMetadata(null));

        #endregion

        public PageDownloads()
        {
            InitializeComponent();
            Requests = new ObservableCollection<BackgroundDownload>();
            DataContext = this;
            BackgroundTransfer.BackgroundTransferRequestStateChanged += OnBackgroundTransferRequestStateChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Requests.Clear();
            
            foreach (var r in BackgroundTransferService.Requests)
            {
                var bd = new BackgroundDownload
                {
                    Id = r.RequestId,
                    Filename = r.Tag,
                    BytesReceived = r.BytesReceived,
                    TotalBytesToReceive = r.TotalBytesToReceive
                };

                if (bd.TotalBytesToReceive == -1)
                    bd.TotalBytesToReceive = bd.BytesReceived * 2;

                Requests.Add(bd);
            }

            TxtNothingToDownload.Visibility = Requests.Any() ? Visibility.Collapsed : Visibility.Visible;
        }

        private void OnBackgroundTransferRequestStateChanged(BackgroundTransferRequest request, EventArgsState e)
        {
            BackgroundDownload download = Requests.FirstOrDefault(r => r.Id == request.RequestId);
            if (download == null) return;

            download.Status = GetStatusText(e.State);

            if (e.State == DownloadRequestState.Downloading)
            {
                download.BytesReceived = request.BytesReceived;

                if (download.TotalBytesToReceive == -1)
                    download.TotalBytesToReceive = download.BytesReceived * 2;
            }

            if (e.State == DownloadRequestState.Completed)
                Requests.Remove(download);

            TxtNothingToDownload.Visibility = Requests.Any() ? Visibility.Collapsed : Visibility.Visible;
        }

        private string GetStatusText(DownloadRequestState status)
        {
            switch (status)
            {
                case DownloadRequestState.Initialized:
                    return AppResources.Page_Downloads_Status_Initialized;
                case DownloadRequestState.Downloading:
                    return AppResources.Page_Downloads_Status_Downloading;
                case DownloadRequestState.Paused:
                    return AppResources.Page_Downloads_Status_Paused;
                case DownloadRequestState.Copying:
                    return AppResources.Page_Downloads_Status_Copying;
                case DownloadRequestState.Removing:
                    return AppResources.Page_Downloads_Status_Removing;
                case DownloadRequestState.Completed:
                    return AppResources.Page_Downloads_Status_Completed;
                case DownloadRequestState.Error:
                    return AppResources.Page_Downloads_Status_Error;
                default:
                    return status.ToString();
            }
        }

        #region DeleteRequestCommand

        private ICommand _deleteRequestCommand;

        public ICommand DeleteRequestCommand
        {
            get { return _deleteRequestCommand ?? (_deleteRequestCommand = new Command(DeleteRequest)); }
        }

        private void DeleteRequest(object o)
        {
            var download = o as BackgroundDownload;
            if (download == null) return;

            try
            {
                BackgroundTransferRequest transferToRemove = BackgroundTransferService.Find(download.Id);
                if (transferToRemove == null) return;

                // Try to remove the transfer from the background transfer service.
                BackgroundTransferService.Remove(transferToRemove);

                // Remove the request from the UI
                Requests.Remove(download);
                TxtNothingToDownload.Visibility = Requests.Any() ? Visibility.Collapsed : Visibility.Visible;
            }
            catch (Exception ex)
            {
                App.TrackException(ex);
            }
        }

        #endregion
    }
}