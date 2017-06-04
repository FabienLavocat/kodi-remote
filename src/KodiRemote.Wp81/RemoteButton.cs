using System.Windows;
using System.Windows.Controls;

namespace KodiRemote.Wp81
{
    public class RemoteButton : Button
    {
        public object PressedContent
        {
            get { return GetValue(PressedContentProperty); }
            set { SetValue(PressedContentProperty, value); }
        }

        public object TopContent
        {
            get { return GetValue(TopContentProperty); }
            set { SetValue(TopContentProperty, value); }
        }

        public static readonly DependencyProperty PressedContentProperty =
            DependencyProperty.Register("PressedContent", typeof(object), typeof(RemoteButton), new PropertyMetadata(null));

        public static readonly DependencyProperty TopContentProperty =
            DependencyProperty.Register("TopContent", typeof(object), typeof(RemoteButton), new PropertyMetadata(null));
    }
}