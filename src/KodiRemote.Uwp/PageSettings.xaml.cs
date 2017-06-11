using System;
using System.Linq;
using KodiRemote.Uwp.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp
{
    public sealed partial class PageSettings : Page
    {
        private bool _newConnection;
        private KodiConnection _connection;

        public PageSettings()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        #region Address

        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            private set { SetValue(AddressProperty, value); }
        }

        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register(nameof(Address), typeof(string), typeof(PageSettings), new PropertyMetadata(null));

        #endregion

        #region Port

        public string Port
        {
            get { return (string)GetValue(PortProperty); }
            private set { SetValue(PortProperty, value); }
        }

        public static readonly DependencyProperty PortProperty =
            DependencyProperty.Register(nameof(Port), typeof(string), typeof(PageSettings), new PropertyMetadata(null));

        #endregion

        #region Login

        public string Login
        {
            get { return (string)GetValue(LoginProperty); }
            private set { SetValue(LoginProperty, value); }
        }

        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register(nameof(Login), typeof(string), typeof(PageSettings), new PropertyMetadata(null));

        #endregion

        #region Password

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            private set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(nameof(Password), typeof(string), typeof(PageSettings), new PropertyMetadata(null));

        #endregion

        #region MacAddress

        public string MacAddress
        {
            get { return (string)GetValue(MacAddressProperty); }
            private set { SetValue(MacAddressProperty, value); }
        }

        public static readonly DependencyProperty MacAddressProperty =
            DependencyProperty.Register(nameof(MacAddress), typeof(string), typeof(PageSettings), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(PageSettings), new PropertyMetadata(false));

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = new Windows.UI.Color() { R = 40, G = 42, B = 43 };
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.White;
            }

            string parameter = e.Parameter.ToString();
            if ("new".Equals(parameter, StringComparison.OrdinalIgnoreCase))
            {
                _newConnection = true;
                _connection = new KodiConnection { Kodi = KodiRemote.Core.Connection.Default() };
            }
            else
            {
                _newConnection = false;
                _connection = App.Context.Connections.FirstOrDefault(c => c.Id.Equals(parameter, StringComparison.OrdinalIgnoreCase));
            }

            Address = _connection.Kodi.Address;
            Port = _connection.Kodi.Port;
            Login = _connection.Kodi.Login;
            Password = _connection.Kodi.Password;
            MacAddress = _connection.Kodi.MacAddress;

            DataContext = this;
        }

        private bool AreInformationValid()
        {
            if (string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(Port))
                return false;

            _connection.Kodi.Address = Address;
            _connection.Kodi.Port = Port;
            _connection.Kodi.Login = Login;
            _connection.Kodi.Password = Password;
            _connection.Kodi.MacAddress = MacAddress;

            return true;
        }

        private async void TestButtonClick(object sender, RoutedEventArgs e)
        {
            if (!AreInformationValid())
                return;

            IsLoading = true;

            try
            {
                var result = await _connection.TestConnectionAsync();

                var resourceLoader = ResourceLoader.GetForCurrentView();
                string message = result ? resourceLoader.GetString("/settings/TestGood") : resourceLoader.GetString("/settings/TestBad");
                var dialog = new MessageDialog(message, resourceLoader.GetString("/settings/ConnectivityTest.Content"));
                await dialog.ShowAsync();

                App.Context.Save();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (!AreInformationValid())
                return;

            if (_newConnection)
                App.Context.Connections.Add(_connection);

            App.Context.Save();
            
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}
