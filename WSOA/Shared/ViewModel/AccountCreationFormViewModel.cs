using System.ComponentModel.DataAnnotations;
using WSOA.Shared.Resources;

namespace WSOA.Shared.ViewModel
{
    public class AccountCreationFormViewModel
    {
        [Required(ErrorMessage = DataValidationResources.FIRSTNAME_MISSING)]
        [RegularExpression(RegexResources.NAME, ErrorMessage = DataValidationResources.FIRSTNAME_FORMAT_NO_VALID)]
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                string firstNameTrimed = value?.Trim();
                _firstName = firstNameTrimed == null ? null : char.ToUpper(firstNameTrimed[0]) + firstNameTrimed.Substring(1);
            }
        }
        private string _firstName;

        [Required(ErrorMessage = DataValidationResources.LASTNAME_MISSING)]
        [RegularExpression(RegexResources.NAME, ErrorMessage = DataValidationResources.LASTNAME_FORMAT_NO_VALID)]
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value?.ToUpper().Trim();
            }
        }
        private string _lastName;

        [Required(ErrorMessage = DataValidationResources.MAIL_MISSING)]
        [RegularExpression(RegexResources.MAIL, ErrorMessage = DataValidationResources.MAIL_FORMAT_NO_VALID)]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value?.ToLower().Trim();
            }
        }
        private string _email;

        [Required(ErrorMessage = DataValidationResources.LOGIN_MISSING)]
        [RegularExpression(RegexResources.LOGIN, ErrorMessage = DataValidationResources.LOGIN_FORMAT_NO_VALID)]
        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value?.Trim();
            }
        }
        private string _login;

        [Required(ErrorMessage = DataValidationResources.PASSWORD_MISSING)]
        [RegularExpression(RegexResources.PASSWORD, ErrorMessage = DataValidationResources.PASSWORD_FORMAT_NO_VALID)]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value?.Trim();
            }
        }
        private string _password;

        [Required(ErrorMessage = DataValidationResources.PASSWORD_CONFIRMATION_MISSING)]
        [Compare(nameof(Password), ErrorMessage = DataValidationResources.PASSWORD_CONFIRMATION_NO_VALID)]
        public string PasswordConfirmation
        {
            get
            {
                return _passwordConfirmation;
            }
            set
            {
                _passwordConfirmation = value?.Trim();
            }
        }
        private string _passwordConfirmation;
    }
}
