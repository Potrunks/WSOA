using System.Text.RegularExpressions;
using WSOA.Shared.Resources;

namespace WSOA.Shared.Utils
{
    public static class StringUtil
    {
        /// <summary>
        /// Check if string is mail format valid or not.
        /// </summary>
        public static bool IsValidMailFormat(this string stringToValidate)
        {
            return Regex.IsMatch(stringToValidate, RegexResources.MAIL_REGEX);
        }
    }
}
