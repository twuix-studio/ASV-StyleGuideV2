using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using Fei.Applications.SAL;

namespace Fei.SliceAndView.Common.Converters
{
    public class ScanResolutionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is ScanResolution)) 
                return DependencyProperty.UnsetValue;
            
            ScanResolution resolution = (ScanResolution)value;
            return string.Format("{0} × {1}", resolution.Width, resolution.Height);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text = value as string;
            if(text == null)
                return null;

            string pattern = @"(\d+)\s*[×X]{1}\s*(\d+)";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = r.Match(text);

            if (match.Success)
            {
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
