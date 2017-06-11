using System;
using System.Linq;
using System.Threading.Tasks;
using KodiRemote.Core.Commands;
using KodiRemote.Uwp.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp
{
    public sealed partial class PageRemote : Page
    {
        private const int TIMER_INTERVAL = 500;
        private readonly DispatcherTimer _timer;
        private string _cnxId;

        public PageRemote()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(TIMER_INTERVAL);
            _timer.Tick += TimerTick;
        }

        #region CurrentPlayingTitle

        public string CurrentPlayingTitle
        {
            get { return (string)GetValue(CurrentPlayingTitleProperty); }
            private set { SetValue(CurrentPlayingTitleProperty, value); }
        }

        public static readonly DependencyProperty CurrentPlayingTitleProperty =
            DependencyProperty.Register(nameof(CurrentPlayingTitle), typeof(string), typeof(PageRemote), new PropertyMetadata(null));

        #endregion

        #region CurrentPosition

        public int CurrentPosition
        {
            get { return (int)GetValue(CurrentPositionProperty); }
            set { SetValue(CurrentPositionProperty, value); }
        }

        public static readonly DependencyProperty CurrentPositionProperty =
            DependencyProperty.Register(nameof(CurrentPosition), typeof(int), typeof(PageRemote), new PropertyMetadata(0));

        #endregion

        #region Lenght

        public int Lenght
        {
            get { return (int)GetValue(LenghtProperty); }
            private set { SetValue(LenghtProperty, value); }
        }

        public static readonly DependencyProperty LenghtProperty =
            DependencyProperty.Register(nameof(Lenght), typeof(int), typeof(PageRemote), new PropertyMetadata(0));

        #endregion

        #region ImageUrl

        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            private set { SetValue(ImageUrlProperty, value); }
        }

        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register(nameof(ImageUrl), typeof(string), typeof(PageRemote), new PropertyMetadata(null));

        #endregion

        #region ActionTitle

        public string ActionTitle
        {
            get { return (string)GetValue(ActionTitleProperty); }
            set { SetValue(ActionTitleProperty, value); }
        }

        public static readonly DependencyProperty ActionTitleProperty =
            DependencyProperty.Register(nameof(ActionTitle), typeof(string), typeof(PageRemote), new PropertyMetadata(null));

        #endregion

        #region ActionSubTitle

        public string ActionSubTitle
        {
            get { return (string)GetValue(ActionSubTitleProperty); }
            set { SetValue(ActionSubTitleProperty, value); }
        }

        public static readonly DependencyProperty ActionSubTitleProperty =
            DependencyProperty.Register(nameof(ActionSubTitle), typeof(string), typeof(PageRemote), new PropertyMetadata(null));

        #endregion

        #region Volume

        private int _volume;

        private int Volume
        {
            get { return _volume; }
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;

                ActionTitle = "VOLUME";
                ActionSubTitle = string.Concat(value, "%");

                _volume = value;
            }
        }

        #endregion

        #region OnNavigatedTo / OnNavigatedFrom

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.Context.Connection.Kodi == null)
            {
                if (Frame.CanGoBack)
                    Frame.GoBack();

                return;
            }

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusbar.HideAsync();
            }

            if (e.Uri != null &&  e.Uri.Query.Contains("tile"))
            {
                BtPinToStartV.Visibility = BtPinToStartH.Visibility = Visibility.Collapsed;
                BtBackV.Visibility = BtBackH.Visibility = Visibility.Visible;

                int index = e.Uri.Query.IndexOf("cnx=");
                if (index >= 0)
                    _cnxId = e.Uri.Query.Substring(index + 4);
            }
            else
            {
                BtPinToStartV.Visibility = BtPinToStartH.Visibility = Visibility.Visible;
                BtBackV.Visibility = BtBackH.Visibility = Visibility.Collapsed;
            }

            ImagePlayingNowH.ImageOpened += (sender, ea) => ImageStoryboardH.Begin();
            ImagePlayingNowV.ImageOpened += (sender, ea) => ImageStoryboardV.Begin();

            //SetOrientation(Orientation);

            TimerTick(this, EventArgs.Empty);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _timer.Stop();
        }

        #endregion

        //private void SetOrientation(PageOrientation orientation)
        //{
        //    if ((orientation & PageOrientation.Portrait) == (PageOrientation.Portrait))
        //    {
        //        RemoteV.Visibility = Visibility.Visible;
        //        RemoteH.Visibility = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        RemoteV.Visibility = Visibility.Collapsed;
        //        RemoteH.Visibility = Visibility.Visible;
        //    }
        //}

        #region Timer

        private async void TimerTick(object sender, object e)
        {
            _timer.Stop();

            await GetVolumeAsync();
            KodiRemote.Core.Model.Player player = await LoadPlayerAsync();
            SliderGridH.Visibility = SliderGridV.Visibility = player == null ? Visibility.Collapsed : Visibility.Visible;
            ImageKodiV.Visibility = ImageKodiH.Visibility = player != null ? Visibility.Collapsed : Visibility.Visible;
            if (player != null)
            {
                await LoadCurrentPlayAsync(player);
            }
            else
            {
                ImageUrl = null;
                CurrentPlayingTitle = string.Empty;
            }

            if (!App.Context.Connection.Kodi.IsMocked)
                _timer.Start();
        }

        private async Task GetVolumeAsync()
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            var properties = await App.Context.Connection.Kodi.Application.GetPropertiesAsync(ApplicationPropertyName.volume);
            Volume = properties.Volume;
        }

        private async Task<KodiRemote.Core.Model.Player> LoadPlayerAsync()
        {
            try
            {
                var players = await App.Context.Connection.Kodi.Player.GetActivePlayersAsync();
                if (players.Any())
                    return players[0];
            }
            catch { }

            return null;
        }

        private async Task LoadCurrentPlayAsync(KodiRemote.Core.Model.Player player)
        {
            if (player == null) return;

            try
            {
                var item = await App.Context.Connection.Kodi.Player.GetItemAsync(player.PlayerId,
                                                                                 ListFieldsAll.album,
                                                                                 ListFieldsAll.title,
                                                                                 ListFieldsAll.artist,
                                                                                 ListFieldsAll.thumbnail);
                var properties = await App.Context.Connection.Kodi.Player.GetPropertiesAsync(player.PlayerId,
                                                                                             PlayerPropertyName.time,
                                                                                             PlayerPropertyName.totaltime);

                if (!_sliderManipulationStarted)
                    CurrentPosition = properties.Time.TotalMilliseconds();
                Lenght = properties.TotalTime.TotalMilliseconds();

                if (item.Thumbnail.Contains("DefaultAlbumCover"))
                    ImageUrl = "/Assets/Default/DefaultAlbumCover.png";
                else if (item.Thumbnail.Contains("DefaultVideo"))
                    ImageUrl = "/Assets/Default/DefaultVideo.png";
                else if (string.IsNullOrWhiteSpace(item.Thumbnail) && item.Type == "song")
                    ImageUrl = "/Assets/Default/DefaultAlbumCover.png";
                else if (string.IsNullOrWhiteSpace(item.Thumbnail))
                    ImageUrl = "";
                else
                {
                    var download = await App.Context.Connection.Kodi.Files.PrepareDownloadAsync(item.Thumbnail);
                    string url = App.Context.Connection.Kodi.GetFileUrl(download.Details.Path);
                    if (url != null && ImageUrl != url)
                        ImageUrl = url;
                }

                CurrentPlayingTitle = string.IsNullOrWhiteSpace(item.Title) ? item.Label : item.Title;
                var artist = item.Artist.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(item.Album) && artist != null)
                    CurrentPlayingTitle += string.Concat("\r", artist, " - ", item.Album);
            }
            catch { }
        }

        #endregion

        #region Slider

        private bool _sliderManipulationStarted;

        private void SliderManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _sliderManipulationStarted = true;
        }
        
        private async void SliderManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            _sliderManipulationStarted = false;

            KodiRemote.Core.Model.Player player = await LoadPlayerAsync();
            if (player == null)
                return;

            // Disable temporarily the slider
            Slider slider = e.OriginalSource as Slider;
            if (slider != null)
                slider.IsEnabled = false;

            try
            {
                TimeSpan time = TimeSpan.FromMilliseconds(CurrentPosition);
                await App.Context.Connection.Kodi.Player.SeekAsync(player.PlayerId, time.Milliseconds, time.Seconds, time.Minutes, time.Hours);
            }
            catch { }

            // Re-enable the slider
            if (slider != null)
                slider.IsEnabled = true;
        }

        #endregion

        #region First Row

        private void BtTextClick(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Frame.Navigate(typeof(PageSendText));
        }

        private async void BtVolumeMute(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            await App.Context.Connection.Kodi.Application.SetMuteAsync(true);
        }

        private async void BtVolumeDown(object sender, RoutedEventArgs e)
        {
            if (!App.Context.Connection.Kodi.IsMocked)
            {
                Helpers.Vibrate();
                await App.Context.Connection.Kodi.Application.DecrementVolumeAsync();
            }

            Volume--;
        }

        private async void BtVolumeUp(object sender, RoutedEventArgs e)
        {
            if (!App.Context.Connection.Kodi.IsMocked)
            {
                Helpers.Vibrate();
                await App.Context.Connection.Kodi.Application.IncrementVolumeAsync();
            }

            Volume++;
        }

        private void BtPinToStartClick(object sender, RoutedEventArgs e)
        {
            // See if the tile is pinned, and if so, make sure the checkbox for it is checked.
            // (User may have deleted it manually.)
            //ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("PageRemote.xaml"));
            //if (tile != null) return;

            //var remoteTile = new StandardTileData
            //{
            //    BackgroundImage = new Uri("Assets/Tiles/TileRemote.png", UriKind.Relative),
            //    Title = AppResources.Global_Remote
            //};

            //// Create the tile and pin it to Start. This will cause a navigation to Start and a deactivation of our application.
            //ShellTile.Create(new Uri("/PageRemote.xaml?from=tile&cnx=" + App.Context.Connection.Id, UriKind.Relative), remoteTile);
        }

        private void BtBackClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), _cnxId);
        }

        #endregion

        #region Arrows

        private async void BtUp(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            await App.Context.Connection.Kodi.Input.UpAsync();
        }

        private async void BtDown(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            await App.Context.Connection.Kodi.Input.DownAsync();
        }

        private async void BtLeft(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            await App.Context.Connection.Kodi.Input.LeftAsync();
        }

        private async void BtRight(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            await App.Context.Connection.Kodi.Input.RightAsync();
        }

        private async void BtSelect(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            await App.Context.Connection.Kodi.Input.SelectAsync();
        }

        private async void BtBack(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            await App.Context.Connection.Kodi.Input.BackAsync();
        }

        private async void BtHome(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            await App.Context.Connection.Kodi.Input.HomeAsync();
        }

        private async void BtInfo(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            await App.Context.Connection.Kodi.Input.InfoAsync();
        }

        private async void BtContextMenu(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            await App.Context.Connection.Kodi.Input.ContextMenuAsync();
        }

        #endregion

        #region Bottom Row

        private async void BtVideoOsdClick(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            try
            {
                await App.Context.Connection.Kodi.Gui.ActivateWindowAsync(GuiWindow.videoosd);
            }
            catch { }
        }

        private async void BtDisplay(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            try
            {
                await App.Context.Connection.Kodi.Input.ExecuteActionAsync(InputActions.togglefullscreen);
            }
            catch { }
        }

        private async void BtStop(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            try
            {
                await App.Context.Connection.Kodi.Input.ExecuteActionAsync(InputActions.stop);
            }
            catch { }
        }

        private async void BtSub(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            try
            {
                await App.Context.Connection.Kodi.Input.ExecuteActionAsync(InputActions.showsubtitles);
            }
            catch { }
        }

        private async void BtAudio(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            try
            {
                await App.Context.Connection.Kodi.Input.ExecuteActionAsync(InputActions.audionextlanguage);
            }
            catch { }
        }

        // Second Row

        private async void BtGoToPrevious(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            var players = await App.Context.Connection.Kodi.Player.GetActivePlayersAsync();
            if (players.Any())
            {
                Helpers.Vibrate();
                try
                {
                    await App.Context.Connection.Kodi.Player.GoToAsync(players[0].PlayerId, GoTos.Previous);
                }
                catch { }
            }
        }

        private async void BtSeekBackward(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            try
            {
                await App.Context.Connection.Kodi.Input.ExecuteActionAsync(InputActions.stepback);
            }
            catch { }
        }

        private async void BtPlayPause(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            try
            {
                await App.Context.Connection.Kodi.Input.ExecuteActionAsync(InputActions.playpause);
            }
            catch { }
        }

        private async void BtSeekForward(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            Helpers.Vibrate();
            try
            {
                await App.Context.Connection.Kodi.Input.ExecuteActionAsync(InputActions.stepforward);
            }
            catch { }
        }

        private async void BtGoToNext(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked) return;

            var players = await App.Context.Connection.Kodi.Player.GetActivePlayersAsync();
            if (players.Any())
            {
                Helpers.Vibrate();
                try
                {
                    await App.Context.Connection.Kodi.Player.GoToAsync(players[0].PlayerId, GoTos.Next);
                }
                catch { }
            }
        }
        
        #endregion

        //public static void SetRemoteApplicationBarButton(IApplicationBar appBar, NavigationService navigationService)
        //{
        //    var appBarBt =
        //        new ApplicationBarIconButton(new Uri("/Assets/appbar.tv.remote.png", UriKind.Relative))
        //        {
        //            Text = AppResources.Global_Remote
        //        };
        //    appBarBt.Click += (sender, e) => navigationService.Navigate(new Uri("/PageRemote.xaml", UriKind.Relative));
        //    appBar.Buttons.Add(appBarBt);
        //}
    }
}
