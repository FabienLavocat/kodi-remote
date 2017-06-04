using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using KodiRemote.Core.Commands;
using KodiRemote.Core.Model;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81
{
    public partial class PageCast : PhoneApplicationPage
    {
        #region IsLoading

        public bool IsLoading
        {
            get { return (bool) GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageCast), new PropertyMetadata(false));


        #endregion

        #region Cast

        public ObservableCollection<ExtendedVideoCast> Cast
        {
            get { return (ObservableCollection<ExtendedVideoCast>) GetValue(CastProperty); }
            private set { SetValue(CastProperty, value); }
        }

        public static readonly DependencyProperty CastProperty =
            DependencyProperty.Register("Cast", typeof(ObservableCollection<ExtendedVideoCast>), typeof(PageCast), new PropertyMetadata(null));

        #endregion

        public PageCast()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string strId, type;
            int id;
            if (!NavigationContext.QueryString.TryGetValue("id", out strId)
                || !NavigationContext.QueryString.TryGetValue("type", out type)
                || !int.TryParse(strId, out id))
            {
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();

                return;
            }

            Cast = new ObservableCollection<ExtendedVideoCast>();
            IsLoading = true;

            try
            {
                VideoCast[] cast;
                if (type.Equals("episode", StringComparison.InvariantCultureIgnoreCase))
                {
                    var episode = await App.Context.Connection.Xbmc.VideoLibrary.GetEpisodeDetailsAsync(id, VideoFieldsEpisode.cast);
                    cast = episode.EpisodeDetails.Cast;
                }
                else if (type.Equals("tvshow", StringComparison.InvariantCultureIgnoreCase))
                {
                    var tvShow = await App.Context.Connection.Xbmc.VideoLibrary.GetTvShowDetailsAsync(id, VideoFieldsTVShow.cast);
                    cast = tvShow.TvShowDetails.Cast;
                }
                else if (type.Equals("movie", StringComparison.InvariantCultureIgnoreCase))
                {
                    var episode = await App.Context.Connection.Xbmc.VideoLibrary.GetEpisodeDetailsAsync(id, VideoFieldsEpisode.cast);
                    cast = episode.EpisodeDetails.Cast;
                }
                else
                {
                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();

                    return;
                }

                foreach (var c in cast)
                    Cast.Add(new ExtendedVideoCast(c));
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
    }
}