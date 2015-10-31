using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Fei.SliceAndView.Common.Converters
{
    public class MultiBoolToVisibilityConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Any(x => x == DependencyProperty.UnsetValue))
            {
                return DependencyProperty.UnsetValue;
            }
            else
            {
                return (values.Cast<bool>().Any(value => !value)) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }

        #endregion
    }
}
