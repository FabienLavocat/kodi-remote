using System;
using Windows.UI.Xaml.Data;

namespace KodiRemote.Uwp.Converters
{
    public class SecondsToStringConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is int)) return "00:00";

            int lenght = (int)value;

            int hour = 0;
            int minutes = lenght / 60;
            int seconds = lenght % 60;

            if (minutes > 60)
            {
                hour = minutes / 60;
                minutes = minutes % 60;
            }

            return hour > 0
                       ? string.Format("{0}:{1:00}:{2:00}", hour, minutes, seconds)
                       : string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class MilliSecondsToStringConverter : SecondsToStringConverter
    {
        public override object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is int)) return "00:00";

            int lenght = (int)value / 1000;
            return base.Convert(lenght, targetType, parameter, language);
        }
    }
}
