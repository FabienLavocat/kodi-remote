using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using KodiRemote.Core.Commands;
using KodiRemote.Core.Model;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.TvShows
{
    public partial class PageTvShows
    {
        #region TvShows

        public ObservableCollection<ExtendedVideoDetailsTvShow> TvShows
        {
            get { return (ObservableCollection<ExtendedVideoDetailsTvShow>) GetValue(TvShowsProperty); }
            private set { SetValue(TvShowsProperty, value); }
        }

        public static readonly DependencyProperty TvShowsProperty =
            DependencyProperty.Register("TvShows", typeof(ObservableCollection<ExtendedVideoDetailsTvShow>), typeof(PageTvShows), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool) GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageTvShows), new PropertyMetadata(false));

        #endregion

        public PageTvShows()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.Context.DownloadThumbnails)
            {
                LstTvShows.ItemTemplate = System.Windows.Application.Current.Resources["TvShowItemTemplateFlat"] as DataTemplate;
            }

            if (TvShows != null) return;

            TvShows = new ObservableCollection<ExtendedVideoDetailsTvShow>();

            LoadTvShowsAsync();
        }

        private bool _keepWatched = true;

        private async void LoadTvShowsAsync()
        {
            if (IsLoading) return;

            IsLoading = true;
            TvShows.Clear();

            if (App.Context.Connection.Xbmc.IsMocked)
            {
                var videoDetailsTvShow = new VideoDetailsTvShow
                {
                    TvShowId = 1,
                    Title = "Saturday Night Live",
                    Art = new MediaArtwork { Banner = "http://thetvdb.com/banners/_cache/graphical/76177-g5.jpg" }
                };

                TvShows.Add(new ExtendedVideoDetailsTvShow(videoDetailsTvShow));

                IsLoading = false;
                return;
            }

            try
            {
                var tvshows = await App.Context.Connection.Xbmc.VideoLibrary.GetTvShowsAsync(
                    fields: new[]
                    {
                        VideoFieldsTVShow.art,
                        VideoFieldsTVShow.title,
                        VideoFieldsTVShow.plot,
                        VideoFieldsTVShow.episode,
                        VideoFieldsTVShow.watchedepisodes,
                        VideoFieldsTVShow.playcount
                    });

                if (tvshows.TvShows == null || !tvshows.TvShows.Any())
                {
                    MessageBox.Show(AppResources.Page_Tv_Shows_Message_No_Tv_Show, AppResources.ApplicationTitle, MessageBoxButton.OK);

                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();

                    return;
                }

                var items = tvshows.TvShows.Where(s => _keepWatched || (!_keepWatched && !s.IsWatched))
                                           .OrderBy(s => s.Label)
                                           .Select(s => new ExtendedVideoDetailsTvShow(s));

                foreach (var extendedVideoDetailsTvShow in items)
                    TvShows.Add(extendedVideoDetailsTvShow);
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

        #region NavigateToCommand

        private ICommand _navigateToCommand;

        public ICommand NavigateToCommand
        {
            get { return _navigateToCommand ?? (_navigateToCommand = new Command(NavigateTo)); }
        }

        private void NavigateTo(object o)
        {
            var tvshow = o as ExtendedVideoDetailsTvShow;
            if (tvshow == null) return;

            string url = string.Concat("/TvShows/PageTvShow.xaml?id=", tvshow.Value.TvShowId, "&title=", HttpUtility.UrlEncode(tvshow.Value.Title));

            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }

        #endregion

        #region CastCommand

        private ICommand _castCommand;

        public ICommand CastCommand
        {
            get { return _castCommand ?? (_castCommand = new Command(NavigateToCast)); }
        }

        private void NavigateToCast(object o)
        {
            var tvshow = o as ExtendedVideoDetailsTvShow;
            if (tvshow == null) return;

            string url = string.Concat("/PageCast.xaml?id=", tvshow.Value.TvShowId, "&type=tvshow");
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }

        #endregion

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            LoadTvShowsAsync();
        }

        private void KeepRemove_Button_Click(object sender, RoutedEventArgs e)
        {
            _keepWatched = !_keepWatched;
            LoadTvShowsAsync();

            //ButtonKeepRemoveWatched.Text = _keepWatched ? AppResources.Page_Tv_Shows_Remove_Watched : AppResources.Page_Tv_Shows_Keep_Watched;
        }
    }
}