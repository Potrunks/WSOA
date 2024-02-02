namespace WSOA.Shared.Utils
{
    public static class StringFormatUtil
    {
        /// <summary>
        /// Format firstname and lastname like this : Alexis A.
        /// </summary>
        public static string ToFullFirstNameAndFirstLetterLastName(string firstName, string lastName)
        {
            return $"{firstName.Substring(0, 1).ToUpper()}{firstName.Substring(1).ToLower()} {lastName.Substring(0, 1).ToUpper()}.";
        }
    }
}
