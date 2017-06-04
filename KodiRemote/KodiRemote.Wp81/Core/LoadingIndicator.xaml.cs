using System.Windows;
using System.Windows.Controls;

namespace KodiRemote.Wp81.Core
{
    public partial class LoadingIndicator : UserControl
    {
        #region IsRunning

        public bool IsRunning
        {
            get { return (bool) GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }

        public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register("IsRunning",
            typeof(bool), typeof(LoadingIndicator), new PropertyMetadata(null));

        #endregion

        #region Text

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string), typeof(LoadingIndicator), new PropertyMetadata(null));

        #endregion

        public LoadingIndicator()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += Loading_Loaded;
        }

        private void Loading_Loaded(object sender, RoutedEventArgs e)
        {
            CirclesStoryboard.Begin();
        }
    }
}
