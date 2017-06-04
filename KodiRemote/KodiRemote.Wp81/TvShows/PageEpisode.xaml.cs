using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using KodiRemote.Core.Model;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Core.Downloads;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.TvShows
{
    public partial class PageEpisode
    {
        #region ImageHeader

        public string ImageHeader
        {
            get { return (string) GetValue(ImageHeaderProperty); }
            private set { SetValue(ImageHeaderProperty, value); }
        }

        public static readonly DependencyProperty ImageHeaderProperty =
            DependencyProperty.Register("ImageHeader", typeof(string), typeof(PageEpisode), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool) GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageEpisode), new PropertyMetadata(false));

        #endregion

        #region Cast

        public ObservableCollection<ExtendedVideoCast> Cast
        {
            get { return (ObservableCollection<ExtendedVideoCast>) GetValue(CastProperty); }
            private set { SetValue(CastProperty, value); }
        }

        public static readonly DependencyProperty CastProperty =
            DependencyProperty.Register("Cast", typeof(ObservableCollection<ExtendedVideoCast>), typeof(PageEpisode), new PropertyMetadata(null));

        #endregion

        #region Episode

        public VideoDetailsEpisode Episode
        {
            get { return (VideoDetailsEpisode) GetValue(EpisodeProperty); }
            private set { SetValue(EpisodeProperty, value); }
        }

        public static readonly DependencyProperty EpisodeProperty =
            DependencyProperty.Register("Episode", typeof(VideoDetailsEpisode), typeof(PageEpisode), new PropertyMetadata(null));

        #endregion

        #region Writer

        public string Writer
        {
            get { return (string) GetValue(WriterProperty); }
            private set { SetValue(WriterProperty, value); }
        }

        public static readonly DependencyProperty WriterProperty =
            DependencyProperty.Register("Writer", typeof(string), typeof(PageEpisode), new PropertyMetadata(null));

        #endregion

        #region Rating

        public double Rating
        {
            get { return (double) GetValue(RatingProperty); }
            private set { SetValue(RatingProperty, value); }
        }

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register("Rating", typeof(double), typeof(PageEpisode), new PropertyMetadata(null));

        #endregion

        #region Votes

        public string Votes
        {
            get { return (string) GetValue(VotesProperty); }
            private set { SetValue(VotesProperty, value); }
        }

        public static readonly DependencyProperty VotesProperty =
            DependencyProperty.Register("Votes", typeof(string), typeof(PageEpisode), new PropertyMetadata(null));

        #endregion

        #region Minutes

        public int Minutes
        {
            get { return (int) GetValue(MinutesProperty); }
            private set { SetValue(MinutesProperty, value); }
        }

        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes", typeof(int), typeof(PageEpisode), new PropertyMetadata(null));

        #endregion

        public PageEpisode()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string id;
            int episodeId;
            if (!NavigationContext.QueryString.TryGetValue("id", out id)
                || !int.TryParse(id, out episodeId))
            {
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();

                return;
            }

            Cast = new ObservableCollection<ExtendedVideoCast>();
            IsLoading = true;

            try
            {
                var episode = await App.Context.Connection.Xbmc.VideoLibrary.GetEpisodeDetailsAsync(episodeId);
                Episode = episode.EpisodeDetails;

                Writer = Helpers.Combine(Episode.Writer);
                Rating = Episode.Rating / 2;
                Votes = string.Format(AppResources.Page_Tv_Shows_Votes_Format, Episode.Votes);
                Minutes = Episode.Runtime / 60;

                foreach (var cast in Episode.Cast.Take(5))
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

            if (Episode != null && !string.IsNullOrWhiteSpace(Episode.Thumbnail))
                ImageHeader = await Helpers.LoadImageUrl(Episode.Thumbnail);
        }

        private void Cast_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Episode == null) return;

            string url = string.Concat("/PageCast.xaml?id=", Episode.EpisodeId, "&type=episode");

            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }

        private async void Play_Button_Click(object sender, EventArgs e)
        {
            if (App.Context.Connection.Xbmc.IsMocked) return;

            IsLoading = true;
            try
            {
                await App.Context.Connection.Xbmc.Player.OpenAsync(episodeId: Episode.EpisodeId);
                NavigationService.Navigate(new Uri("/PageRemote.xaml", UriKind.Relative));
                return;
            }
            catch { }
                        
            IsLoading = false;
        }

        private void Remote_Button_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageRemote.xaml", UriKind.Relative));
        }

        private async void Download_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Episode == null) return;

            await BackgroundTransfer.InitiateBackgroundTransferAsync(Episode.File);
        }
    }
}