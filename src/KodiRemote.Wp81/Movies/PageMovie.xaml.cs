using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;
using MyToolkit.Multimedia;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Core.Downloads;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.Movies
{
    public partial class PageMovie
    {
        #region MovieTitle

        public string MovieTitle
        {
            get { return (string)GetValue(MovieTitleProperty); }
            set { SetValue(MovieTitleProperty, value); }
        }

        public static readonly DependencyProperty MovieTitleProperty =
            DependencyProperty.Register("MovieTitle", typeof(string), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region ImageUrl

        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            private set { SetValue(ImageUrlProperty, value); }
        }

        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register("ImageUrl", typeof(string), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Movie

        public ExtendedVideoDetailsMovie Movie
        {
            get { return (ExtendedVideoDetailsMovie)GetValue(MovieProperty); }
            private set { SetValue(MovieProperty, value); }
        }

        public static readonly DependencyProperty MovieProperty =
            DependencyProperty.Register("Movie", typeof(ExtendedVideoDetailsMovie), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Cast

        public ObservableCollection<ExtendedVideoCast> Cast
        {
            get { return (ObservableCollection<ExtendedVideoCast>)GetValue(CastProperty); }
            private set { SetValue(CastProperty, value); }
        }

        public static readonly DependencyProperty CastProperty =
            DependencyProperty.Register("Cast", typeof(ObservableCollection<ExtendedVideoCast>), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Genres

        public string Genres
        {
            get { return (string)GetValue(GenresProperty); }
            private set { SetValue(GenresProperty, value); }
        }

        public static readonly DependencyProperty GenresProperty =
            DependencyProperty.Register("Genres", typeof(string), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Director

        public string Director
        {
            get { return (string)GetValue(DirectorProperty); }
            private set { SetValue(DirectorProperty, value); }
        }

        public static readonly DependencyProperty DirectorProperty =
            DependencyProperty.Register("Director", typeof(string), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Studio

        public string Studio
        {
            get { return (string)GetValue(StudioProperty); }
            private set { SetValue(StudioProperty, value); }
        }

        public static readonly DependencyProperty StudioProperty =
            DependencyProperty.Register("Studio", typeof(string), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Rating

        public double Rating
        {
            get { return (double)GetValue(RatingProperty); }
            private set { SetValue(RatingProperty, value); }
        }

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register("Rating", typeof(double), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Votes

        public string Votes
        {
            get { return (string) GetValue(VotesProperty); }
            private set { SetValue(VotesProperty, value); }
        }

        public static readonly DependencyProperty VotesProperty =
            DependencyProperty.Register("Votes", typeof(string), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region Minutes

        public int Minutes
        {
            get { return (int) GetValue(MinutesProperty); }
            private set { SetValue(MinutesProperty, value); }
        }

        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes", typeof(int), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region TrailerVisibility

        public Visibility TrailerVisibility
        {
            get { return (Visibility) GetValue(TrailerVisibilityProperty); }
            private set { SetValue(TrailerVisibilityProperty, value); }
        }

        public static readonly DependencyProperty TrailerVisibilityProperty =
            DependencyProperty.Register("TrailerVisibility", typeof(Visibility), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region ImageTrailer

        public Uri ImageTrailer
        {
            get { return (Uri) GetValue(ImageTrailerProperty); }
            private set { SetValue(ImageTrailerProperty, value); }
        }

        public static readonly DependencyProperty ImageTrailerProperty =
            DependencyProperty.Register("ImageTrailer", typeof(Uri), typeof(PageMovie), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool) GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageMovie), new PropertyMetadata(false));

        #endregion

        public PageMovie()
        {
            InitializeComponent();
            DataContext = this;
        }

        private string _youtubeId;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string movieTitle;
            string id;
            int intId;
            if (!NavigationContext.QueryString.TryGetValue("id", out id)
                || !NavigationContext.QueryString.TryGetValue("title", out movieTitle)
                || !int.TryParse(id, out intId))
            {
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();

                return;
            }

            Cast = new ObservableCollection<ExtendedVideoCast>();
            MovieTitle = HttpUtility.UrlDecode(movieTitle);
            IsLoading = true;
            Genres = "";

            try
            {
                var movie = await App.Context.Connection.Xbmc.VideoLibrary.GetMovieDetailsAsync(intId);
                Movie = new ExtendedVideoDetailsMovie(movie, false);

                foreach (var cast in movie.Cast.Take(5))
                    Cast.Add(new ExtendedVideoCast(cast));

                Genres = Helpers.Combine(movie.Genre);
                Director = Helpers.Combine(movie.Director);
                Studio = Helpers.Combine(movie.Studio);
                Rating = movie.Rating/2;
                Votes = string.Format(AppResources.Page_Tv_Shows_Votes_Format, movie.Votes);
                Minutes = movie.Runtime / 60;
                if (Movie.Movie.ImdbNumber == null)
                    ButtonSeeImdb.Visibility = Visibility.Collapsed;

                TrailerVisibility = Visibility.Collapsed;
                if (!string.IsNullOrWhiteSpace(Movie.Movie.Trailer))
                {
                    TrailerVisibility = Visibility.Visible;
                    int index = Movie.Movie.Trailer.IndexOf("videoid=", StringComparison.Ordinal);
                    if (index < 0)
                        index = Movie.Movie.Trailer.IndexOf("video_id=", StringComparison.Ordinal);
                    _youtubeId = Movie.Movie.Trailer.Substring(index);
                    index = _youtubeId.IndexOf('=');
                    _youtubeId = _youtubeId.Substring(index + 1);

                    ImageTrailer = YouTube.GetThumbnailUri(_youtubeId);
                }
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

            if (Movie?.Movie != null && !string.IsNullOrWhiteSpace(Movie.Movie.Thumbnail))
                ImageUrl = await Helpers.LoadImageUrl(Movie.Movie.Thumbnail);
        }

        private void PlayTrailer(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var dico = new Dictionary<string, string>
                       {
                           {"Youtube ID", _youtubeId},
                           {"Movie Title", Movie.Movie.Title}
                       };

            Helpers.Vibrate();
            YouTube.PlayAsync(_youtubeId, YouTubeQuality.QualityHigh);
        }

        public async void Play_Button_Click(object sender, EventArgs e)
        {
            IsLoading = true;
            await App.Context.Connection.Xbmc.Player.OpenAsync(movieId: Movie.Movie.MovieId);
            IsLoading = false;

            NavigationService.Navigate(new Uri("/PageRemote.xaml", UriKind.Relative));
        }

        public void Imdb_Button_Click(object sender, EventArgs e)
        {
            if (Movie == null || Movie.Movie.ImdbNumber == null) return;

            string url = string.Concat("http://www.imdb.com/title/", Movie.Movie.ImdbNumber);
            var wbt = new WebBrowserTask { Uri = new Uri(url, UriKind.Absolute) };
            wbt.Show();
        }

        public void Cast_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Movie == null) return;

            string url = string.Concat("/PageCast.xaml?id=", Movie.Movie.MovieId, "&type=movie");

            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }
        
        public void Remote_Button_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageRemote.xaml", UriKind.Relative));
        }

        private async void Download_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Movie == null) return;

            await BackgroundTransfer.InitiateBackgroundTransferAsync(Movie.Movie.File);
        }
    }
}