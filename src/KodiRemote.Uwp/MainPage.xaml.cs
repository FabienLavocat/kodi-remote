using System;
using System.Linq;
using System.Windows.Input;
using KodiRemote.Uwp.Addons;
using KodiRemote.Uwp.AppSettings;
using KodiRemote.Uwp.Core;
using KodiRemote.Uwp.Movies;
using KodiRemote.Uwp.Musics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp
{
    public sealed partial class MainPage : Page
    {
        private KodiConnection _connection = null;

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = new Windows.UI.Color() { R = 76, G = 155, B = 214 };
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.White;
            }

            _connection = App.Context.Connections.FirstOrDefault(c => c.Id.Equals(e.Parameter?.ToString(), StringComparison.OrdinalIgnoreCase));

            if (_connection != null)
            {
                if (_connection.Kodi.IsMocked)
                {
                    ButtonMovies.Visibility = Visibility.Collapsed;
                    ButtonMusic.Visibility = Visibility.Collapsed;
                    ButtonAddons.Visibility = Visibility.Collapsed;
                    ButtonPlaylists.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ButtonMovies.Visibility = Visibility.Visible;
                    ButtonMusic.Visibility = Visibility.Visible;
                    ButtonAddons.Visibility = Visibility.Visible;
                    ButtonPlaylists.Visibility = Visibility.Visible;
                }

                BtActions.DataContext = _connection;
                App.Context.SetDefaultConnection(_connection);
                App.Context.Save();

                await _connection.TestConnectionAsync();
                return;
            }

            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        #region ActionsCommand

        private ICommand _actionsCommand;

        public ICommand ActionsCommand
        {
            get { return _actionsCommand ?? (_actionsCommand = new Command(GoToActions)); }
        }

        private void GoToActions(object o)
        {
            // Frame.Navigate(typeof(PageActions), _connection.Id);
        }

        #endregion

        #region MusicsCommand

        private ICommand _musicsCommand;

        public ICommand MusicsCommand
        {
            get { return _musicsCommand ?? (_musicsCommand = new Command(GoToMusics)); }
        }

        private void GoToMusics(object o)
        {
            Frame.Navigate(typeof(PageMusics));
        }

        #endregion

        #region MoviesCommand

        private ICommand _moviesCommand;

        public ICommand MoviesCommand
        {
            get { return _moviesCommand ?? (_moviesCommand = new Command(GoToMovies)); }
        }

        private void GoToMovies(object o)
        {
            Frame.Navigate(typeof(PageMovies));
        }

        #endregion

        #region TvShowsCommand

        private ICommand _tvShowsCommand;

        public ICommand TvShowsCommand
        {
            get { return _tvShowsCommand ?? (_tvShowsCommand = new Command(GoToTvShows)); }
        }

        private void GoToTvShows(object o)
        {
            // Frame.Navigate(typeof(PageTvShows));
        }

        #endregion
        
        #region RemoteCommand

        private ICommand _remoteCommand;

        public ICommand RemoteCommand
        {
            get { return _remoteCommand ?? (_remoteCommand = new Command(GoToRemote)); }
        }

        private void GoToRemote(object o)
        {
            Frame.Navigate(typeof(PageRemote));
        }

        #endregion

        #region AddonsCommand

        private ICommand _addonsCommand;

        public ICommand AddonsCommand
        {
            get { return _addonsCommand ?? (_addonsCommand = new Command(GoToAddons)); }
        }

        private void GoToAddons(object o)
        {
            Frame.Navigate(typeof(PageAddons));
        }

        #endregion

        #region PlaylistsCommand

        private ICommand _playlistsCommand;

        public ICommand PlaylistsCommand
        {
            get { return _playlistsCommand ?? (_playlistsCommand = new Command(GoToPlaylists)); }
        }

        private void GoToPlaylists(object o)
        {
            // Frame.Navigate(typeof(PagePlaylists));
        }

        #endregion

        #region SettingsCommand

        private ICommand _settingsCommand;

        public ICommand SettingsCommand
        {
            get { return _settingsCommand ?? (_settingsCommand = new Command(GoToSettings)); }
        }

        private void GoToSettings(object o)
        {
            Frame.Navigate(typeof(PageAppSettings));
        }

        #endregion

        #region AboutCommand

        private ICommand _aboutCommand;

        public ICommand AboutCommand
        {
            get { return _aboutCommand ?? (_aboutCommand = new Command(GoToAbout)); }
        }

        private void GoToAbout(object o)
        {
            Frame.Navigate(typeof(PageAbout));
        }

        #endregion
    }
}
