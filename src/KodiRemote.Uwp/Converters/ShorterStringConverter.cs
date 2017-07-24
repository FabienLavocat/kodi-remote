using System;
using Windows.UI.Xaml.Data;

namespace KodiRemote.Uwp.Converters
{
    public class ShorterStringConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null) return string.Empty;

            if (!int.TryParse(parameter.ToString(), out int max)) return value;

            string str = value.ToString();
            if (str.Length <= max) return str;

            return str.Substring(0, max - 3) + "...";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
