using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using KodiRemote.Uwp.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp.Addons
{
    public sealed partial class PageAddons : Page
    {
        private readonly ResourceLoader _resourceLoader;

        public PageAddons()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;

            _resourceLoader = ResourceLoader.GetForCurrentView();
        }

        #region Addons

        public ObservableCollection<ExtendedAddonDetailsBase> Addons
        {
            get { return (ObservableCollection<ExtendedAddonDetailsBase>)GetValue(AddonsProperty); }
            private set { SetValue(AddonsProperty, value); }
        }

        public static readonly DependencyProperty AddonsProperty =
            DependencyProperty.Register(nameof(Addons), typeof(ObservableCollection<ExtendedAddonDetailsBase>), typeof(PageAddons), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(PageAddons), new PropertyMetadata(null));

        #endregion

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            IsLoading = true;

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = new Windows.UI.Color() { R = 201, G = 196, B = 42 };
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.White;
            }

            try
            {
                var addons = await App.Context.Connection.Kodi.Addons.GetAddonsAsync();
                var items = addons.Addons.OrderBy(s => s.Name).Select(a => new ExtendedAddonDetailsBase(a));
                Addons = new ObservableCollection<ExtendedAddonDetailsBase>(items);
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

        #region NavigateToCommand

        private ICommand _navigateToCommand;

        public ICommand NavigateToCommand
        {
            get { return _navigateToCommand ?? (_navigateToCommand = new Command(NavigateTo)); }
        }

        private void NavigateTo(object o)
        {
            var addon = o as ExtendedAddonDetailsBase;
            if (addon == null) return;

            Frame.Navigate(typeof(PageAddon), addon.Value.AddonId + "|" + addon.Value.Name);
        }

        #endregion
    }
}
