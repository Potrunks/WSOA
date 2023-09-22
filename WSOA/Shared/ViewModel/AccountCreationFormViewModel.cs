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
                string firstNameTrimed = _firstName?.Trim();
                return char.ToUpper(firstNameTrimed[0]) + firstNameTrimed.Substring(1);
            }
            set
            {
                _firstName = value;
            }
        }
        private string _firstName;

        [Required(ErrorMessage = DataValidationResources.LASTNAME_MISSING)]
        [RegularExpression(RegexResources.NAME, ErrorMessage = DataValidationResources.LASTNAME_FORMAT_NO_VALID)]
        public string LastName
        {
            get
            {
                return _lastName?.ToUpper().Trim();
            }
            set
            {
                _lastName = value;
            }
        }
        private string _lastName;

        [Required(ErrorMessage = DataValidationResources.MAIL_MISSING)]
        [RegularExpression(RegexResources.MAIL, ErrorMessage = DataValidationResources.MAIL_FORMAT_NO_VALID)]
        public string Email
        {
            get
            {
                return _email?.Trim();
            }
            set
            {
                _email = value;
            }
        }
        private string _email;

        [Required(ErrorMessage = DataValidationResources.LOGIN_MISSING)]
        [RegularExpression(RegexResources.LOGIN, ErrorMessage = DataValidationResources.LOGIN_FORMAT_NO_VALID)]
        public string Login
        {
            get
            {
                return _login?.Trim();
            }
            set
            {
                _login = value;
            }
        }
        private string _login;

        [Required(ErrorMessage = DataValidationResources.PASSWORD_MISSING)]
        [RegularExpression(RegexResources.PASSWORD, ErrorMessage = DataValidationResources.PASSWORD_FORMAT_NO_VALID)]
        public string Password
        {
            get
            {
                return _password?.Trim();
            }
            set
            {
                _password = value;
            }
        }
        private string _password;

        [Required(ErrorMessage = DataValidationResources.PASSWORD_CONFIRMATION_MISSING)]
        [Compare(nameof(Password), ErrorMessage = DataValidationResources.PASSWORD_CONFIRMATION_NO_VALID)]
        public string PasswordConfirmation
        {
            get
            {
                return _passwordConfirmation?.Trim();
            }
            set
            {
                _passwordConfirmation = value;
            }
        }
        private string _passwordConfirmation;
    }
}
