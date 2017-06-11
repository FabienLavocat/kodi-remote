using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace KodiRemote.Uwp.Controls
{
    public sealed partial class LoadingIndicator : UserControl
    {
        public LoadingIndicator()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += Loading_Loaded;
        }

        #region IsRunning

        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }

        public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register(nameof(IsRunning),
            typeof(bool), typeof(LoadingIndicator), new PropertyMetadata(null));

        #endregion

        #region Text

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
            typeof(string), typeof(LoadingIndicator), new PropertyMetadata(null));

        #endregion

        private void Loading_Loaded(object sender, RoutedEventArgs e)
        {
            CirclesStoryboard.Begin();
        }
    }
}
