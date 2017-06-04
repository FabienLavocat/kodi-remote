using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.Movies
{
    public partial class PageRecentlyAdded
    {
        #region Movies

        public ObservableCollection<ExtendedVideoDetailsMovie> Movies
        {
            get { return (ObservableCollection<ExtendedVideoDetailsMovie>)GetValue(MoviesProperty); }
            private set { SetValue(MoviesProperty, value); }
        }

        public static readonly DependencyProperty MoviesProperty =
            DependencyProperty.Register("Movies", typeof(ObservableCollection<ExtendedVideoDetailsMovie>), typeof(PageRecentlyAdded), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageRecentlyAdded), new PropertyMetadata(false));

        #endregion

        public PageRecentlyAdded()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            AddApplicationBar();

            if (!App.Context.DownloadThumbnails)
            {
                LstMovies.ItemTemplate = Application.Current.Resources["MovieItemTemplateFlat"] as DataTemplate;
                LstMovies.Style = Application.Current.Resources["ListLongListSelectorStyle"] as Style;
            }

            IsLoading = true;

            try
            {
                var movies = await App.Context.Connection.Xbmc.VideoLibrary.GetRecentlyAddedMoviesAsync();

                var items = movies.Movies.Select(s => new ExtendedVideoDetailsMovie(s, false));
                Movies = new ObservableCollection<ExtendedVideoDetailsMovie>(items);
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

        private void LstMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var movie = LstMovies.SelectedItem as ExtendedVideoDetailsMovie;
            if (movie == null) return;

            string url = string.Concat("/Movies/PageMovie.xaml?id=", movie.Movie.MovieId, "&title=", HttpUtility.UrlEncode(movie.Movie.Title));

            Helpers.Vibrate();
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }

        private void AddApplicationBar()
        {
            ApplicationBar.Buttons.Clear();
            PageRemote.SetRemoteApplicationBarButton(ApplicationBar, NavigationService);
        }
    }
}