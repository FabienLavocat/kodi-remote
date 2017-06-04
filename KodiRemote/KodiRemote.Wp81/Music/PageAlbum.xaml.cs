using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using KodiRemote.Core.Commands;
using KodiRemote.Core.Model;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.Music
{
    public partial class PageAlbum
    {
        #region ImageHeader

        public string ImageHeader
        {
            get { return (string) GetValue(ImageHeaderProperty); }
            private set { SetValue(ImageHeaderProperty, value); }
        }

        public static readonly DependencyProperty ImageHeaderProperty =
            DependencyProperty.Register("ImageHeader", typeof(string), typeof(PageAlbum), new PropertyMetadata(null));

        #endregion

        #region ImageVertical

        public string ImageVertical
        {
            get { return (string) GetValue(ImageVerticalProperty); }
            private set { SetValue(ImageVerticalProperty, value); }
        }

        public static readonly DependencyProperty ImageVerticalProperty =
            DependencyProperty.Register("ImageVertical", typeof(string), typeof(PageAlbum), new PropertyMetadata(null));

        #endregion

        #region AlbumTitle

        public string AlbumTitle
        {
            get { return (string)GetValue(AlbumTitleProperty); }
            set { SetValue(AlbumTitleProperty, value); }
        }

        public static readonly DependencyProperty AlbumTitleProperty =
            DependencyProperty.Register("AlbumTitle", typeof(string), typeof(PageAlbum), new PropertyMetadata(null));

        #endregion

        #region Album

        public AudioDetailsAlbum Album
        {
            get { return (AudioDetailsAlbum) GetValue(AlbumProperty); }
            set { SetValue(AlbumProperty, value); }
        }

        public static readonly DependencyProperty AlbumProperty =
            DependencyProperty.Register("Album", typeof(AudioDetailsAlbum), typeof(PageAlbum), new PropertyMetadata(null));

        #endregion

        #region Genres

        public string Genres
        {
            get { return (string) GetValue(GenresProperty); }
            private set { SetValue(GenresProperty, value); }
        }

        public static readonly DependencyProperty GenresProperty =
            DependencyProperty.Register("Genres", typeof(string), typeof(PageAlbum), new PropertyMetadata(null));

        #endregion

        #region Songs

        public List<AudioDetailsSong> Songs
        {
            get { return (List<AudioDetailsSong>)GetValue(SongsProperty); }
            set { SetValue(SongsProperty, value); }
        }

        public static readonly DependencyProperty SongsProperty =
            DependencyProperty.Register("Songs", typeof(List<AudioDetailsSong>), typeof(PageAlbum), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageAlbum), new PropertyMetadata(false));

        #endregion

        private int _albumId;

        public PageAlbum()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string id;
            if (!NavigationContext.QueryString.TryGetValue("id", out id)
                || !int.TryParse(id, out _albumId))
            {
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();

                return;
            }
            
            IsLoading = true;

            try
            {
                Album = await App.Context.Connection.Xbmc.AudioLibrary.GetAlbumDetailsAsync(_albumId);

                Genres = Helpers.Combine(Album.Genre);

                var songs = await App.Context.Connection.Xbmc.AudioLibrary.GetSongsAsync(_albumId);
                Songs = songs?.Songs?.OrderBy(s => s.Track).ToList();
            }
            catch (Exception ex)
            {
                App.TrackException(ex);
                MessageBox.Show(AppResources.Global_Error_Message, AppResources.ApplicationTitle, MessageBoxButton.OK);
            }
            finally
            {
                IsLoading = false;
            }

            if (Album != null && !string.IsNullOrWhiteSpace(Album.Thumbnail))
                ImageVertical = await Helpers.LoadImageUrl(Album.Thumbnail);

            if (Album != null && App.Context.DownloadFanArt)
                ImageHeader = await Helpers.LoadImageUrl(Album.FanArt);
        }

        #region SongCommand

        private ICommand _seasonCommand;

        public ICommand SongCommand
        {
            get { return _seasonCommand ?? (_seasonCommand = new Command(PlaySong)); }
        }

        private async void PlaySong(object o)
        {
            var song = o as AudioDetailsSong;
            if (song == null) return;

            // Start playing the song
            Helpers.Vibrate();
            await App.Context.Connection.Xbmc.Player.OpenAsync(song.SongId);
        }

        #endregion
        
        private async void Play_Button_Click(object sender, EventArgs e)
        {
            if (App.Context.Connection.Xbmc.IsMocked) return;

            IsLoading = true;
            try
            {
                await App.Context.Connection.Xbmc.Player.OpenAsync(albumId: _albumId);
                NavigationService.Navigate(new Uri("/PageRemote.xaml", UriKind.Relative));
                return;
            }
            catch { }

            IsLoading = false;
        }

        private async void Playlist_Button_Click(object sender, EventArgs e)
        {
            IsLoading = true;

            try
            {
                var playlists = await App.Context.Connection.Xbmc.Playlist.GetPlaylistsAsync();
                var playlist = playlists.First(p => p.Type == PlaylistType.audio);
                await App.Context.Connection.Xbmc.Playlist.AddAsync(playlist.PlaylistId, albumId: _albumId);
            }
            catch { }

            IsLoading = false;
        }

        private void Remote_Button_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageRemote.xaml", UriKind.Relative));
        }
    }
}