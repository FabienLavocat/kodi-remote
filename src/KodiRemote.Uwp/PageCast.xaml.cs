using System;
using System.Collections.ObjectModel;
using KodiRemote.Core.Commands;
using KodiRemote.Core.Model;
using KodiRemote.Uwp.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp
{
    public sealed partial class PageCast : Page
    {
        private readonly ResourceLoader _resourceLoader;

        public PageCast()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;

            _resourceLoader = ResourceLoader.GetForCurrentView();
        }
        
        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(PageCast), new PropertyMetadata(false));


        #endregion

        #region Cast

        public ObservableCollection<ExtendedVideoCast> Cast
        {
            get { return (ObservableCollection<ExtendedVideoCast>)GetValue(CastProperty); }
            private set { SetValue(CastProperty, value); }
        }

        public static readonly DependencyProperty CastProperty =
            DependencyProperty.Register(nameof(Cast), typeof(ObservableCollection<ExtendedVideoCast>), typeof(PageCast), new PropertyMetadata(null));

        #endregion
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = new Windows.UI.Color() { R = 115, G = 143, B = 186 };
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.White;
            }

            int paramIndex = e.Parameter.ToString().IndexOf("|");
            string strId = e.Parameter.ToString().Substring(0, paramIndex);
            string type = e.Parameter.ToString().Substring(paramIndex + 1);
            
            Cast = new ObservableCollection<ExtendedVideoCast>();
            IsLoading = true;

            try
            {
                int id = int.Parse(strId);

                VideoCast[] cast;
                if (type.Equals("episode", StringComparison.OrdinalIgnoreCase))
                {
                    var episode = await App.Context.Connection.Kodi.VideoLibrary.GetEpisodeDetailsAsync(id, VideoFieldsEpisode.cast);
                    cast = episode.EpisodeDetails.Cast;
                }
                else if (type.Equals("tvshow", StringComparison.OrdinalIgnoreCase))
                {
                    var tvShow = await App.Context.Connection.Kodi.VideoLibrary.GetTvShowDetailsAsync(id, VideoFieldsTVShow.cast);
                    cast = tvShow.TvShowDetails.Cast;
                }
                else if (type.Equals("movie", StringComparison.OrdinalIgnoreCase))
                {
                    var movie = await App.Context.Connection.Kodi.VideoLibrary.GetMovieDetailsAsync(id, VideoFieldsMovie.cast);
                    cast = movie.Cast;
                }
                else
                {
                    if (Frame.CanGoBack)
                        Frame.GoBack();

                    return;
                }

                foreach (var c in cast)
                    Cast.Add(new ExtendedVideoCast(c));
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
    }
}
