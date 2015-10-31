using System.Windows.Controls;
using Fei.SliceAndView.Common.Utilities;

namespace Fei.SliceAndView.Common.Validation
{
    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string text = value as string;
            if (text != null && ValidationUtils.IsEmailValid(text))
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, null);   //todo: add error info
        }
    }
}
