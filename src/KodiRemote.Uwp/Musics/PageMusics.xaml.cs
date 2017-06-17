using System;
using System.Collections.Generic;
using System.Linq;
using KodiRemote.Core.Commands;
using KodiRemote.Core.Model;
using KodiRemote.Uwp.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp.Musics
{
    public sealed partial class PageMusics : Page
    {
        private readonly ResourceLoader _resourceLoader;

        public PageMusics()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;

            _resourceLoader = ResourceLoader.GetForCurrentView();
        }

        #region Artists

        public List<Group<AudioDetailsArtist>> Artists
        {
            get { return (List<Group<AudioDetailsArtist>>)GetValue(ArtistsProperty); }
            private set { SetValue(ArtistsProperty, value); }
        }

        public static readonly DependencyProperty ArtistsProperty =
            DependencyProperty.Register(nameof(Artists), typeof(List<Group<AudioDetailsArtist>>), typeof(PageMusics), new PropertyMetadata(null));

        #endregion

        #region Albums

        public List<Group<ExtendedAudioDetailsAlbum>> Albums
        {
            get { return (List<Group<ExtendedAudioDetailsAlbum>>)GetValue(AlbumsProperty); }
            private set { SetValue(AlbumsProperty, value); }
        }

        public static readonly DependencyProperty AlbumsProperty =
            DependencyProperty.Register(nameof(Albums), typeof(List<Group<ExtendedAudioDetailsAlbum>>), typeof(PageMusics), new PropertyMetadata(null));

        #endregion

        #region Genres

        public List<LibraryDetailsGenre> Genres
        {
            get { return (List<LibraryDetailsGenre>)GetValue(GenresProperty); }
            private set { SetValue(GenresProperty, value); }
        }

        public static readonly DependencyProperty GenresProperty =
            DependencyProperty.Register(nameof(Genres), typeof(List<LibraryDetailsGenre>), typeof(PageMusics), new PropertyMetadata(null));

        #endregion

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = Windows.UI.Colors.Black;
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.White;
            }

            if (Artists != null || Albums != null || Genres != null) return;

            try
            {
                var artists = await App.Context.Connection.Kodi.AudioLibrary.GetArtistsAsync();
                if (artists.Artists != null)
                    Artists = Group.CreateGroups(artists.Artists, a => Group.GetGroupKey(a.ArtistName));
            }
            catch (Exception ex)
            {
                MusicPivot.Items.Remove(PivotArtists);
                App.TrackException(ex);
                var dialog = new MessageDialog(_resourceLoader.GetString("GlobalErrorMessage"), _resourceLoader.GetString("ApplicationTitle"));
                await dialog.ShowAsync();
            }

            try
            {
                var albums = await App.Context.Connection.Kodi.AudioLibrary.GetAlbumsAsync(
                    fields: new[] { AudioFieldsAlbum.thumbnail, AudioFieldsAlbum.title, AudioFieldsAlbum.displayartist });
                if (albums.Albums != null)
                {
                    var extendedAlbums = albums.Albums.Select(a => new ExtendedAudioDetailsAlbum(a));
                    Albums = Group.CreateGroups(extendedAlbums, a => Group.GetGroupKey(a.Value.Title));
                }
            }
            catch (Exception ex)
            {
                MusicPivot.Items.Remove(PivotAlbums);
                App.TrackException(ex);
                var dialog = new MessageDialog(_resourceLoader.GetString("GlobalErrorMessage"), _resourceLoader.GetString("ApplicationTitle"));
                await dialog.ShowAsync();
            }

            try
            {
                var genres = await App.Context.Connection.Kodi.AudioLibrary.GetGenresAsync(fields: LibraryFieldsGenre.title);
                if (genres.Genres != null)
                    Genres = genres.Genres.ToList();
            }
            catch (Exception ex)
            {
                MusicPivot.Items.Remove(PivotGenres);
                App.TrackException(ex);
                var dialog = new MessageDialog(_resourceLoader.GetString("GlobalErrorMessage"), _resourceLoader.GetString("ApplicationTitle"));
                await dialog.ShowAsync();
            }
        }

        private void ArtistButton_Click(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var artist = fe?.DataContext as AudioDetailsArtist;
            if (artist == null) return;

            Frame.Navigate(typeof(PageArtist), artist.ArtistId);
        }

        private void AlbumButton_Click(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var album = fe?.DataContext as ExtendedAudioDetailsAlbum;
            if (album == null) return;

            Frame.Navigate(typeof(PageAlbum), album.Value.AlbumId);
        }

        private void GenreButton_Click(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var genre = fe?.DataContext as LibraryDetailsGenre;
            if (genre == null) return;
            
            Frame.Navigate(typeof(PageGenre), $"{genre.GenreId}|{genre.Title}");
        }

        private void RemoteButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageRemote));
        }
    }
}
