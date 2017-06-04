using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Resources;
using KodiRemote.Core;
using System.Windows.Input;
using System.Threading.Tasks;

namespace KodiRemote.Wp81
{
    public partial class PageServers
    {
        #region Context

        public Context Context
        {
            get { return (Context)GetValue(ContextProperty); }
            private set { SetValue(ContextProperty, value); }
        }

        public static readonly DependencyProperty ContextProperty =
            DependencyProperty.Register("Context", typeof(Context), typeof(PageServers), new PropertyMetadata(null));

        #endregion

        public PageServers()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.Context.Connections.Any())
            {
                NavigationService.Navigate(new Uri("/PageWelcome.xaml", UriKind.Relative));
                return;
            }

            bool hasPageWelcome = NavigationService.BackStack.Any(je => je.Source.OriginalString.Contains("PageWelcome.xaml"));
            if (hasPageWelcome)
            {
                while (NavigationService.RemoveBackEntry() != null)
                {
                    ; // do nothing
                }
            }

            Context = App.Context;
            
            foreach (var xbmcConnection in App.Context.Connections)
                Task.Factory.StartNew(xbmcConnection.TestConnectionAsync);
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageSettings.xaml?new", UriKind.Relative));
        }

        private void About_Button_Click(object o, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageAbout.xaml", UriKind.Relative));
        }

        private void TapEdit(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as FrameworkElement;
            if (menuItem == null) return;
            var cnx = menuItem.DataContext as XbmcConnection;
            if (cnx == null) return;

            NavigationService.Navigate(new Uri("/PageSettings.xaml?" + cnx.Id, UriKind.Relative));
        }

        private void TapWakeOnLan(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as FrameworkElement;
            if (menuItem == null) return;
            var cnx = menuItem.DataContext as XbmcConnection;
            if (cnx == null
                || cnx.Xbmc.IsMocked
                || string.IsNullOrWhiteSpace(cnx.Xbmc.MacAddress))
                return;
            
            try
            {
                cnx.Xbmc.WakeUp();
            }
            catch (Exception ex)
            {
                App.TrackException(ex);
                MessageBox.Show(AppResources.Global_Error_Message, AppResources.ApplicationTitle, MessageBoxButton.OK);
            }
        }

        private void TapRemove(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as FrameworkElement;
            if (menuItem == null) return;
            var cnx = menuItem.DataContext as XbmcConnection;
            if (cnx == null) return;

            var response = MessageBox.Show(AppResources.Page_Servers_App_Bar_Remove_Confirm,
                                           AppResources.ApplicationTitle,
                                           MessageBoxButton.OKCancel);

            if (response == MessageBoxResult.Cancel) return;

            App.Context.Connections.Remove(cnx);
            if (!App.Context.Connections.Any())
            {
                while (NavigationService.RemoveBackEntry() != null)
                {
                    ; // do nothing
                }

                NavigationService.Navigate(new Uri("/PageWelcome.xaml", UriKind.Relative));
            }

            App.Context.Save();
        }

        #region SelectCommand

        private ICommand _selectCommand;

        public ICommand SelectCommand
        {
            get { return _selectCommand ?? (_selectCommand = new Command(GoToMainPage)); }
        }

        private void GoToMainPage(object o)
        {
            var cnx = o as XbmcConnection;
            if (cnx == null) return;

            NavigationService.Navigate(new Uri("/MainPage.xaml?" + cnx.Id, UriKind.Relative));
        }

        #endregion
    }
}