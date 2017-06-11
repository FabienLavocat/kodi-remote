using System;
using System.Collections.ObjectModel;
using System.Linq;
using KodiRemote.Uwp.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp.Movies
{
    public sealed partial class PageRecentlyAdded : Page
    {
        private readonly ResourceLoader _resourceLoader;

        public PageRecentlyAdded()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;

            _resourceLoader = ResourceLoader.GetForCurrentView();
        }

        #region Movies

        public ObservableCollection<ExtendedVideoDetailsMovie> Movies
        {
            get { return (ObservableCollection<ExtendedVideoDetailsMovie>)GetValue(MoviesProperty); }
            private set { SetValue(MoviesProperty, value); }
        }

        public static readonly DependencyProperty MoviesProperty =
            DependencyProperty.Register(nameof(Movies), typeof(ObservableCollection<ExtendedVideoDetailsMovie>), typeof(PageRecentlyAdded), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(PageRecentlyAdded), new PropertyMetadata(false));

        #endregion

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = new Windows.UI.Color() { R = 153, G = 122, B = 165 };
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.White;
            }

            if (!App.Context.DownloadThumbnails)
            {
                LstMovies.ItemTemplate = Application.Current.Resources["MovieItemTemplateFlat"] as DataTemplate;
            }

            IsLoading = true;

            try
            {
                var movies = await App.Context.Connection.Kodi.VideoLibrary.GetRecentlyAddedMoviesAsync();

                var items = movies.Movies.Select(s => new ExtendedVideoDetailsMovie(s, false));
                Movies = new ObservableCollection<ExtendedVideoDetailsMovie>(items);
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

        private void LstMovies_ItemClick(object sender, ItemClickEventArgs e)
        {
            var movie = e.ClickedItem as ExtendedVideoDetailsMovie;
            if (movie == null) return;

            Helpers.Vibrate();
            Frame.Navigate(typeof(PageMovie), $"{movie.Movie.MovieId}|{movie.Movie.Title}");
        }
        
        private void Remote_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageRemote));
        }
    }
}
