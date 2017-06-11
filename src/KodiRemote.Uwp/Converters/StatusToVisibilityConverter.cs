using System;
using KodiRemote.Uwp.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace KodiRemote.Uwp.Converters
{
    public class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ConnectionStatus statusA = (ConnectionStatus)value;
            ConnectionStatus statusB = (ConnectionStatus)Enum.Parse(typeof(ConnectionStatus), parameter.ToString());

            return (statusA == statusB ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
