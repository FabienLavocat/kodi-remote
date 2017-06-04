using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using KodiRemote.Wp81.Core;

namespace KodiRemote.Wp81.Converters
{
    public class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ConnectionStatus statusA = (ConnectionStatus)value;
            ConnectionStatus statusB = (ConnectionStatus)Enum.Parse(typeof(ConnectionStatus), parameter.ToString());

            return (statusA == statusB ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}