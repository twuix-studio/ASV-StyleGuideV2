using System;
using System.Windows;
using System.Windows.Data;

namespace Fei.SliceAndView.Common.Converters
{
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class TextAvailableToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.IsNullOrEmpty((string)value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
