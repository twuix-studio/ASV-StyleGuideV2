using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Fei.SliceAndView.Common.Converters
{
    public class NumberComparisonToObjectConverter : IValueConverter
    {
        public enum ComparisonType
        {
            LessThan,
            LessThanEqual,
            Equal,
            GreaterThan,
            GreaterThanEqual,
            NotEqual
        }

        public ComparisonType Comparison
        {
            get;
            set;
        }

        public object TrueValue
        {
            get;
            set;
        }

        public object FalseValue
        {
            get;
            set;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double numericValue = System.Convert.ToDouble(value);
                double numericParameter = System.Convert.ToDouble(parameter);
                bool returnValue = false;

                switch(Comparison)
                {
                    case ComparisonType.LessThan:
                        returnValue = numericValue < numericParameter;
                        break;
                    case ComparisonType.LessThanEqual:
                        returnValue = numericValue <= numericParameter;
                        break;
                    case ComparisonType.Equal:
                        returnValue = numericValue == numericParameter;
                        break;
                    case ComparisonType.GreaterThan:
                        returnValue = numericValue > numericParameter;
                        break;
                    case ComparisonType.GreaterThanEqual:
                        returnValue = numericValue >= numericParameter;
                        break;
                    case ComparisonType.NotEqual:
                        returnValue = numericValue != numericParameter;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (returnValue)
                {
                    return TrueValue;
                }
                else
                {
                    return FalseValue;
                }
            }
            catch(Exception)
            {
                return FalseValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
