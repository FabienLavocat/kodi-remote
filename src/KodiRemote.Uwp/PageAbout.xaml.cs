using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp
{
    public sealed partial class PageAbout : Page
    {
        private readonly ResourceLoader _resourceLoader;
        private readonly DataTransferManager _dataTransferManager;

        public PageAbout()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;

            _resourceLoader = ResourceLoader.GetForCurrentView();

            // Display the version number
            string appTitle = _resourceLoader.GetString("ApplicationTitle");
            TxtApplicationTitle.Text = $"{appTitle} {GetCurrentVersion()}";
            
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = new Windows.UI.Color() { R = 95, G = 166, B = 165 };
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.White;
            }
        }

        private static string GetCurrentVersion()
        {
            try
            {
                PackageVersion pv = Package.Current.Id.Version;
                return string.Format("v{0}.{1}", pv.Major, pv.Minor);
            }
            catch (Exception)
            {
                return "vError";
            }
        }

        private async void BlogButtonClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.fabienlavocat.com"));
        }

        private async void DownloadButtonClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://kodi.tv"));
        }

        private async void ReviewButtonClick(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("ms-windows-store:reviewapp?appid=9wzdncrdqz3d");
            await Launcher.LaunchUriAsync(uri);
        }

        private void ShareButtonClick(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataPackage requestData = e.Request.Data;
            requestData.Properties.Title = _resourceLoader.GetString("ApplicationTitle");
            requestData.Properties.ContentSourceApplicationLink = new Uri("https://www.microsoft.com/store/apps/9wzdncrdqz3d");
            
            requestData.SetText(_resourceLoader.GetString("/about/ShareMessage") + " https://www.microsoft.com/store/apps/9wzdncrdqz3d");
        }

        private async void TwitterButtonClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://twitter.com/FabienLavocat"));
        }

        private async void FacebookButtonClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.facebook.com/mediacenterremote/"));
        }

        private async void GitHubButtonClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/FabienLavocat/kodi-remote"));
        }
    }
}
