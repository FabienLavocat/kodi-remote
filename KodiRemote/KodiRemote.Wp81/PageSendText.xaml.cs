using System.Windows;
using System.Windows.Navigation;

namespace KodiRemote.Wp81
{
    public partial class PageSendText
    {
        #region TextToSend

        public string TextToSend
        {
            get { return (string) GetValue(TextToSendProperty); }
            set { SetValue(TextToSendProperty, value); }
        }

        public static readonly DependencyProperty TextToSendProperty =
            DependencyProperty.Register("TextToSend", typeof(string), typeof(PageSendText), new PropertyMetadata(null));

        #endregion

        public PageSendText()
        {
            InitializeComponent();
            Loaded += PageSendText_Loaded;
        }

        private void PageSendText_Loaded(object sender, RoutedEventArgs e)
        {
            TxtTextToSend.Focus();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = this;
        }

        public async void ButtonSendClick(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connection.Xbmc.IsMocked || string.IsNullOrWhiteSpace(TextToSend)) return;

            await App.Context.Connection.Xbmc.Input.SendTextAsync(TextToSend);
        }
    }
}