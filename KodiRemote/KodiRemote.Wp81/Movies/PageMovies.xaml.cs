using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using KodiRemote.Core.Responses;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.Movies
{
    public partial class PageMovies
    {
        #region Movies

        public List<ExtendedVideoDetailsMovie> Movies
        {
            get { return (List<ExtendedVideoDetailsMovie>) GetValue(MoviesProperty); }
            private set { SetValue(MoviesProperty, value); }
        }

        public static readonly DependencyProperty MoviesProperty =
            DependencyProperty.Register("Movies", typeof(List<ExtendedVideoDetailsMovie>), typeof(PageMovies), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool) GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageMovies), new PropertyMetadata(false));

        #endregion

        public PageMovies()
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
                MoviesResponse movies = await App.Context.Connection.Xbmc.VideoLibrary.GetMoviesAsync();
                if (movies.Movies == null || !movies.Movies.Any())
                {
                    MessageBox.Show(AppResources.Page_Movies_Message_No_Movie, AppResources.ApplicationTitle, MessageBoxButton.OK);

                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();

                    return;
                }

                Movies = movies.Movies.Select(s => new ExtendedVideoDetailsMovie(s, false)).OrderBy(m => m.Movie.Title).ToList();
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

        private const string YEAR = "year";
        private const string NAME = "name";

        private void AddApplicationBar()
        {
            ApplicationBar.MenuItems.Clear();
            ApplicationBar.Buttons.Clear();

            var appBarSortYear = new ApplicationBarMenuItem(AppResources.Page_Movies_OrderBy_Year);
            appBarSortYear.Click += (sender, e) => SortMovies(YEAR);
            ApplicationBar.MenuItems.Add(appBarSortYear);

            var appBarSortName = new ApplicationBarMenuItem(AppResources.Page_Movies_OrderBy_Title);
            appBarSortName.Click += (sender, e) => SortMovies(NAME);
            ApplicationBar.MenuItems.Add(appBarSortName);

            var appBarRecent = new ApplicationBarMenuItem(AppResources.Page_Movies_Movies_RecentlyAdded);
            appBarRecent.Click += (sender, e) => NavigationService.Navigate(new Uri("/Movies/PageRecentlyAdded.xaml", UriKind.Relative));
            ApplicationBar.MenuItems.Add(appBarRecent);

            PageRemote.SetRemoteApplicationBarButton(ApplicationBar, NavigationService);
        }

        private void SortMovies(string sortBy)
        {
            Movies = sortBy.Equals(YEAR)
                ? Movies.OrderByDescending(m => m.Movie.Year).ToList()
                : Movies.OrderBy(m => m.Movie.Title).ToList();
        }
    }
}