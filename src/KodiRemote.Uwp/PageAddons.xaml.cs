using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KodiRemote.Uwp.Core;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp
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
            AddonSelector.MapDetails = (o) => GetSelectionDetailsAsync(o).GetAwaiter().GetResult();
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
        
        private async Task<object> GetSelectionDetailsAsync(object o)
        {
            var selectedAddon = o as ExtendedAddonDetailsBase;
            if (selectedAddon == null) return null;
            
            IsLoading = true;

            try
            {
                var task = Task.Factory.StartNew(async () => {
                    return await App.Context.Connection.Kodi.Addons.GetAddonDetailsAsync(selectedAddon.Value.AddonId);
                });

                var addon = task.GetAwaiter().GetResult();
                var addonDetails = new ExtendedAddonDetailsBase(addon.Result);
                addonDetails.Value.Description = addonDetails.Value.Description.Replace("[CR]", "\n");

                return addonDetails;
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

            return null;
        }
        
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var addon = AddonSelector.SelectedItem as ExtendedAddonDetailsBase;
            EnableAddon(addon, true);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            var addon = AddonSelector.SelectedItem as ExtendedAddonDetailsBase;
            EnableAddon(addon, false);
        }

        private async void EnableAddon(ExtendedAddonDetailsBase addon, bool enabled)
        {
            if (addon == null || addon.Value.Enabled == enabled) return;

            IsLoading = true;
            await App.Context.Connection.Kodi.Addons.SetAddonEnabledAsync(addon.Value.AddonId, enabled);
            IsLoading = false;

            //Refresh();
        }
    }
}
