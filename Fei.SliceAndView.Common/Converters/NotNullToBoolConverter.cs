
using System.Windows.Data;

namespace Fei.SliceAndView.Common.Converters
{
    public class IsNullToObjectConverter : IValueConverter
    {
        public object NullValue
        {
            get;
            set;
        }

        public object NotNullValue
        {
            get;
            set;
        }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return NullValue;
            }
            else
            {
                return NotNullValue;
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}



