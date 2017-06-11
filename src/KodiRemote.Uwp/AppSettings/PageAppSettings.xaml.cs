using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp.AppSettings
{
    public sealed partial class PageAppSettings : Page
    {
        public PageAppSettings()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = new Windows.UI.Color() { R = 137, G = 182, B = 90 };
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.White;
            }

            TgDownloadFanArt.IsOn = App.Context.DownloadFanArt;
            TgDownloadThumbnails.IsOn = App.Context.DownloadThumbnails;
            TgAllowVibrate.IsOn = App.Context.AllowVibrate;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            App.Context.DownloadFanArt = TgDownloadFanArt.IsOn;
            App.Context.DownloadThumbnails = TgDownloadThumbnails.IsOn;
            App.Context.AllowVibrate = TgAllowVibrate.IsOn;
        }

        //private void DownloadButtonClick(object sender, RoutedEventArgs e)
        //{
        //    Frame.Navigate(typeof(PageDownloads));
        //}
    }
}
