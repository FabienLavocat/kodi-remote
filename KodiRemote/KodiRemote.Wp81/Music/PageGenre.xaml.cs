using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Navigation;
using KodiRemote.Core.Model;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.Music
{
    public partial class PageGenre
    {
        #region GenreTitle

        public string GenreTitle
        {
            get { return (string)GetValue(GenreTitleProperty); }
            set { SetValue(GenreTitleProperty, value); }
        }

        public static readonly DependencyProperty GenreTitleProperty =
            DependencyProperty.Register("GenreTitle", typeof(string), typeof(PageGenre), new PropertyMetadata(null));

        #endregion

        #region Songs

        public List<ExtendedAudioDetailsSong> Songs
        {
            get { return (List<ExtendedAudioDetailsSong>)GetValue(SongsProperty); }
            set { SetValue(SongsProperty, value); }
        }

        public static readonly DependencyProperty SongsProperty =
            DependencyProperty.Register("Songs", typeof(List<ExtendedAudioDetailsSong>), typeof(PageGenre), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageGenre), new PropertyMetadata(false));

        #endregion

        public PageGenre()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string genreTitle;
            string id;
            int intId;
            if (!NavigationContext.QueryString.TryGetValue("id", out id)
                || !NavigationContext.QueryString.TryGetValue("title", out genreTitle)
                || !int.TryParse(id, out intId))
            {
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();

                return;
            }

            if (!App.Context.DownloadThumbnails)
                ListSongs.ItemTemplate = Resources["SongItemTemplateFlat"] as DataTemplate;

            GenreTitle = HttpUtility.UrlDecode(genreTitle).ToUpperInvariant();

            IsLoading = true;
            try
            {
                var songs = await App.Context.Connection.Xbmc.AudioLibrary.GetSongsAsync(genreId: intId);
                Songs = songs.Songs.Select(s => new ExtendedAudioDetailsSong(s)).ToList();
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