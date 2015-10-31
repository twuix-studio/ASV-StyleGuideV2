using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fei.SliceAndView.Controls;

namespace Fei.SliceAndView.Common.Validation
{
    public static class ErrorRecovery
    {
        static ErrorRecovery()
        {
            ProcessRecoveryProperty = DependencyProperty.RegisterAttached("ProcessRecovery", typeof(bool), typeof(ErrorRecovery), new PropertyMetadata(false, RecoverErrorChanged));
        }

        public static bool GetProcessRecovery(DependencyObject obj)
        {
            return (bool)obj.GetValue(ProcessRecoveryProperty);
        }

        public static void SetProcessRecovery(DependencyObject obj, bool value)
        {
            obj.SetValue(ProcessRecoveryProperty, value);
        }

        public static readonly DependencyProperty ProcessRecoveryProperty;

        private static void RecoverErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool processRecovery = (bool)e.NewValue;
            if (processRecovery)
            {
                // we should recover from error - update value from view model
                var frameworkElement = d as FrameworkElement;
                if (frameworkElement != null)
                {
                    foreach (ValidationError error in System.Windows.Controls.Validation.GetErrors(frameworkElement))
                    {
                        if (error.BindingInError is BindingExpressionBase)
                        {
                            (error.BindingInError as BindingExpressionBase).UpdateTarget();
                        }
                    }
                }

                if (frameworkElement is NumericEditor)
                {
                    var numericEditor = ((NumericEditor)frameworkElement);
                    numericEditor.UpdateText();
                    numericEditor.SelectNumericText();

                }
                else if (frameworkElement is TextBox)
                {
                    var textBox = ((TextBox)frameworkElement);
                    textBox.SelectionLength = 0;
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

    }
}
