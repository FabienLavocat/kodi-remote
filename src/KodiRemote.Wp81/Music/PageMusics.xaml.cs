using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Navigation;
using KodiRemote.Core.Commands;
using KodiRemote.Core.Model;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.Music
{
    public partial class PageMusics
    {
        #region Artists

        public List<Group<AudioDetailsArtist>> Artists
        {
            get { return (List<Group<AudioDetailsArtist>>)GetValue(ArtistsProperty); }
            private set { SetValue(ArtistsProperty, value); }
        }

        public static readonly DependencyProperty ArtistsProperty =
            DependencyProperty.Register("Artists", typeof(List<Group<AudioDetailsArtist>>), typeof(PageMusics), new PropertyMetadata(null));

        #endregion

        #region Albums

        public List<Group<ExtendedAudioDetailsAlbum>> Albums
        {
            get { return (List<Group<ExtendedAudioDetailsAlbum>>)GetValue(AlbumsProperty); }
            private set { SetValue(AlbumsProperty, value); }
        }

        public static readonly DependencyProperty AlbumsProperty =
            DependencyProperty.Register("Albums", typeof(List<Group<ExtendedAudioDetailsAlbum>>), typeof(PageMusics), new PropertyMetadata(null));

        #endregion

        #region Genres

        public List<LibraryDetailsGenre> Genres
        {
            get { return (List<LibraryDetailsGenre>)GetValue(GenresProperty); }
            private set { SetValue(GenresProperty, value); }
        }

        public static readonly DependencyProperty GenresProperty =
            DependencyProperty.Register("Genres", typeof(List<LibraryDetailsGenre>), typeof(PageMusics), new PropertyMetadata(null));

        #endregion

        public PageMusics()
        {
            InitializeComponent();
            DataContext = this;

            MusicPivot.SelectionChanged += PivotSelectionChanged;
            ArtistsBorder.Tap += (s, e) => ChangePivotIndex(0);
            AlbumsBorder.Tap += (s, e) => ChangePivotIndex(1);
            GenresBorder.Tap += (s, e) => ChangePivotIndex(2);
        }

        #region Handle the pivot
        
        private void PivotSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (MusicPivot.SelectedIndex == 0)
            {
                ArtistsBorder.Style = Resources["PivotHeaderSelectedStyle"] as Style;
                AlbumsBorder.Style = Resources["PivotHeaderUnSelectedStyle"] as Style;
                GenresBorder.Style = Resources["PivotHeaderUnSelectedStyle"] as Style;
            }
            else if (MusicPivot.SelectedIndex == 1)
            {
                ArtistsBorder.Style = Resources["PivotHeaderUnSelectedStyle"] as Style;
                AlbumsBorder.Style = Resources["PivotHeaderSelectedStyle"] as Style;
                GenresBorder.Style = Resources["PivotHeaderUnSelectedStyle"] as Style;
            }
            else if (MusicPivot.SelectedIndex == 2)
            {
                ArtistsBorder.Style = Resources["PivotHeaderUnSelectedStyle"] as Style;
                AlbumsBorder.Style = Resources["PivotHeaderUnSelectedStyle"] as Style;
                GenresBorder.Style = Resources["PivotHeaderSelectedStyle"] as Style;
            }

        }

        private void ChangePivotIndex(int index)
        {
            if (MusicPivot.SelectedIndex != index)
                MusicPivot.SelectedIndex = index;
        }

        #endregion

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Artists != null && Albums != null && Genres != null) return;

            try
            {
                var artists = await App.Context.Connection.Xbmc.AudioLibrary.GetArtistsAsync();
                if (artists.Artists == null)
                    throw new Exception(AppResources.Page_Music_Message_No_Musique);

                Artists = Group.CreateGroups(artists.Artists, a => Group.GetGroupKey(a.ArtistName));
            }
            catch (Exception ex)
            {
                ArtistsBorder.Visibility = Visibility.Collapsed;
                App.TrackException(ex);
                MessageBox.Show(AppResources.Global_Error_Message, AppResources.ApplicationTitle, MessageBoxButton.OK);
            }

            try
            {
                var albums = await App.Context.Connection.Xbmc.AudioLibrary.GetAlbumsAsync(
                    fields: new[] { AudioFieldsAlbum.thumbnail, AudioFieldsAlbum.title, AudioFieldsAlbum.displayartist });
                if (albums.Albums == null)
                    throw new Exception(AppResources.Page_Music_Message_No_Musique);

                var extendedAlbums = albums.Albums.Select(a => new ExtendedAudioDetailsAlbum(a));
                Albums = Group.CreateGroups(extendedAlbums, a => Group.GetGroupKey(a.Value.Title));
            }
            catch (Exception ex)
            {
                AlbumsBorder.Visibility = Visibility.Collapsed;
                App.TrackException(ex);
                MessageBox.Show(AppResources.Global_Error_Message, AppResources.ApplicationTitle, MessageBoxButton.OK);
            }

            try
            {
                var genres = await App.Context.Connection.Xbmc.AudioLibrary.GetGenresAsync(fields: LibraryFieldsGenre.title);
                if (genres.Genres == null)
                    throw new Exception(AppResources.Page_Music_Message_No_Musique);
                Genres = genres.Genres.ToList();
            }
            catch (Exception ex)
            {
                GenresBorder.Visibility = Visibility.Collapsed;
                App.TrackException(ex);
                MessageBox.Show(AppResources.Global_Error_Message, AppResources.ApplicationTitle, MessageBoxButton.OK);
            }
        }

        private void Artist_Button_Click(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe == null) return;

            var artist = fe.DataContext as AudioDetailsArtist;
            if (artist == null) return;

            NavigationService.Navigate(new Uri("/Music/PageArtist.xaml?id=" + artist.ArtistId, UriKind.Relative));
        }

        private void Album_Button_Click(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe == null) return;

            var album = fe.DataContext as ExtendedAudioDetailsAlbum;
            if (album == null) return;

            NavigationService.Navigate(new Uri("/Music/PageAlbum.xaml?id=" + album.Value.AlbumId, UriKind.Relative));
        }

        private void Genre_Button_Click(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe == null) return;

            var genre = fe.DataContext as LibraryDetailsGenre;
            if (genre == null) return;

            string url = string.Concat("/Music/PageGenre.xaml?id=", genre.GenreId, "&title=", HttpUtility.UrlEncode(genre.Title));

            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }

        private void Remote_Button_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageRemote.xaml", UriKind.Relative));
        }
    }
}