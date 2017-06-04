using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using KodiRemote.Core.Model;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.TvShows
{
    public partial class PageTvShow : PhoneApplicationPage
    {
        #region TvShowTitle

        public string TvShowTitle
        {
            get { return (string) GetValue(TvShowTitleProperty); }
            private set { SetValue(TvShowTitleProperty, value); }
        }

        public static readonly DependencyProperty TvShowTitleProperty =
            DependencyProperty.Register("TvShowTitle", typeof(string), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region Studio

        public string Studio
        {
            get { return (string) GetValue(StudioProperty); }
            private set { SetValue(StudioProperty, value); }
        }

        public static readonly DependencyProperty StudioProperty =
            DependencyProperty.Register("Studio", typeof(string), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region Genres

        public string Genres
        {
            get { return (string) GetValue(GenresProperty); }
            private set { SetValue(GenresProperty, value); }
        }

        public static readonly DependencyProperty GenresProperty =
            DependencyProperty.Register("Genres", typeof(string), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool) GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageTvShow), new PropertyMetadata(false));

        #endregion

        #region TvShow

        public VideoDetailsTvShow TvShow
        {
            get { return (VideoDetailsTvShow) GetValue(TvShowProperty); }
            private set { SetValue(TvShowProperty, value); }
        }

        public static readonly DependencyProperty TvShowProperty =
            DependencyProperty.Register("TvShow", typeof(VideoDetailsTvShow), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region Seasons

        public ObservableCollection<VideoDetailsSeason> Seasons
        {
            get { return (ObservableCollection<VideoDetailsSeason>) GetValue(SeasonsProperty); }
            private set { SetValue(SeasonsProperty, value); }
        }

        public static readonly DependencyProperty SeasonsProperty =
            DependencyProperty.Register("Seasons", typeof(ObservableCollection<VideoDetailsSeason>), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region Cast

        public ObservableCollection<ExtendedVideoCast> Cast
        {
            get { return (ObservableCollection<ExtendedVideoCast>) GetValue(CastProperty); }
            private set { SetValue(CastProperty, value); }
        }

        public static readonly DependencyProperty CastProperty =
            DependencyProperty.Register("Cast", typeof(ObservableCollection<ExtendedVideoCast>), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region ImageHeader

        public string ImageHeader
        {
            get { return (string) GetValue(ImageHeaderProperty); }
            private set { SetValue(ImageHeaderProperty, value); }
        }

        public static readonly DependencyProperty ImageHeaderProperty =
            DependencyProperty.Register("ImageHeader", typeof(string), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region ImageVertical

        public string ImageVertical
        {
            get { return (string) GetValue(ImageVerticalProperty); }
            private set { SetValue(ImageVerticalProperty, value); }
        }

        public static readonly DependencyProperty ImageVerticalProperty =
            DependencyProperty.Register("ImageVertical", typeof(string), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region Rating

        public double Rating
        {
            get { return (double) GetValue(RatingProperty); }
            private set { SetValue(RatingProperty, value); }
        }

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register("Rating", typeof(double), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region Votes

        public string Votes
        {
            get { return (string) GetValue(VotesProperty); }
            private set { SetValue(VotesProperty, value); }
        }

        public static readonly DependencyProperty VotesProperty =
            DependencyProperty.Register("Votes", typeof(string), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        public PageTvShow()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string setTitle;
            string id;
            int tvShowId;
            if (!NavigationContext.QueryString.TryGetValue("id", out id)
                || !NavigationContext.QueryString.TryGetValue("title", out setTitle)
                || !int.TryParse(id, out tvShowId))
            {
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();

                return;
            }

            Cast = new ObservableCollection<ExtendedVideoCast>();
            Seasons = new ObservableCollection<VideoDetailsSeason>();
            TvShowTitle = HttpUtility.UrlDecode(setTitle);
            IsLoading = true;

            try
            {
                if (App.Context.Connection.Xbmc.IsMocked)
                {
                    #region Mocked
                    TvShow = new VideoDetailsTvShow
                    {
                        TvShowId = 1,
                        Rating = 9,
                        Genre = new[] { "Comedy" },
                        Studio = new[] { "NBC" },
                        ImdbNumber = "tt0072562",
                        Thumbnail = "http://thetvdb.com/banners/_cache/posters/76177-4.jpg",
                        FanArt = "http://thetvdb.com/banners/fanart/original/76177-6.jpg",
                        Title = "Saturday Night Live",
                        Plot =
                                "A weekly late-night 90-minute American sketch comedy/variety show broadcast live from Studio 8H at the GE Building in New York's Rockefeller Center. The show is one of the longest-running network programs in American television history and has launched careers for many major American comedy stars of the last thirty years.",
                        Art = new MediaArtwork { Banner = "http://thetvdb.com/banners/_cache/graphical/76177-g5.jpg" },
                        Cast = new[]
                                {
                                    new VideoCast
                                        {
                                            Name = "Tina Fey",
                                            Role = "Artist",
                                            Thumbnail =
                                                "http://ia.media-imdb.com/images/M/MV5BMTU3NzMwMDI2MF5BMl5BanBnXkFtZTcwNDk0MzcyNw@@._V1._SX214_CR0,0,214,314_.jpg"
                                        }
                                }
                    };
                    #endregion
                }
                else
                {
                    var tvShow = await App.Context.Connection.Xbmc.VideoLibrary.GetTvShowDetailsAsync(tvShowId);
                    TvShow = tvShow.TvShowDetails;

                    var seasons = await App.Context.Connection.Xbmc.VideoLibrary.GetSeasonsAsync(tvShowId);
                    foreach (VideoDetailsSeason season in seasons.Seasons)
                        Seasons.Add(season);
                }

                Studio = Helpers.Combine(TvShow.Studio);
                Genres = Helpers.Combine(TvShow.Genre);
                Rating = TvShow.Rating / 2;
                Votes = string.Format(AppResources.Page_Tv_Shows_Votes_Format, TvShow.Votes);
                if (TvShow.ImdbNumber == null)
                    ButtonSeeImdb.Visibility = Visibility.Collapsed;

                foreach (var cast in TvShow.Cast.Take(5))
                    Cast.Add(new ExtendedVideoCast(cast));
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

            if (TvShow != null && !string.IsNullOrWhiteSpace(TvShow.Thumbnail))
                ImageVertical = await Helpers.LoadImageUrl(TvShow.Thumbnail);

            if (TvShow != null && App.Context.DownloadFanArt)
                ImageHeader = await Helpers.LoadImageUrl(TvShow.FanArt);
        }

        #region SeasonCommand

        private ICommand _seasonCommand;

        public ICommand SeasonCommand
        {
            get { return _seasonCommand ?? (_seasonCommand = new Command(NavigateToSeason)); }
        }

        private void NavigateToSeason(object o)
        {
            var season = o as VideoDetailsSeason;
            if (season == null) return;

            string title = HttpUtility.UrlEncode(TvShow.Title) + " - " + season.Label;
            string url = string.Concat("/TvShows/PageEpisodes.xaml?tvid=", TvShow.TvShowId, "&season=", season.Season, "&title=", title);

            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }

        #endregion

        private void Imdb_Button_Click(object sender, EventArgs e)
        {
            if (TvShow == null || TvShow.ImdbNumber == null) return;

            string url = string.Concat("http://www.imdb.com/title/", TvShow.ImdbNumber);
            var wbt = new WebBrowserTask { Uri = new Uri(url, UriKind.Absolute) };
            wbt.Show();
        }

        private void Cast_Button_Click(object sender, RoutedEventArgs e)
        {
            if (TvShow == null) return;

            string url = string.Concat("/PageCast.xaml?id=", TvShow.TvShowId, "&type=tvshow");

            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }

        private void Remote_Button_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageRemote.xaml", UriKind.Relative));
        }
    }
}