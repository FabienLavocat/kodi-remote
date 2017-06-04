using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81
{
    public partial class PageSettings
    {
        #region Address

        public string Address
        {
            get { return (string) GetValue(AddressProperty); }
            private set { SetValue(AddressProperty, value); }
        }

        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register("Address", typeof(string), typeof(PageSettings), new PropertyMetadata(null));

        #endregion

        #region Port

        public string Port
        {
            get { return (string) GetValue(PortProperty); }
            private set { SetValue(PortProperty, value); }
        }

        public static readonly DependencyProperty PortProperty =
            DependencyProperty.Register("Port", typeof(string), typeof(PageSettings), new PropertyMetadata(null));

        #endregion

        #region Login

        public string Login
        {
            get { return (string) GetValue(LoginProperty); }
            private set { SetValue(LoginProperty, value); }
        }

        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register("Login", typeof(string), typeof(PageSettings), new PropertyMetadata(null));

        #endregion

        #region Password

        public string Password
        {
            get { return (string) GetValue(PasswordProperty); }
            private set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PageSettings), new PropertyMetadata(null));

        #endregion

        #region MacAddress

        public string MacAddress
        {
            get { return (string) GetValue(MacAddressProperty); }
            private set { SetValue(MacAddressProperty, value); }
        }

        public static readonly DependencyProperty MacAddressProperty =
            DependencyProperty.Register("MacAddress", typeof(string), typeof(PageSettings), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool) GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageSettings), new PropertyMetadata(false));

        #endregion

        private bool _newConnection;
        private XbmcConnection _connection;

        public PageSettings()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string url = e.Uri.ToString();

            if (url.Contains("new"))
            {
                _newConnection = true;
                _connection = new XbmcConnection { Xbmc = KodiRemote.Core.Connection.Default() };
            }
            else
            {
                int index = url.IndexOf('?');
                _newConnection = false;
                _connection = App.Context.Connections.FirstOrDefault(c => c.Id == url.Substring(index + 1));
            }

            Address = _connection.Xbmc.Address;
            Port = _connection.Xbmc.Port;
            Login = _connection.Xbmc.Login;
            Password = _connection.Xbmc.Password;
            MacAddress = _connection.Xbmc.MacAddress;

            DataContext = this;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (!App.Context.Connections.Any())
            {
                try
                {
                    while (NavigationService.RemoveBackEntry() != null) { }
                }
                catch (InvalidOperationException) { }
            }

            base.OnBackKeyPress(e);
        }

        private bool AreInformationValid()
        {
            if (string.IsNullOrWhiteSpace(Address)
                || string.IsNullOrWhiteSpace(Port))
                return false;

            _connection.Xbmc.Address = Address;
            _connection.Xbmc.Port = Port;
            _connection.Xbmc.Login = Login;
            _connection.Xbmc.Password = Password;
            _connection.Xbmc.MacAddress = MacAddress;

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

                string message = result ? AppResources.Page_Settings_Test_Good : AppResources.Page_Settings_Test_Bad;
                MessageBox.Show(message, AppResources.Page_Settings_Connectivity_Test, MessageBoxButton.OK);

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

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}