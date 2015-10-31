using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Fei.SliceAndView.Common.Converters
{
    [ContentProperty("Converters")]
    public class ConverterGroupConverter : IValueConverter
    {
        private readonly Collection<IValueConverter> converters = new Collection<IValueConverter>();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Collection<IValueConverter> Converters
        {
            get { return converters; }
        }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            for (int i = 0; i < converters.Count; i++)
            {
                value = converters[i].Convert(value, targetType, parameter, culture);
            }

            return value;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            for (int i = converters.Count - 1; i >= 0; i--)
            {
                value = converters[i].ConvertBack(value, targetType, parameter, culture);
            }

            return value;
        }
    }
}
