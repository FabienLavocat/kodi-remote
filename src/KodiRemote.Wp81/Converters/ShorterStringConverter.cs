using System;
using System.Globalization;
using System.Windows.Data;

namespace KodiRemote.Wp81.Converters
{
    public class ShorterStringConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return string.Empty;
            
            int max;
            if (!int.TryParse(parameter.ToString(), out max)) return value;

            string str = value.ToString();
            if (str.Length <= max) return str;

            return str.Substring(0, max - 3) + "...";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}