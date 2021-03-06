﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KodiRemote.Wp81.Converters
{
    public class InvertedBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool)
                flag = (bool)value;

            return (flag ? Visibility.Collapsed : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((value is Visibility) && (((Visibility)value) != Visibility.Visible));
        }
    }
}
