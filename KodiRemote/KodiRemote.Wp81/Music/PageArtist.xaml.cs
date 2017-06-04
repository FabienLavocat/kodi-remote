using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using KodiRemote.Core.Model;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.Music
{
    public partial class PageArtist
    {
        #region ImageHeader

        public string ImageHeader
        {
            get { return (string) GetValue(ImageHeaderProperty); }
            private set { SetValue(ImageHeaderProperty, value); }
        }

        public static readonly DependencyProperty ImageHeaderProperty =
            DependencyProperty.Register("ImageHeader", typeof(string), typeof(PageArtist), new PropertyMetadata(null));

        #endregion

        #region ImageVertical

        public string ImageVertical
        {
            get { return (string) GetValue(ImageVerticalProperty); }
            private set { SetValue(ImageVerticalProperty, value); }
        }

        public static readonly DependencyProperty ImageVerticalProperty =
            DependencyProperty.Register("ImageVertical", typeof(string), typeof(PageArtist), new PropertyMetadata(null));

        #endregion

        #region Artist

        public AudioDetailsArtist Artist
        {
            get { return (AudioDetailsArtist) GetValue(ArtistProperty); }
            set { SetValue(ArtistProperty, value); }
        }

        public static readonly DependencyProperty ArtistProperty =
            DependencyProperty.Register("Artist", typeof(AudioDetailsArtist), typeof(PageArtist), new PropertyMetadata(null));

        #endregion

        #region Albums

        public List<ExtendedAudioDetailsAlbum> Albums
        {
            get { return (List<ExtendedAudioDetailsAlbum>) GetValue(AlbumsProperty); }
            private set { SetValue(AlbumsProperty, value); }
        }

        public static readonly DependencyProperty AlbumsProperty =
            DependencyProperty.Register("Albums", typeof(List<ExtendedAudioDetailsAlbum>), typeof(PageArtist), new PropertyMetadata(null));

        #endregion

        #region Songs

        public List<ExtendedAudioDetailsSong> Songs
        {
            get { return (List<ExtendedAudioDetailsSong>) GetValue(SongsProperty); }
            set { SetValue(SongsProperty, value); }
        }

        public static readonly DependencyProperty SongsProperty =
            DependencyProperty.Register("Songs", typeof(List<ExtendedAudioDetailsSong>), typeof(PageArtist), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool) GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageArtist), new PropertyMetadata(false));

        #endregion

        public PageArtist()
        {
            InitializeComponent();
            DataContext = this;

            MainPivot.SelectionChanged += PivotSelectionChanged;
            AlbumsBorder.Tap += (s, e) => ChangePivotIndex(0);
            SongsBorder.Tap += (s, e) => ChangePivotIndex(1);
        }

        #region Handle the pivot

        private void PivotSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (MainPivot.SelectedIndex == 0)
            {
                AlbumsBorder.Style = Resources["PivotHeaderSelectedStyle"] as Style;
                SongsBorder.Style = Resources["PivotHeaderUnSelectedStyle"] as Style;
            }
            else if (MainPivot.SelectedIndex == 1)
            {
                AlbumsBorder.Style = Resources["PivotHeaderUnSelectedStyle"] as Style;
                SongsBorder.Style = Resources["PivotHeaderSelectedStyle"] as Style;
            }
        }

        private void ChangePivotIndex(int index)
        {
            if (MainPivot.SelectedIndex != index)
                MainPivot.SelectedIndex = index;
        }

        #endregion

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string id;
            int intId;
            if (!NavigationContext.QueryString.TryGetValue("id", out id)
                || !int.TryParse(id, out intId))
            {
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();

                return;
            }

            IsLoading = true;

            try
            {
                Artist = await App.Context.Connection.Xbmc.AudioLibrary.GetArtistDetailsAsync(intId);

                var albums = await App.Context.Connection.Xbmc.AudioLibrary.GetAlbumsAsync(intId);
                Albums = albums.Albums.OrderByDescending(a => a.Year).Select(a => new ExtendedAudioDetailsAlbum(a)).ToList();

                var songs = await App.Context.Connection.Xbmc.AudioLibrary.GetSongsAsync(artistId: intId);
                Songs = songs.Songs.OrderBy(s => s.Title).Select(s => new ExtendedAudioDetailsSong(s)).ToList();
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

            if (Artist != null && !string.IsNullOrWhiteSpace(Artist.Thumbnail))
                ImageVertical = await Helpers.LoadImageUrl(Artist.Thumbnail);

            if (Artist != null && App.Context.DownloadFanArt)
                ImageHeader = await Helpers.LoadImageUrl(Artist.FanArt);
        }

        private void AlbumItemTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe == null) return;

            var album = fe.DataContext as ExtendedAudioDetailsAlbum;
            if (album == null) return;

            NavigationService.Navigate(new Uri("/Music/PageAlbum.xaml?id=" + album.Value.AlbumId, UriKind.Relative));
        }

        private async void SongItemTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe == null) return;

            var song = fe.DataContext as ExtendedAudioDetailsSong;
            if (song == null) return;

            Helpers.Vibrate();
            // Start playing the song
            await App.Context.Connection.Xbmc.Player.OpenAsync(song.Value.SongId);
        }

        private void Remote_Button_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageRemote.xaml", UriKind.Relative));
        }
    }
}