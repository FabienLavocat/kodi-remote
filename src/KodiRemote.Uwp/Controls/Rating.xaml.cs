using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace KodiRemote.Uwp.Controls
{
    public sealed partial class Rating : UserControl
    {
        public Rating()
        {
            InitializeComponent();
        }

        #region Value

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set
            {
                int val = Math.Max(0, Math.Min(value, Maximum));
                SetValue(ValueProperty, val);
            }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(int), typeof(Rating), new PropertyMetadata(0, new PropertyChangedCallback(ValueChanged)));

        private static void ValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UIElementCollection children = ((sender as Rating)?.Content as Panel)?.Children;
            if (children == null || !children.Any()) return;

            int ratingValue = (int)e.NewValue;
            for (int i = 0; i < children.Count; i++)
                (children[i] as TextBlock).Text = i <= ratingValue ? "\uE0B4" : "\uE224";
        }

        #endregion

        #region Maximum

        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            nameof(Maximum), typeof(int), typeof(Rating), new PropertyMetadata(0, new PropertyChangedCallback(MaximumChanged)));

        private static void MaximumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Rating parent = sender as Rating;
            if (parent == null) return;
            int MaximumValue = (int)e.NewValue;

            for (int i = 1; i <= MaximumValue; i++)
            {
                var textBlock = new TextBlock { Text = "\uE224" };
                (parent.Content as Panel)?.Children.Add(textBlock);
            }
        }

        #endregion
    }
}
