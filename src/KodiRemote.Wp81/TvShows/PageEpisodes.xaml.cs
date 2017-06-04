using System;
using System.Collections.ObjectModel;
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
    public partial class PageEpisodes
    {
        #region PageTitle

        public string PageTitle
        {
            get { return (string)GetValue(PageTitleProperty); }
            private set { SetValue(PageTitleProperty, value); }
        }

        public static readonly DependencyProperty PageTitleProperty =
            DependencyProperty.Register("PageTitle", typeof(string), typeof(PageEpisodes), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageEpisodes), new PropertyMetadata(false));

        #endregion

        #region Episodes

        public ObservableCollection<VideoDetailsEpisode> Episodes
        {
            get { return (ObservableCollection<VideoDetailsEpisode>) GetValue(EpisodesProperty); }
            private set { SetValue(EpisodesProperty, value); }
        }

        public static readonly DependencyProperty EpisodesProperty =
            DependencyProperty.Register("Episodes", typeof(ObservableCollection<VideoDetailsEpisode>), typeof(PageEpisodes), new PropertyMetadata(null));

        #endregion

        public PageEpisodes()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string setTitle;
            string tvid, sId;
            int tvShowId, seasonId;
            if (!NavigationContext.QueryString.TryGetValue("tvid", out tvid)
                || !NavigationContext.QueryString.TryGetValue("season", out sId)
                || !NavigationContext.QueryString.TryGetValue("title", out setTitle)
                || !int.TryParse(tvid, out tvShowId)
                || !int.TryParse(sId, out seasonId))
            {
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();

                return;
            }

            Episodes = new ObservableCollection<VideoDetailsEpisode>();
            PageTitle = HttpUtility.UrlDecode(setTitle);
            IsLoading = true;

            try
            {
                var episodes = await App.Context.Connection.Xbmc.VideoLibrary.GetEpisodesAsync(tvShowId, seasonId,
                    fields: new []{ VideoFieldsEpisode.episode, VideoFieldsEpisode.title, VideoFieldsEpisode.playcount });
                foreach (var episode in episodes.Episodes)
                    Episodes.Add(episode);
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

        private ICommand _episodeCommand;

        public ICommand EpisodeCommand
        {
            get { return _episodeCommand ?? (_episodeCommand = new Command(NavigateToEpisode)); }
        }

        private void NavigateToEpisode(object o)
        {
            var episode = o as VideoDetailsEpisode;
            if (episode == null) return;

            string url = string.Concat("/TvShows/PageEpisode.xaml?id=", episode.EpisodeId);

            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }

        #endregion
    }
}