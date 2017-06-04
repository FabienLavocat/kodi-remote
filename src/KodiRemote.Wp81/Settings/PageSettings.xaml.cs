using System;
using System.Windows;
using System.Windows.Navigation;

namespace KodiRemote.Wp81.Settings
{
    public partial class PageSettings
    {
        public PageSettings()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TgDownloadFanArt.IsChecked = App.Context.DownloadFanArt;
            TgDownloadThumbnails.IsChecked = App.Context.DownloadThumbnails;
            TgAllowVibrate.IsChecked = App.Context.AllowVibrate;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.Context.DownloadFanArt = TgDownloadFanArt.IsChecked.HasValue && TgDownloadFanArt.IsChecked.Value;
            App.Context.DownloadThumbnails = TgDownloadThumbnails.IsChecked.HasValue && TgDownloadThumbnails.IsChecked.Value;
            App.Context.AllowVibrate = TgAllowVibrate.IsChecked.HasValue && TgAllowVibrate.IsChecked.Value;

            base.OnNavigatedFrom(e);
        }

        private void Download_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings/PageDownloads.xaml", UriKind.Relative));
        }
    }
}