using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace KodiRemote.Uwp.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool flag = false;
            if (value is bool)
                flag = (bool)value;

            return (flag ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return ((value is Visibility) && (((Visibility)value) == Visibility.Visible));
        }
    }
}
