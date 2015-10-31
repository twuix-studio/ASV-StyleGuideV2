using System;
using System.Windows.Data;

namespace Fei.SliceAndView.Common.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    public class PercentageConverter : IValueConverter
    {
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double dValue = System.Convert.ToDouble(value);
                return string.Format("{0}*", dValue);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
