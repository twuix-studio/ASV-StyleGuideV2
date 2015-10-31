using System;
using System.Windows.Data;
using Fei.Common.Quantities;
using Fei.Common.Types;
using Fei.SharedObjects;

namespace Fei.SliceAndView.Common.Converters
{
    public class PhysicalValueConverter : IValueConverter
    {
        #region Constructors

        public PhysicalValueConverter()
        {
            // set default values
            DefaultUnit = Unit.None;
            Format = PhysicalValueFormat.SI;
            MinimumPrefix = SIPrefix.Femto;
            MaximumPrefix = SIPrefix.Peta;
            DefaultPrefix = SIPrefix.None;
            NumberOfDigits = 0;
            Precision = 0.0;
            MaximumMagnitude = 0.0;
        }

        #endregion

        public Unit DefaultUnit { get; set; }
        public PhysicalValueFormat Format { get; set; }
        public SIPrefix MinimumPrefix { get; set; }
        public SIPrefix MaximumPrefix { get; set; }
        public SIPrefix DefaultPrefix { get; set; }
        public int NumberOfDigits { get; set; }
        public double Precision { get; set; }
        public double MaximumMagnitude { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Unit unit;
            double doubleValue;
            if (value is PhysicalValue)
            {
                PhysicalValue physicalValue = (PhysicalValue)value;
                unit = physicalValue.Unit;
                doubleValue = physicalValue.Value;
            }
            else if (value is Quantity)
            {
                Quantity quantity = (Quantity)value;
                unit = quantity.Unit;
                doubleValue = quantity.Value;
            }
            else if (value is double)
            {
                unit = DefaultUnit;
                doubleValue = (double)value;
            }
            else
            {
                return null;
            }

            return PhysicalValue.Format(doubleValue, unit, Format, MinimumPrefix, MaximumPrefix, DefaultPrefix, NumberOfDigits, Precision, MaximumMagnitude);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            PhysicalValue result;
            if (PhysicalValue.TryParse(value.ToString(), DefaultUnit, out result))
            {
                return result.Value;
            }
            else
            {
                return double.NaN;
            }
        }
    }
}
