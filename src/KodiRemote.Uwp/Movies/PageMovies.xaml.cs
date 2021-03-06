﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using KodiRemote.Core.Model;
using KodiRemote.Core.Responses;
using KodiRemote.Uwp.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp.Movies
{
    public sealed partial class PageMovies : Page
    {
        private readonly ResourceLoader _resourceLoader;

        public PageMovies()
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
            DependencyProperty.Register(nameof(Movies), typeof(ObservableCollection<ExtendedVideoDetailsMovie>), typeof(PageMovies), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(PageMovies), new PropertyMetadata(false));

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

            Movies = new ObservableCollection<ExtendedVideoDetailsMovie>();
            await LoadMoviesAsync(false);
        }

        private async Task LoadMoviesAsync(bool recentlyAdded)
        {
            IsLoading = true;
            Movies.Clear();

            try
            {
                MoviesResponse movies;
                if (recentlyAdded)
                    movies = await App.Context.Connection.Kodi.VideoLibrary.GetRecentlyAddedMoviesAsync();
                else
                    movies = await App.Context.Connection.Kodi.VideoLibrary.GetMoviesAsync();

                if (movies.Movies == null || !movies.Movies.Any())
                {
                    var dialog = new MessageDialog(_resourceLoader.GetString("/movies/NoMovies"), _resourceLoader.GetString("ApplicationTitle"));
                    await dialog.ShowAsync();

                    if (Frame.CanGoBack)
                        Frame.GoBack();

                    return;
                }
                
                foreach (var movie in movies.Movies.OrderBy(m => m.SortTitle))
                {
                    Movies.Add(new ExtendedVideoDetailsMovie(movie, false));
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
        }

        private void Movies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var movie = e.AddedItems.FirstOrDefault() as ExtendedVideoDetailsMovie;
            if (movie == null) return;

            Frame.Navigate(typeof(PageMovie), $"{movie.Movie.MovieId}|{movie.Movie.Title}");
        }

        private void LstMovies_ItemClick(object sender, ItemClickEventArgs e)
        {
            var movie = e.ClickedItem as ExtendedVideoDetailsMovie;
            if (movie == null) return;

            Helpers.Vibrate();
            Frame.Navigate(typeof(PageMovie), $"{movie.Movie.MovieId}|{movie.Movie.Title}");
        }

        private void SortByYear_Click(object sender, RoutedEventArgs e)
        {
            var movies = Movies.OrderByDescending(m => m.Movie.Year).ToList();
            Movies.Clear();
            foreach (var movie in movies)
            {
                Movies.Add(movie);
            }
        }

        private void SortByTitle_Click(object sender, RoutedEventArgs e)
        {
            var movies = Movies.OrderBy(m => m.Movie.Title).ToList();
            Movies.Clear();
            foreach (var movie in movies)
            {
                Movies.Add(movie);
            }
        }

        private async void RecentlyAdded_Click(object sender, RoutedEventArgs e)
        {
            RecentlyAddedCommandButton.Visibility = Visibility.Collapsed;
            AllMoviesCommandButton.Visibility = Visibility.Visible;

            await LoadMoviesAsync(true);
        }

        private async void AllMovies_Click(object sender, RoutedEventArgs e)
        {
            RecentlyAddedCommandButton.Visibility = Visibility.Visible;
            AllMoviesCommandButton.Visibility = Visibility.Collapsed;

            await LoadMoviesAsync(false);
        }
    }
}
