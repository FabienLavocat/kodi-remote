using System;
using System.Reflection;
using Microsoft.Phone.Tasks;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81
{
    public partial class PageAbout
    {
        public PageAbout()
        {
            InitializeComponent();

            // Display the version number
            TxtApplicationTitle.Text = string.Concat(AppResources.ApplicationTitle, " ", GetCurrentVersion());
        }

        private static void OpenWebBrowser(string url)
        {
            var wbt = new WebBrowserTask { Uri = new Uri(url, UriKind.Absolute) };
            wbt.Show();
        }

        /// <summary>
        /// Retrieve the current version of the executing assembly.
        /// This is the one in the AssemblyVersion attribute.
        /// </summary>
        private static string GetCurrentVersion()
        {
            try
            {
                string assembly = Assembly.GetExecutingAssembly().FullName;
                string fullVersionNumber = assembly.Split('=')[1].Split(',')[0];
                var version = new Version(fullVersionNumber);
                return string.Format("v{0}.{1}", version.Major, version.Minor);
            }
            catch (Exception)
            {
                return "v1.error";
            }
        }

        private void Blog_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenWebBrowser("https://www.fabienlavocat.com");
        }

        private void Download_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenWebBrowser("https://kodi.tv/");
        }

        private void Review_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new MarketplaceReviewTask().Show();
        }

        private void Share_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var slt = new ShareLinkTask
            {
                Title = AppResources.ApplicationTitle,
                Message = AppResources.Page_About_Share_Message,
                LinkUri = new Uri("https://www.windowsphone.com/s?appid=7cdb0b87-1e00-4328-b839-43a6bf9c8556", UriKind.Absolute)
            };
            slt.Show();
        }

        private void Twitter_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenWebBrowser("https://twitter.com/FabienLavocat");
        }

        private void Facebook_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenWebBrowser("https://www.facebook.com/mediacenterremote/");
        }

        private void GitHub_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenWebBrowser("https://github.com/FabienLavocat/kodi-remote");
        }
    }
}