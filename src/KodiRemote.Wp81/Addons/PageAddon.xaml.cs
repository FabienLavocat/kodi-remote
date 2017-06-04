using System;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.Addons
{
    public partial class PageAddon
    {
        #region AddonName

        public string AddonName
        {
            get { return (string)GetValue(AddonNameProperty); }
            private set { SetValue(AddonNameProperty, value); }
        }

        public static readonly DependencyProperty AddonNameProperty =
            DependencyProperty.Register("AddonName", typeof(string), typeof(PageAddon), new PropertyMetadata(null));

        #endregion

        #region AddonDetails

        public ExtendedAddonDetailsBase AddonDetails
        {
            get { return (ExtendedAddonDetailsBase)GetValue(AddonDetailsProperty); }
            private set { SetValue(AddonDetailsProperty, value); }
        }

        public static readonly DependencyProperty AddonDetailsProperty =
            DependencyProperty.Register("AddonDetails", typeof(ExtendedAddonDetailsBase), typeof(PageAddon), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageAddon), new PropertyMetadata(false));

        #endregion

        public PageAddon()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Refresh();
        }

        private async void Refresh()
        {
            string setTitle;
            string addonId;
            if (!NavigationContext.QueryString.TryGetValue("id", out addonId)
                || !NavigationContext.QueryString.TryGetValue("title", out setTitle))
            {
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();

                return;
            }

            AddonName = HttpUtility.UrlDecode(setTitle);
            IsLoading = true;

            try
            {
                var addon = await App.Context.Connection.Xbmc.Addons.GetAddonDetailsAsync(addonId);
                AddonDetails = new ExtendedAddonDetailsBase(addon);
                AddonDetails.Value.Description = AddonDetails.Value.Description.Replace("[CR]", "\n");
                GetImageAsync(addon.Thumbnail);
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

        private async void GetImageAsync(string thumbnail)
        {
            if (string.IsNullOrWhiteSpace(thumbnail))
                return;

            try
            {
                var download = await App.Context.Connection.Xbmc.Files.PrepareDownloadAsync(thumbnail);
                string url = App.Context.Connection.Xbmc.GetFileUrl(download.Details.Path);
                if (url != null)
                    SetImage(url);
            }
            catch { }
        }

        private void SetImage(string source)
        {
            BitmapImage img1 = new BitmapImage(new Uri(source, UriKind.RelativeOrAbsolute));
            ImageBack.Source = img1;

            img1.CreateOptions = BitmapCreateOptions.None;
            img1.ImageOpened += (s, e) =>
            {
                WriteableBitmap bitmap = new WriteableBitmap(img1);
                WriteableBitmapConvolutionExtensions.BoxBlur(bitmap, 145);

                ImageBackBlur.Source = bitmap;
            };
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            EnableAddon(AddonDetails.Value.AddonId, true);
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            EnableAddon(AddonDetails.Value.AddonId, false);
        }

        private async void EnableAddon(string addonId, bool enabled)
        {
            if (AddonDetails.Value.Enabled == enabled) return;

            IsLoading = true;
            await App.Context.Connection.Xbmc.Addons.SetAddonEnabledAsync(addonId, enabled);
            IsLoading = false;

            Refresh();
        }
    }
}