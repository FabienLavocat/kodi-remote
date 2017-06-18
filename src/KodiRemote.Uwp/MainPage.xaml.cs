using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using KodiRemote.Uwp.AppSettings;
using KodiRemote.Uwp.Controls;
using KodiRemote.Uwp.Core;
using KodiRemote.Uwp.Movies;
using KodiRemote.Uwp.Musics;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp
{
    public sealed partial class MainPage : Page
    {
        private bool isPaddingAdded = false;

        private List<NavMenuItem> navlist = new List<NavMenuItem>(
            new[]
            {
                new NavMenuItem()
                {
                    Symbol = Symbol.Contact,
                    Label = "TV Shows",
                    DestPage = typeof(PageAbout)
                },
                new NavMenuItem()
                {
                    Symbol = Symbol.Video,
                    Label = "Movies",
                    DestPage = typeof(PageMovies)
                },
                new NavMenuItem()
                {
                    Symbol = Symbol.MusicInfo,
                    Label = "Musics",
                    DestPage = typeof(PageMusics)
                },
                new NavMenuItem()
                {
                    Symbol = Symbol.CellPhone,
                    Label = "Remote Control",
                    DestPage = typeof(PageRemote)
                },
                new NavMenuItem()
                {
                    Symbol = Symbol.Bullets,
                    Label = "Playlists",
                    DestPage = typeof(PageAbout)
                },
                new NavMenuItem()
                {
                    Symbol = Symbol.More,
                    Label = "Addons",
                    DestPage = typeof(PageAddons)
                },
                new NavMenuItem()
                {
                    Symbol = Symbol.Setting,
                    Label = "Settings",
                    DestPage = typeof(PageAppSettings)
                },
                new NavMenuItem()
                {
                    Symbol = Symbol.Help,
                    Label = "About",
                    DestPage = typeof(PageAbout)
                },
            });

        public static MainPage Current = null;

        private KodiConnection _connection = null;

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;

            RootSplitView.RegisterPropertyChangedCallback(SplitView.DisplayModeProperty, (s, a) => { CheckTogglePaneButtonSizeChanged(); });

            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            NavMenuList.ItemsSource = navlist;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Current = this;

            CheckTogglePaneButtonSizeChanged();

            var titleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            titleBar.IsVisibleChanged += TitleBar_IsVisibleChanged;
        }

        public Frame AppFrame { get { return frame; } }

        /// <summary>
        /// Invoked when window title bar visibility changes, such as after loading or in tablet mode
        /// Ensures correct padding at window top, between title bar and app content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TitleBar_IsVisibleChanged(Windows.ApplicationModel.Core.CoreApplicationViewTitleBar sender, object args)
        {
            if (isPaddingAdded || !sender.IsVisible) return;

            // Add extra padding between window title bar and app content
            double extraPadding = (Double)Application.Current.Resources["DesktopWindowTopPadding"];
            isPaddingAdded = true;

            Thickness margin = NavMenuList.Margin;
            NavMenuList.Margin = new Thickness(margin.Left, margin.Top + extraPadding, margin.Right, margin.Bottom);
            margin = AppFrame.Margin;
            AppFrame.Margin = new Thickness(margin.Left, margin.Top + extraPadding, margin.Right, margin.Bottom);
            margin = TogglePaneButton.Margin;
            TogglePaneButton.Margin = new Thickness(margin.Left, margin.Top + extraPadding, margin.Right, margin.Bottom);

        }
        
        #region BackRequested Handlers

        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            bool handled = e.Handled;
            BackRequested(ref handled);
            e.Handled = handled;
        }

        private void BackRequested(ref bool handled)
        {
            // Get a hold of the current frame so that we can inspect the app back stack.
            if (AppFrame == null)
                return;

            // Check to see if this is the top-most page on the app back stack.
            if (AppFrame.CanGoBack && !handled)
            {
                // If not, set the event to handled and go back to the previous page in the app.
                handled = true;
                AppFrame.GoBack();
            }
        }

        #endregion

        #region Navigation

        /// <summary>
        /// Navigate to the Page for the selected <paramref name="listViewItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listViewItem"></param>
        private void NavMenuList_ItemInvoked(object sender, ListViewItem listViewItem)
        {
            foreach (var i in navlist)
            {
                i.IsSelected = false;
            }

            var item = (NavMenuItem)((NavMenuListView)sender).ItemFromContainer(listViewItem);

            if (item != null)
            {
                item.IsSelected = true;
                if (item.DestPage != null &&
                    item.DestPage != AppFrame.CurrentSourcePageType)
                {
                    AppFrame.Navigate(item.DestPage, item.Arguments);
                }
            }
        }

        /// <summary>
        /// Ensures the nav menu reflects reality when navigation is triggered outside of
        /// the nav menu buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                var item = (from p in navlist where p.DestPage == e.SourcePageType select p).SingleOrDefault();
                if (item == null && AppFrame.BackStackDepth > 0)
                {
                    // In cases where a page drills into sub-pages then we'll highlight the most recent
                    // navigation menu item that appears in the BackStack
                    foreach (var entry in AppFrame.BackStack.Reverse())
                    {
                        item = (from p in navlist where p.DestPage == entry.SourcePageType select p).SingleOrDefault();
                        if (item != null)
                            break;
                    }
                }

                foreach (var i in navlist)
                {
                    i.IsSelected = false;
                }
                if (item != null)
                {
                    item.IsSelected = true;
                }

                var container = (ListViewItem)NavMenuList.ContainerFromItem(item);

                // While updating the selection state of the item prevent it from taking keyboard focus.  If a
                // user is invoking the back button via the keyboard causing the selected nav menu item to change
                // then focus will remain on the back button.
                if (container != null) container.IsTabStop = false;
                NavMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = new Windows.UI.Color() { R = 76, G = 155, B = 214 };
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.White;
            }

            _connection = App.Context.Connections.FirstOrDefault(c => c.Id.Equals(e.Parameter?.ToString(), StringComparison.OrdinalIgnoreCase));

            NavMenuList.SelectedIndex = 0;
            navlist[0].IsSelected = true;
            AppFrame.Navigate(navlist[0].DestPage, navlist[0].Arguments);

            OpenNavePane();

            if (_connection != null)
            {
                //if (_connection.Kodi.IsMocked)
                //{
                //    ButtonMovies.Visibility = Visibility.Collapsed;
                //    ButtonMusic.Visibility = Visibility.Collapsed;
                //    ButtonAddons.Visibility = Visibility.Collapsed;
                //    ButtonPlaylists.Visibility = Visibility.Collapsed;
                //}
                //else  
                //{
                //    ButtonMovies.Visibility = Visibility.Visible;
                //    ButtonMusic.Visibility = Visibility.Visible;
                //    ButtonAddons.Visibility = Visibility.Visible;
                //    ButtonPlaylists.Visibility = Visibility.Visible;
                //}

                //BtActions.DataContext = _connection;
                App.Context.SetDefaultConnection(_connection);
                App.Context.Save();

                await _connection.TestConnectionAsync();
                return;
            }

            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        #endregion

        public Rect TogglePaneButtonRect { get; private set; }

        /// <summary>
        /// An event to notify listeners when the hamburger button may occlude other content in the app.
        /// The custom "PageHeader" user control is using this.
        /// </summary>
        public event TypedEventHandler<MainPage, Rect> TogglePaneButtonRectChanged;

        /// <summary>
        /// Public method to allow pages to open SplitView's pane.
        /// Used for custom app shortcuts like navigating left from page's left-most item
        /// </summary>
        public void OpenNavePane()
        {
            TogglePaneButton.IsChecked = true;
        }

        /// <summary>
        /// Callback when the SplitView's Pane is toggled closed.  When the Pane is not visible
        /// then the floating hamburger may be occluding other content in the app unless it is aware.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TogglePaneButton_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckTogglePaneButtonSizeChanged();
        }

        /// <summary>
        /// Callback when the SplitView's Pane is toggled opened.
        /// Restores divider's visibility and ensures that margins around the floating hamburger are correctly set.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TogglePaneButton_Checked(object sender, RoutedEventArgs e)
        {
            CheckTogglePaneButtonSizeChanged();
        }

        /// <summary>
        /// Check for the conditions where the navigation pane does not occupy the space under the floating
        /// hamburger button and trigger the event.
        /// </summary>
        private void CheckTogglePaneButtonSizeChanged()
        {
            if (RootSplitView.DisplayMode == SplitViewDisplayMode.Inline ||
                RootSplitView.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                var transform = TogglePaneButton.TransformToVisual(this);
                var rect = transform.TransformBounds(new Rect(0, 0, TogglePaneButton.ActualWidth, TogglePaneButton.ActualHeight));
                TogglePaneButtonRect = rect;
            }
            else
            {
                TogglePaneButtonRect = new Rect();
            }

            var handler = TogglePaneButtonRectChanged;
            if (handler != null)
            {
                // handler(this, this.TogglePaneButtonRect);
                handler.DynamicInvoke(this, TogglePaneButtonRect);
            }
        }

        /// <summary>
        /// Enable accessibility on each nav menu item by setting the AutomationProperties.Name on each container
        /// using the associated Label of each item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavMenuItemContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue && args.Item != null && args.Item is NavMenuItem)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, ((NavMenuItem)args.Item).Label);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }
        }
    }
}
