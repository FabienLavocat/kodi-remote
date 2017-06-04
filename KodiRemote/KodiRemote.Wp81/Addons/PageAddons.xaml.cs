using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.Addons
{
    public partial class PageAddons
    {
        #region Addons

        public ObservableCollection<ExtendedAddonDetailsBase> Addons
        {
            get { return (ObservableCollection<ExtendedAddonDetailsBase>)GetValue(AddonsProperty); }
            private set { SetValue(AddonsProperty, value); }
        }

        public static readonly DependencyProperty AddonsProperty =
            DependencyProperty.Register("Addons", typeof(ObservableCollection<ExtendedAddonDetailsBase>), typeof(PageAddons), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageAddons), new PropertyMetadata(null));

        #endregion

        public PageAddons()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            IsLoading = true;

            try
            {
                var addons = await App.Context.Connection.Xbmc.Addons.GetAddonsAsync();
                var items = addons.Addons.OrderBy(s => s.Name).Select(a => new ExtendedAddonDetailsBase(a));
                Addons = new ObservableCollection<ExtendedAddonDetailsBase>(items);
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

        private ICommand _navigateToCommand;

        public ICommand NavigateToCommand
        {
            get { return _navigateToCommand ?? (_navigateToCommand = new Command(NavigateTo)); }
        }

        private void NavigateTo(object o)
        {
            var addon = o as ExtendedAddonDetailsBase;
            if (addon == null) return;

            string url = string.Concat("/Addons/PageAddon.xaml?id=", addon.Value.AddonId, "&title=", HttpUtility.UrlEncode(addon.Value.Name));

            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }

        #endregion
    }
}