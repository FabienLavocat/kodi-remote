using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using KodiRemote.Uwp.Core;
using MyToolkit.Multimedia;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp.Movies
{
    public sealed partial class PageMovie : Page
    {
        private readonly ResourceLoader _resourceLoader;

        public PageMovie()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;

            _resourceLoader = ResourceLoader.GetForCurrentView();
            
            SizeChanged += PageMovie_SizeChanged;
        }

        #region MovieTitle

        public string MovieTitle
        {
            get { return (string)GetValue(MovieTitleProperty); }
            set { SetValue(MovieTitleProperty, value); }
        }

        public static readonly DependencyProperty MovieTitleProperty =
            DependencyProperty.Register(nameof(MovieTitle), typeof(string), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region FanArt

        public string FanArt
        {
            get { return (string)GetValue(FanArtProperty); }
            private set { SetValue(FanArtProperty, value); }
        }

        public static readonly DependencyProperty FanArtProperty =
            DependencyProperty.Register(nameof(FanArt), typeof(string), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region ImageUrl

        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            private set { SetValue(ImageUrlProperty, value); }
        }

        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register(nameof(ImageUrl), typeof(string), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Movie

        public ExtendedVideoDetailsMovie Movie
        {
            get { return (ExtendedVideoDetailsMovie)GetValue(MovieProperty); }
            private set { SetValue(MovieProperty, value); }
        }

        public static readonly DependencyProperty MovieProperty =
            DependencyProperty.Register(nameof(Movie), typeof(ExtendedVideoDetailsMovie), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Cast

        public ObservableCollection<ExtendedVideoCast> Cast
        {
            get { return (ObservableCollection<ExtendedVideoCast>)GetValue(CastProperty); }
            private set { SetValue(CastProperty, value); }
        }

        public static readonly DependencyProperty CastProperty =
            DependencyProperty.Register(nameof(Cast), typeof(ObservableCollection<ExtendedVideoCast>), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Genres

        public string Genres
        {
            get { return (string)GetValue(GenresProperty); }
            private set { SetValue(GenresProperty, value); }
        }

        public static readonly DependencyProperty GenresProperty =
            DependencyProperty.Register(nameof(Genres), typeof(string), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Director

        public string Director
        {
            get { return (string)GetValue(DirectorProperty); }
            private set { SetValue(DirectorProperty, value); }
        }

        public static readonly DependencyProperty DirectorProperty =
            DependencyProperty.Register(nameof(Director), typeof(string), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Studio

        public string Studio
        {
            get { return (string)GetValue(StudioProperty); }
            private set { SetValue(StudioProperty, value); }
        }

        public static readonly DependencyProperty StudioProperty =
            DependencyProperty.Register(nameof(Studio), typeof(string), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Rating

        public double Rating
        {
            get { return (double)GetValue(RatingProperty); }
            private set { SetValue(RatingProperty, value); }
        }

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register(nameof(Rating), typeof(double), typeof(PageMovie), new PropertyMetadata(0));

        #endregion

        #region Votes

        public string Votes
        {
            get { return (string)GetValue(VotesProperty); }
            private set { SetValue(VotesProperty, value); }
        }

        public static readonly DependencyProperty VotesProperty =
            DependencyProperty.Register(nameof(Votes), typeof(string), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Minutes

        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            private set { SetValue(MinutesProperty, value); }
        }

        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register(nameof(Minutes), typeof(int), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region TrailerVisibility

        public Visibility TrailerVisibility
        {
            get { return (Visibility)GetValue(TrailerVisibilityProperty); }
            private set { SetValue(TrailerVisibilityProperty, value); }
        }

        public static readonly DependencyProperty TrailerVisibilityProperty =
            DependencyProperty.Register(nameof(TrailerVisibility), typeof(Visibility), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region ImageTrailer

        public Uri ImageTrailer
        {
            get { return (Uri)GetValue(ImageTrailerProperty); }
            private set { SetValue(ImageTrailerProperty, value); }
        }

        public static readonly DependencyProperty ImageTrailerProperty =
            DependencyProperty.Register(nameof(ImageTrailer), typeof(Uri), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region PlaybackList

        public MediaPlaybackList PlaybackList
        {
            get { return (MediaPlaybackList)GetValue(PlaybackListProperty); }
            private set { SetValue(PlaybackListProperty, value); }
        }

        public static readonly DependencyProperty PlaybackListProperty =
            DependencyProperty.Register(nameof(PlaybackList), typeof(MediaPlaybackList), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(PageMovie), new PropertyMetadata(false));

        #endregion
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = new Windows.UI.Color() { R = 153, G = 122, B = 165 };
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.White;
            }

            int paramIndex = e.Parameter.ToString().IndexOf("|");
            string id = e.Parameter.ToString().Substring(0, paramIndex);
            MovieTitle = e.Parameter.ToString().Substring(paramIndex + 1);

            Cast = new ObservableCollection<ExtendedVideoCast>();
            IsLoading = true;
            Genres = "";

            try
            {
                var movie = await App.Context.Connection.Kodi.VideoLibrary.GetMovieDetailsAsync(int.Parse(id));
                Movie = new ExtendedVideoDetailsMovie(movie, false);

                foreach (var cast in movie.Cast)
                    Cast.Add(new ExtendedVideoCast(cast));

                Genres = Helpers.Combine(movie.Genre);
                Director = Helpers.Combine(movie.Director);
                Studio = Helpers.Combine(movie.Studio);
                Rating = movie.Rating / 2;
                Votes = string.Format(_resourceLoader.GetString("/movies/VotesFormat"), movie.Votes);
                Minutes = movie.Runtime / 60;
                if (Movie.Movie.ImdbNumber == null)
                    ButtonSeeImdb.Visibility = Visibility.Collapsed;

                TrailerVisibility = Visibility.Collapsed;
                if (!string.IsNullOrWhiteSpace(Movie.Movie.Trailer))
                {
                    TrailerVisibility = Visibility.Visible;
                    Regex regex = new Regex(@"video[_]?id=(?<youtubeId>[^&]*)");
                    if (regex.IsMatch(Movie.Movie.Trailer))
                    {
                        Match match = regex.Match(Movie.Movie.Trailer);
                        if (match.Groups["youtubeId"].Success)
                        {
                            string youtubeId = match.Groups["youtubeId"].Value;
                            ImageTrailer = YouTube.GetThumbnailUri(youtubeId);

                            var urlTrailer = await YouTube.GetVideoUriAsync(youtubeId, YouTubeQuality.Quality1080P);
                            PlaybackList = new MediaPlaybackList();
                            PlaybackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(urlTrailer.Uri)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.TrackException(ex);
                var dialog = new MessageDialog(_resourceLoader.GetString("GlobalErrorMessage"), _resourceLoader.GetString("ApplicationTitle"));
                await dialog.ShowAsync();
            }
            finally
            {
                IsLoading = false;
            }

            if (!string.IsNullOrWhiteSpace(Movie?.Movie?.Thumbnail))
                ImageUrl = await Helpers.LoadImageUrl(Movie.Movie.Thumbnail);

            if (!string.IsNullOrWhiteSpace(Movie?.Movie?.FanArt))
                FanArt = await Helpers.LoadImageUrl(Movie.Movie.FanArt);
        }
        
        public async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            IsLoading = true;
            await App.Context.Connection.Kodi.Player.OpenAsync(movieId: Movie.Movie.MovieId);
            IsLoading = false;

            Frame.Navigate(typeof(PageRemote));
        }

        public async void ImdbButton_Click(object sender, RoutedEventArgs e)
        {
            if (Movie?.Movie.ImdbNumber == null) return;

            string url = string.Concat("http://www.imdb.com/title/", Movie.Movie.ImdbNumber);
            await Launcher.LaunchUriAsync(new Uri(url, UriKind.Absolute));
        }

        public void CastButton_Click(object sender, RoutedEventArgs e)
        {
            if (Movie == null) return;
            
            Frame.Navigate(typeof(PageCast), $"{Movie.Movie.MovieId}|movie");
        }

        public void RemoteButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageRemote));
        }

        private void MediaPlayer_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (sender is MediaPlayerElement mediaPlayer)
            {
                mediaPlayer.IsFullWindow = !mediaPlayer.IsFullWindow;
            }
        }

        private void MediaPlayer_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape)
                MediaPlayer.IsFullWindow = false;
        }

        private void PageMovie_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Page.BottomAppBar.Visibility = MediaPlayer.IsFullWindow ? Visibility.Collapsed : Visibility.Visible;
        }

        //private void DownloadButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (Movie == null) return;

        //    await BackgroundTransfer.InitiateBackgroundTransferAsync(Movie.Movie.File);
        //}
    }
}
