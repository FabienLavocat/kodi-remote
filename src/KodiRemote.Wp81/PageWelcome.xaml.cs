using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace KodiRemote.Wp81
{
    public partial class PageWelcome : PhoneApplicationPage
    {
        public PageWelcome()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.Context.Connections.Any())
            {
                NavigationService.Navigate(new Uri("/PageServers.xaml", UriKind.Relative));
                return;
            }
        }

        private void Add_Button_Click(object o, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageSettings.xaml?new", UriKind.Relative));
        }
    }
}