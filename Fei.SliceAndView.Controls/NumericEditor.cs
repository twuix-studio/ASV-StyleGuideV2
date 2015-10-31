using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Fei.Common.Types;
using Fei.Common.Types.Processing;
using Fei.SharedObjects;

namespace Fei.SliceAndView.Controls
{
    public static class SliceAndViewUnits
    {
        public static readonly Unit AmperePerSquareMicroMeter = new Unit("A/μm" + Unit.SuperscriptTwo);
    }

    public class NumericEditor : TextBox
    {
        public NumericEditor()
        {
            LostFocus += NumericEditor_LostFocus;

            // observe process recovery changes
            // when recovery process ends (ProcessRecoveryProperty is set to false), we should call UpdateText to ensure that valid value is displayed
            //var descriptor = DependencyPropertyDescriptor.FromProperty( ErrorRecovery.ProcessRecoveryProperty, typeof( NumericEditor ) );
            //descriptor.AddValueChanged( this, OnProcessRecoveryChanged );
        }

        public SIPrefix LastPrefix
        {
            get { return (SIPrefix)GetValue(LastPrefixProperty); }
            set { SetValue(LastPrefixProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the display format.
        /// </summary>
        /// <value>The display format.</value>
        [Category("NumericEditor")]
        public PhysicalValueFormat PhysicalValueFormat
        {
            get { return (PhysicalValueFormat)GetValue(PhysicalValueFormatProperty); }
            set { SetValue(PhysicalValueFormatProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the minimum prefix.
        /// </summary>
        /// <value>The minimum prefix.</value>
        [Category("NumericEditor")]
        public SIPrefix MinimumSIPrefix
        {
            get { return (SIPrefix)GetValue(MinimumSIPrefixProperty); }
            set { SetValue(MinimumSIPrefixProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the maximum prefix.
        /// </summary>
        /// <value>The maximum prefix.</value>
        [Category("NumericEditor")]
        public SIPrefix MaximumSIPrefix
        {
            get { return (SIPrefix)GetValue(MaximumSIPrefixProperty); }
            set { SetValue(MaximumSIPrefixProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the number of fraction digits.
        /// </summary>
        /// <value>The number of fraction digits.</value>
        [Category("NumericEditor")]
        public int NumberOfFractionDigits
        {
            get { return (int)GetValue(NumberOfFractionDigitsProperty); }
            set { SetValue(NumberOfFractionDigitsProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the number of significant digits.
        /// </summary>
        /// <value>The number of significant digits.</value>
        [Category("NumericEditor")]
        public int NumberOfSignificantDigits
        {
            get { return (int)GetValue(NumberOfSignificantDigitsProperty); }
            set { SetValue(NumberOfSignificantDigitsProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the precision.
        /// </summary>
        /// <value>The precision.</value>
        [Category("NumericEditor")]
        public double Precision
        {
            get { return (double)GetValue(PrecisionProperty); }
            set { SetValue(PrecisionProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the maximum magnitude.
        /// </summary>
        /// <value>The maximum magnitude.</value>
        [Category("NumericEditor")]
        public double MaximumMagnitude
        {
            get { return (double)GetValue(MaximumMagnitudeProperty); }
            set { SetValue(MaximumMagnitudeProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the display unit if different from the unit.
        /// </summary>
        [Category("NumericEditor")]
        public Unit? DisplayUnit
        {
            get { return (Unit?)GetValue(DisplayUnitProperty); }
            set { SetValue(DisplayUnitProperty, value); }
        }

        /// <summary>
        ///     Prefix of <see cref="DisplayUnit" /> used for parsing text values without "unit".
        /// </summary>
        /// <value>Default: <see cref="SIPrefix.None" />.</value>
        [Category("NumericEditor")]
        public SIPrefix ParsePrefix
        {
            get { return (SIPrefix)GetValue(ParsePrefixProperty); }
            set { SetValue(ParsePrefixProperty, value); }
        }

        [Category("NumericEditor")]
        public Unit Unit
        {
            get { return (Unit)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, GetClippedValue(value)); UpdateText();
            }
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        private void NumericEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            double value;
            if (!TryParseText(Text, out value))
            {
                // If cannot parse value, visualize error
                SetErrorState();
            }
            else
            {
                Value = value;
                SelectNumericText();
            }
        }

        private static void OnFormatPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((NumericEditor)d).UpdateText();
        }

        public void UpdateText()
        {
            Text = FormatValue();
        }

        private static void OnParsePrefixChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (NumericEditor)d;
            editor.LastPrefix = (SIPrefix)e.NewValue;

            OnFormatPropertiesChanged(d, e);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericEditor = (NumericEditor)d;
            numericEditor.Value = numericEditor.GetClippedValue((double)e.NewValue);
            numericEditor.UpdateText();
            numericEditor.LastPrefix = GetSIPrefix(numericEditor.Unit, (double)e.NewValue, numericEditor.MinimumSIPrefix, numericEditor.MaximumSIPrefix, numericEditor.ParsePrefix);
        }

        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericEditor = (NumericEditor)d;
            numericEditor.Value = numericEditor.GetClippedValue(numericEditor.Value);
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericEditor = (NumericEditor)d;
            numericEditor.Value = numericEditor.GetClippedValue(numericEditor.Value);
            numericEditor.UpdateText();
        }

        private double GetClippedValue(double value)
        {
            var newValue = Math.Min(Maximum, Math.Max(Minimum, value));
            return newValue;
        }

        protected string FormatValue()
        {
            return FormatValue(Value);
        }

        /// <summary>
        ///     Formats the value with the format options of the control.
        /// </summary>
        /// <param name="value">The value.</param>
        protected internal string FormatValue(double value)
        {
            var displayedUnit = Unit;
            var precision = Precision;
            var maxMagnitude = MaximumMagnitude;

            if (DisplayUnit.HasValue && DisplayUnit.Value != Unit)
            {
                value = Conversions.Convert(value, Unit, DisplayUnit.Value);
                precision = Conversions.Convert(Precision, Unit, DisplayUnit.Value);
                maxMagnitude = Conversions.Convert(MaximumMagnitude, Unit, DisplayUnit.Value);
                displayedUnit = DisplayUnit.Value;
            }
            var digits = NumberOfFractionDigits;
            if (PhysicalValueFormat == PhysicalValueFormat.SIUsingSignificantDigits)
            {
                digits = NumberOfSignificantDigits;
            }

            return PhysicalValue.Format(value, displayedUnit, PhysicalValueFormat, MinimumSIPrefix, MaximumSIPrefix, ParsePrefix, digits, precision, maxMagnitude);
        }

        private bool TryParseText(string valueText, out double numericValue)
        {
            PhysicalValue physicalValueResult;

            if (DisplayUnit.HasValue)
            {
                if (!valueText.Contains(DisplayUnit.Value.ToString()))
                {
                    valueText = valueText + " " + DisplayUnit.Value;
                }
                PhysicalValue.TryParse(valueText, DisplayUnit.Value, LastPrefix, out physicalValueResult);

                if (!double.IsNaN(physicalValueResult.Value))
                {
                    numericValue = physicalValueResult.ConvertTo(Unit).Value;
                }
                else
                {
                    numericValue = Value;
                }
            }
            else
            {
                if (!PhysicalValue.TryParse(valueText, Unit, LastPrefix, out physicalValueResult))
                {
                    physicalValueResult.Value = Double.NaN;
                }

                numericValue = double.IsNaN(physicalValueResult.Value) ? Value : physicalValueResult.Value;
            }

            return !double.IsNaN(physicalValueResult.Value);
        }

        public static SIPrefix GetSIPrefix(Unit unit, double value, SIPrefix minimumPrefix, SIPrefix maximumPrefix, SIPrefix defaultPrefix, int signiFicantDigits = 15)
        {
            if (unit == Unit.DegreeAngle || unit == Unit.Percent || unit == Unit.None)
            {
                return SIPrefix.None;
            }
            if (Math.Abs(value) > Double.Epsilon)
            {
                value = Math.Abs(value);
                var prefixBase = 3 * unit.PrefixExponent;
                double significantDigitsBias = 1;
                if (signiFicantDigits < Math.Abs(3 * unit.PrefixExponent))
                {
                    significantDigitsBias = Math.Pow(10, Math.Abs(3 * unit.PrefixExponent - signiFicantDigits));
                }
                var exponent = (int)Math.Floor((Math.Log10(value * significantDigitsBias)) / prefixBase) * prefixBase;
                return PrefixFromExponent(exponent / unit.PrefixExponent, minimumPrefix, maximumPrefix);
            }
            //use the more suitable prefix from limits for value == 0.0
            return (SIPrefix)Math.Max((int)minimumPrefix, Math.Min((int)defaultPrefix, (int)maximumPrefix));
        }

        public static SIPrefix PrefixFromExponent(int exponent, SIPrefix minimumPrefix, SIPrefix maximumPrefix)
        {
            if (exponent < (int)minimumPrefix)
            {
                return minimumPrefix;
            }
            if (exponent > (int)maximumPrefix)
            {
                return maximumPrefix;
            }
            return (SIPrefix)(exponent - exponent % 3);
        }


        #region Overrides of TextBoxBase

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                double value;
                if (!TryParseText(Text, out value))
                {
                    // If cannot parse value, visualize error
                    SetErrorState();
                    e.Handled = true;
                }
                else
                {
                    if (Math.Abs(value - Value) > Double.Epsilon)
                    {
                        e.Handled = true;
                    }
                    Value = value;
                    UpdateText();
                    SelectNumericText();
                }
            }
            else if (e.Key == Key.Escape)
            {
                // set text corresponding to current value
                UpdateText();

                // focus focusable parent
                FrameworkElement parent = Parent as FrameworkElement;
                while (parent != null && parent is IInputElement && !((IInputElement)parent).Focusable)
                {
                    parent = (FrameworkElement)parent.Parent;
                }

                DependencyObject scope = FocusManager.GetFocusScope(this);
                FocusManager.SetFocusedElement(scope, parent);

                // remove selection
                Select(0, 0);

                e.Handled = true;
            }
        }

        /// <summary>
        /// Set value binding into error state
        /// </summary>
        private void SetErrorState()
        {
            BindingExpression bindingExpression = this.GetBindingExpression(ValueProperty);
            if (bindingExpression != null)
            {
                ValidationError validationError = new ValidationError(new DataErrorValidationRule(), bindingExpression);
                System.Windows.Controls.Validation.MarkInvalid(bindingExpression, validationError);
            }
        }

        /// <summary>
        ///     Selects the numeric part of the text.
        /// </summary>
        public void SelectNumericText()
        {
            var numeric = PhysicalValue.ExtractNumericPart(Text);
            var selectionStart = Text.IndexOf(numeric, StringComparison.Ordinal);
            var selectionLength = numeric.Length;
            Select(selectionStart, selectionLength);
        }

        #endregion


        /// <summary>
        ///     The dependency property for the display format
        /// </summary>
        public static readonly DependencyProperty PhysicalValueFormatProperty = DependencyProperty.Register("PhysicalValueFormat", typeof(PhysicalValueFormat), typeof(NumericEditor),
            new FrameworkPropertyMetadata(PhysicalValueFormat.SI, OnFormatPropertiesChanged));

        /// <summary>
        ///     The dependency property for the minimum prefix
        /// </summary>
        public static readonly DependencyProperty MinimumSIPrefixProperty = DependencyProperty.Register("MinimumSIPrefix", typeof(SIPrefix), typeof(NumericEditor),
            new FrameworkPropertyMetadata(SIPrefix.Pico, OnFormatPropertiesChanged));

        /// <summary>
        ///     The dependency property for the maximum prefix
        /// </summary>
        public static readonly DependencyProperty MaximumSIPrefixProperty = DependencyProperty.Register("MaximumSIPrefix", typeof(SIPrefix), typeof(NumericEditor),
            new FrameworkPropertyMetadata(SIPrefix.Tera, OnFormatPropertiesChanged));

        /// <summary>
        ///     The dependency property for the number of fraction digits
        /// </summary>
        public static readonly DependencyProperty NumberOfFractionDigitsProperty = DependencyProperty.Register("NumberOfFractionDigits", typeof(int), typeof(NumericEditor),
            new FrameworkPropertyMetadata(0, OnFormatPropertiesChanged));

        // Using a DependencyProperty as the backing store for NumberOfSignificanDigits.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberOfSignificantDigitsProperty = DependencyProperty.Register("NumberOfSignificantDigits", typeof(int), typeof(NumericEditor),
            new FrameworkPropertyMetadata(0, OnFormatPropertiesChanged));

        /// <summary>
        ///     The dependency property for the display format. ie 0.01
        /// </summary>
        public static readonly DependencyProperty PrecisionProperty = DependencyProperty.Register("Precision", typeof(double), typeof(NumericEditor),
            new FrameworkPropertyMetadata(1.0, OnFormatPropertiesChanged));

        /// <summary>
        ///     The dependency property for the maximum magnitude
        /// </summary>
        public static readonly DependencyProperty MaximumMagnitudeProperty = DependencyProperty.Register("MaximumMagnitude", typeof(double), typeof(NumericEditor),
            new FrameworkPropertyMetadata(0.0, OnFormatPropertiesChanged));

        /// <summary>
        ///     The dependency property for the display unit
        /// </summary>
        public static readonly DependencyProperty DisplayUnitProperty = DependencyProperty.Register("DisplayUnit", typeof(Unit?), typeof(NumericEditor),
            new FrameworkPropertyMetadata(null, OnFormatPropertiesChanged));

        /// <summary>
        ///      last Used prefix is used if changing value that allready had prefix and not entering new prefix
        /// </summary>
        public static readonly DependencyProperty LastPrefixProperty = DependencyProperty.Register("LastPrefix", typeof(SIPrefix), typeof(NumericEditor), new UIPropertyMetadata(SIPrefix.None));

        /// <summary>
        ///      Prefix that will be used for parsing when User inputs value without prefix. In expression "10 nA",  "n" is the Prefix. 
        /// </summary>
        public static readonly DependencyProperty ParsePrefixProperty = DependencyProperty.Register("ParsePrefix", typeof(SIPrefix), typeof(NumericEditor),
            new UIPropertyMetadata(SIPrefix.None, OnParsePrefixChanged));

        /// <summary>
        ///      In expression "10 nA",  "A" is the Unit
        /// </summary>
        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register("Unit", typeof(Unit), typeof(NumericEditor),
            new FrameworkPropertyMetadata(Unit.None, OnFormatPropertiesChanged));

        /// <summary>
        ///      In expression "10 nA",  "10e-9" is the Value
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(NumericEditor),
            new FrameworkPropertyMetadata(0.0, OnValueChanged));

        /// <summary>
        ///      In expression "10 nA",  Minimum could be "5e-9" (A)
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(NumericEditor),
            new FrameworkPropertyMetadata(double.MinValue, OnMinimumChanged));

        /// <summary>
        ///      In expression "10 nA",  Maximum could be "20e-9" (A)
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(NumericEditor),
            new FrameworkPropertyMetadata(double.MaxValue, OnMaximumChanged));
    }
}
