using System;
using System.Numerics;
using KodiRemote.Uwp.Core;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp.Addons
{
    public sealed partial class PageAddon : Page
    {
        private readonly ResourceLoader _resourceLoader;

        public PageAddon()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;

            _resourceLoader = ResourceLoader.GetForCurrentView();
        }

        #region AddonName

        public string AddonName
        {
            get { return (string)GetValue(AddonNameProperty); }
            private set { SetValue(AddonNameProperty, value); }
        }

        public static readonly DependencyProperty AddonNameProperty =
            DependencyProperty.Register(nameof(AddonName), typeof(string), typeof(PageAddon), new PropertyMetadata(null));

        #endregion

        #region AddonDetails

        public ExtendedAddonDetailsBase AddonDetails
        {
            get { return (ExtendedAddonDetailsBase)GetValue(AddonDetailsProperty); }
            private set { SetValue(AddonDetailsProperty, value); }
        }

        public static readonly DependencyProperty AddonDetailsProperty =
            DependencyProperty.Register(nameof(AddonDetails), typeof(ExtendedAddonDetailsBase), typeof(PageAddon), new PropertyMetadata(null));

        #endregion

        #region IsLoading

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(PageAddon), new PropertyMetadata(false));

        #endregion

        private string _addonId = null;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int index = e.Parameter.ToString().IndexOf("|");
            _addonId = e.Parameter.ToString().Substring(0, index);
            AddonName = e.Parameter.ToString().Substring(index + 1);

            Refresh();
        }

        private async void Refresh()
        {
            IsLoading = true;

            try
            {
                var addon = await App.Context.Connection.Kodi.Addons.GetAddonDetailsAsync(_addonId);
                AddonDetails = new ExtendedAddonDetailsBase(addon);
                AddonDetails.Value.Description = AddonDetails.Value.Description.Replace("[CR]", "\n");
                GetImageAsync(addon.Thumbnail);
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

        private async void GetImageAsync(string thumbnail)
        {
            if (string.IsNullOrWhiteSpace(thumbnail))
                return;

            try
            {
                var download = await App.Context.Connection.Kodi.Files.PrepareDownloadAsync(thumbnail);
                _source = App.Context.Connection.Kodi.GetFileUrl(download.Details.Path);
                if (_source != null)
                {
                    ImageBack.Source = new BitmapImage(new Uri(_source, UriKind.RelativeOrAbsolute));

                    CanvasControl cc = new CanvasControl();
                    cc.Height = 400;
                    cc.CreateResources += ImageBackBlur_CreateResources;
                    cc.Draw += ImageBackBlur_Draw;
                    ImageBackBlur.Children.Add(cc);
                }
            }
            catch { }
        }

        private string _source = null;
        private CanvasBitmap _image = null;

        private async void ImageBackBlur_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
            if (_source == null) return;

            _image = await CanvasBitmap.LoadAsync(sender.Device, new Uri(_source));
            sender.Invalidate();
        }

        private void ImageBackBlur_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (_image == null) return;

            using (var session = args.DrawingSession)
            {
                var scaleEffect = new ScaleEffect();
                var blurEffect = new GaussianBlurEffect();

                session.Units = CanvasUnits.Pixels;

                double displayScaling = DisplayInformation.GetForCurrentView().LogicalDpi / 96.0;

                double pixelWidth = sender.ActualWidth * displayScaling;
                
                var scalefactor = pixelWidth / _image.Size.Width;

                scaleEffect.Source = _image;
                scaleEffect.Scale = new Vector2()
                {
                    X = (float)scalefactor,
                    Y = (float)scalefactor
                };

                blurEffect.Source = scaleEffect;
                blurEffect.BlurAmount = 145.0f;

                session.DrawImage(blurEffect, 0.0f, 0.0f);
            }
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
            await App.Context.Connection.Kodi.Addons.SetAddonEnabledAsync(addonId, enabled);
            IsLoading = false;

            Refresh();
        }
    }
}
