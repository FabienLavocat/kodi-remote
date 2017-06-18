using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KodiRemote.Core;
using KodiRemote.Uwp.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp
{
    public sealed partial class PageServers : Page
    {
        private readonly ResourceLoader _resourceLoader;

        public PageServers()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;

            _resourceLoader = ResourceLoader.GetForCurrentView();
        }

        #region Context

        public Context Context
        {
            get { return (Context)GetValue(ContextProperty); }
            private set { SetValue(ContextProperty, value); }
        }

        public static readonly DependencyProperty ContextProperty =
            DependencyProperty.Register(nameof(Context), typeof(Context), typeof(PageServers), new PropertyMetadata(null));

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.Context.Connections.Any())
            {
                Frame.Navigate(typeof(PageWelcome));
                return;
            }

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = new Windows.UI.Color() { R = 76, G = 155, B = 214 };
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.White;
            }

            bool hasPageWelcome = Frame.BackStack.Any(je => je.SourcePageType == typeof(PageWelcome));
            if (hasPageWelcome)
            {
                Frame.BackStack.Clear();
            }

            Context = App.Context;

            foreach (var kodiConnection in App.Context.Connections)
                Task.Factory.StartNew(kodiConnection.TestConnectionAsync);
        }

        private void TapEdit(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as FrameworkElement;
            if (menuItem == null) return;
            var cnx = menuItem.DataContext as KodiConnection;
            if (cnx == null) return;

            Frame.Navigate(typeof(PageSettings), cnx.Id);
        }

        private async void TapRemove(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as FrameworkElement;
            if (menuItem == null) return;
            var cnx = menuItem.DataContext as KodiConnection;
            if (cnx == null) return;

            var okCommand = new UICommand(_resourceLoader.GetString("DialogOK"));
            var cancelCommand = new UICommand(_resourceLoader.GetString("DialogCancel"));
            var dialog = new MessageDialog(_resourceLoader.GetString("/servers/RemoveConfirm"), _resourceLoader.GetString("ApplicationTitle"));
            dialog.Commands.Add(okCommand);
            dialog.Commands.Add(cancelCommand);
            
            IUICommand command = await dialog.ShowAsync();
            if (command == cancelCommand) return;

            App.Context.Connections.Remove(cnx);
            if (!App.Context.Connections.Any())
            {
                Frame.BackStack.Clear();
                Frame.Navigate(typeof(PageWelcome));
            }

            App.Context.Save();
        }

        private async void TapWakeOnLan(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as FrameworkElement;
            if (menuItem == null) return;
            var cnx = menuItem.DataContext as KodiConnection;
            if (cnx == null
                || cnx.Kodi.IsMocked
                || string.IsNullOrWhiteSpace(cnx.Kodi.MacAddress))
                return;

            try
            {
                cnx.Kodi.WakeUp();
            }
            catch (Exception)
            {
                var dialog = new MessageDialog(_resourceLoader.GetString("GlobalErrorMessage"), _resourceLoader.GetString("ApplicationTitle"));
                await dialog.ShowAsync();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageSettings), "new");
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageAbout));
        }
        
        #region SelectCommand

        private ICommand _selectCommand;

        public ICommand SelectCommand
        {
            get { return _selectCommand ?? (_selectCommand = new Command(GoToMainPage)); }
        }

        private void GoToMainPage(object o)
        {
            var cnx = o as KodiConnection;
            if (cnx == null) return;

            Frame.Navigate(typeof(MainPage), cnx.Id);
        }

        #endregion
    }
}
