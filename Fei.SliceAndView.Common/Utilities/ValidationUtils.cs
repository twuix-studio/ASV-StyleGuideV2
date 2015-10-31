using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Fei.SliceAndView.Common.Utilities
{
    public static class ValidationUtils
    {
        public static bool IsProjectNameValid(string projectName)
        {
            bool isValid = projectName.IndexOfAny(GetProjectNameInvalidCharacters()) == -1;

            return isValid;
        }

        public static string GetValidProjectName(string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                return string.Empty;
            }
            else
            {
                foreach (char c in GetProjectNameInvalidCharacters())
                {
                    projectName = projectName.Replace(c.ToString(CultureInfo.InvariantCulture), string.Empty);
                }

                return projectName;
            }
        }

        private static char[] GetProjectNameInvalidCharacters()
        {
            var invalidChars = new List<char>(Path.GetInvalidPathChars());
            invalidChars.Add('.');
            invalidChars.Add('\\');
            invalidChars.Add('/');
            return invalidChars.ToArray();
        }

        public static bool IsEmailValid(string email)
        {
            var match = Regex.Match(email.ToUpper(), @"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b");

            return match.Success;
        }

    }
}
