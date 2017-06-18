using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp
{
    public sealed partial class PageWelcome : Page
    {
        public PageWelcome()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;

            Loaded += PageWelcome_Loaded;
        }

        private void PageWelcome_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connections.Any())
            {
                Frame.Navigate(typeof(PageServers));
                return;
            }

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = new Windows.UI.Color() { R = 40, G = 42, B = 43 };
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.White;
            }
        }

        private void AddButton_Click(object o, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageSettings), "new");
        }
    }
}
