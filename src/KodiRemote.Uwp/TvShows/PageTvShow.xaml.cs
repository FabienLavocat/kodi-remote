using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using KodiRemote.Core.Model;
using KodiRemote.Uwp.Core;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp.TvShows
{
    public sealed partial class PageTvShow : Page
    {
        private readonly ResourceLoader _resourceLoader;

        public PageTvShow()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;

            _resourceLoader = ResourceLoader.GetForCurrentView();
        }

        #region TvShowTitle

        public string TvShowTitle
        {
            get { return (string)GetValue(TvShowTitleProperty); }
            private set { SetValue(TvShowTitleProperty, value); }
        }

        public static readonly DependencyProperty TvShowTitleProperty =
            DependencyProperty.Register(nameof(TvShowTitle), typeof(string), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region Studio

        public string Studio
        {
            get { return (string)GetValue(StudioProperty); }
            private set { SetValue(StudioProperty, value); }
        }

        public static readonly DependencyProperty StudioProperty =
            DependencyProperty.Register(nameof(Studio), typeof(string), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region Genres

        public string Genres
        {
            get { return (string)GetValue(GenresProperty); }
            private set { SetValue(GenresProperty, value); }
        }

        public static readonly DependencyProperty GenresProperty =
            DependencyProperty.Register(nameof(Genres), typeof(string), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(PageTvShow), new PropertyMetadata(false));

        #endregion

        #region TvShow

        public VideoDetailsTvShow TvShow
        {
            get { return (VideoDetailsTvShow)GetValue(TvShowProperty); }
            private set { SetValue(TvShowProperty, value); }
        }

        public static readonly DependencyProperty TvShowProperty =
            DependencyProperty.Register(nameof(TvShow), typeof(VideoDetailsTvShow), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region Seasons

        public ObservableCollection<VideoDetailsSeason> Seasons
        {
            get { return (ObservableCollection<VideoDetailsSeason>)GetValue(SeasonsProperty); }
            private set { SetValue(SeasonsProperty, value); }
        }

        public static readonly DependencyProperty SeasonsProperty =
            DependencyProperty.Register(nameof(Seasons), typeof(ObservableCollection<VideoDetailsSeason>), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region Cast

        public ObservableCollection<ExtendedVideoCast> Cast
        {
            get { return (ObservableCollection<ExtendedVideoCast>)GetValue(CastProperty); }
            private set { SetValue(CastProperty, value); }
        }

        public static readonly DependencyProperty CastProperty =
            DependencyProperty.Register(nameof(Cast), typeof(ObservableCollection<ExtendedVideoCast>), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region ImageHeader

        public string ImageHeader
        {
            get { return (string)GetValue(ImageHeaderProperty); }
            private set { SetValue(ImageHeaderProperty, value); }
        }

        public static readonly DependencyProperty ImageHeaderProperty =
            DependencyProperty.Register(nameof(ImageHeader), typeof(string), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region ImageVertical

        public string ImageVertical
        {
            get { return (string)GetValue(ImageVerticalProperty); }
            private set { SetValue(ImageVerticalProperty, value); }
        }

        public static readonly DependencyProperty ImageVerticalProperty =
            DependencyProperty.Register(nameof(ImageVertical), typeof(string), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region Rating

        public double Rating
        {
            get { return (double)GetValue(RatingProperty); }
            private set { SetValue(RatingProperty, value); }
        }

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register(nameof(Rating), typeof(double), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        #region Votes

        public string Votes
        {
            get { return (string)GetValue(VotesProperty); }
            private set { SetValue(VotesProperty, value); }
        }

        public static readonly DependencyProperty VotesProperty =
            DependencyProperty.Register(nameof(Votes), typeof(string), typeof(PageTvShow), new PropertyMetadata(null));

        #endregion

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = new Windows.UI.Color() { R = 115, G = 143, B = 186 };
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.White;
            }

            int paramIndex = e.Parameter.ToString().IndexOf("|");
            string id = e.Parameter.ToString().Substring(0, paramIndex);
            TvShowTitle = e.Parameter.ToString().Substring(paramIndex + 1);

            Cast = new ObservableCollection<ExtendedVideoCast>();
            Seasons = new ObservableCollection<VideoDetailsSeason>();
            IsLoading = true;

            try
            {
                int tvShowId = int.Parse(id);

                if (App.Context.Connection.Kodi.IsMocked)
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
                    var tvShow = await App.Context.Connection.Kodi.VideoLibrary.GetTvShowDetailsAsync(tvShowId);
                    TvShow = tvShow.TvShowDetails;

                    var seasons = await App.Context.Connection.Kodi.VideoLibrary.GetSeasonsAsync(tvShowId);
                    foreach (VideoDetailsSeason season in seasons.Seasons)
                        Seasons.Add(season);
                }

                Studio = Helpers.Combine(TvShow.Studio);
                Genres = Helpers.Combine(TvShow.Genre);
                Rating = TvShow.Rating / 2;
                //Votes = string.Format(AppResources.Page_Tv_Shows_Votes_Format, TvShow.Votes);
                //if (TvShow.ImdbNumber == null)
                //    ButtonSeeImdb.Visibility = Visibility.Collapsed;

                foreach (var cast in TvShow.Cast)
                    Cast.Add(new ExtendedVideoCast(cast));
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

            if (TvShow != null && !string.IsNullOrWhiteSpace(TvShow.Thumbnail))
                ImageVertical = await Helpers.LoadImageUrl(TvShow.Thumbnail);

            if (TvShow != null && App.Context.DownloadFanArt)
                ImageHeader = await Helpers.LoadImageUrl(TvShow.FanArt);
        }

        public void CastButton_Click(object sender, RoutedEventArgs e)
        {
            if (TvShow == null) return;

            Frame.Navigate(typeof(PageCast), $"{TvShow.TvShowId}|tvshow");
        }
    }
}
