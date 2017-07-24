using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using KodiRemote.Core.Commands;
using KodiRemote.Core.Model;
using KodiRemote.Uwp.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp.TvShows
{
    public sealed partial class PageTvShows : Page
    {
        private readonly ResourceLoader _resourceLoader;

        public PageTvShows()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;

            _resourceLoader = ResourceLoader.GetForCurrentView();
        }
        
        #region TvShows

        public ObservableCollection<ExtendedVideoDetailsTvShow> TvShows
        {
            get { return (ObservableCollection<ExtendedVideoDetailsTvShow>)GetValue(TvShowsProperty); }
            private set { SetValue(TvShowsProperty, value); }
        }

        public static readonly DependencyProperty TvShowsProperty =
            DependencyProperty.Register(nameof(TvShows), typeof(ObservableCollection<ExtendedVideoDetailsTvShow>), typeof(PageTvShows), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(PageTvShows), new PropertyMetadata(false));

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

            if (TvShows != null) return;
            
            TvShows = new ObservableCollection<ExtendedVideoDetailsTvShow>();

            await LoadTvShowsAsync(true);
        }

        private async Task LoadTvShowsAsync(bool keepWatched)
        {
            if (IsLoading) return;

            IsLoading = true;
            TvShows.Clear();

            if (App.Context.Connection.Kodi.IsMocked)
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
                var tvshows = await App.Context.Connection.Kodi.VideoLibrary.GetTvShowsAsync(
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
                    return;

                var items = tvshows.TvShows.Where(s => keepWatched || (!keepWatched && !s.IsWatched))
                                           .OrderBy(s => s.Label)
                                           .Select(s => new ExtendedVideoDetailsTvShow(s));

                foreach (var extendedVideoDetailsTvShow in items)
                    TvShows.Add(extendedVideoDetailsTvShow);
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
        }
        
        private void TvShows_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tvshow = e.AddedItems.FirstOrDefault() as ExtendedVideoDetailsTvShow;
            if (tvshow == null) return;

            Frame.Navigate(typeof(PageTvShow), $"{tvshow.Value.TvShowId}|{tvshow.Value.Title}");
        }
    }
}
