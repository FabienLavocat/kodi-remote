using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KodiRemote.Wp81.Core
{
    public class ColoredProgressBar : ProgressBar
    {
        #region InProgressColor

        public Color InProgressColor
        {
            get { return (Color) GetValue(InProgressColorProperty); }
            set { SetValue(InProgressColorProperty, value); }
        }

        public static readonly DependencyProperty InProgressColorProperty = DependencyProperty.Register("InProgressColor",
            typeof(Color), typeof(ColoredProgressBar), new PropertyMetadata(null));

        #endregion

        #region FinishedColor

        public Color FinishedColor
        {
            get { return (Color) GetValue(FinishedColorProperty); }
            set { SetValue(FinishedColorProperty, value); }
        }

        public static readonly DependencyProperty FinishedColorProperty = DependencyProperty.Register("FinishedColor",
            typeof(Color), typeof(ColoredProgressBar), new PropertyMetadata(null));

        #endregion

        public ColoredProgressBar()
        {
            ValueChanged += OnValueChanged;
        }

        private void OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Color color = e.NewValue < Maximum ? InProgressColor : FinishedColor;
            Foreground = new SolidColorBrush(color);
        }
    }
}
