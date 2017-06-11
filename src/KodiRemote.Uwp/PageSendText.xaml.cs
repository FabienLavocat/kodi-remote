using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp
{
    public sealed partial class PageSendText : Page
    {
        public PageSendText()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;
            Loaded += PageSendText_Loaded;
        }

        #region TextToSend

        public string TextToSend
        {
            get { return (string)GetValue(TextToSendProperty); }
            set { SetValue(TextToSendProperty, value); }
        }

        public static readonly DependencyProperty TextToSendProperty =
            DependencyProperty.Register(nameof(TextToSend), typeof(string), typeof(PageSendText), new PropertyMetadata(null));

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = new Windows.UI.Color() { R = 40, G = 42, B = 43 };
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.White;
            }
        }

        private void PageSendText_Loaded(object sender, RoutedEventArgs e)
        {
            TxtTextToSend.Focus();
        }

        public async void ButtonSendClick(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Kodi.IsMocked || string.IsNullOrWhiteSpace(TextToSend)) return;

            await App.Context.Connection.Kodi.Input.SendTextAsync(TextToSend);
        }
    }
}
