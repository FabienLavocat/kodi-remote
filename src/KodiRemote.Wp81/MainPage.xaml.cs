using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using KodiRemote.Wp81.Core;

namespace KodiRemote.Wp81
{
    public partial class MainPage
    {
        private XbmcConnection _connection = null;

        public MainPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string url = e.Uri.ToString();
            int index = url.IndexOf('?');
            _connection = App.Context.Connections.FirstOrDefault(c => c.Id == url.Substring(index + 1));

            if (_connection != null)
            {
                if (_connection.Xbmc.IsMocked)
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

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        #region ActionsCommand

        private ICommand _actionsCommand;

        public ICommand ActionsCommand
        {
            get { return _actionsCommand ?? (_actionsCommand = new Command(GoToActions)); }
        }

        private void GoToActions(object o)
        {
            NavigationService.Navigate(new Uri("/PageActions.xaml?" + _connection.Id, UriKind.Relative));
        }

        #endregion

        #region MusicCommand

        private ICommand _musicCommand;

        public ICommand MusicCommand
        {
            get { return _musicCommand ?? (_musicCommand = new Command(GoToMusic)); }
        }

        private void GoToMusic(object o)
        {
            NavigationService.Navigate(new Uri("/Music/PageMusics.xaml", UriKind.Relative));
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
            NavigationService.Navigate(new Uri("/Movies/PageMovies.xaml", UriKind.Relative));
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
            NavigationService.Navigate(new Uri("/TvShows/PageTvShows.xaml", UriKind.Relative));
        }

        #endregion

        #region PicturesCommand

        private ICommand _picturesCommand;

        public ICommand PicturesCommand
        {
            get { return _picturesCommand ?? (_picturesCommand = new Command(GoToPictures)); }
        }

        private void GoToPictures(object o)
        {

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
            NavigationService.Navigate(new Uri("/PageRemote.xaml", UriKind.Relative));
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
            NavigationService.Navigate(new Uri("/Addons/PageAddons.xaml", UriKind.Relative));
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
            NavigationService.Navigate(new Uri("/PagePlaylists.xaml", UriKind.Relative));
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
            NavigationService.Navigate(new Uri("/Settings/PageSettings.xaml", UriKind.Relative));
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
            NavigationService.Navigate(new Uri("/PageAbout.xaml", UriKind.Relative));
        }

        #endregion
    }
}