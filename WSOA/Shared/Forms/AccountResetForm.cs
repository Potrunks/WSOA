using System.ComponentModel.DataAnnotations;
using WSOA.Shared.Resources;

namespace WSOA.Shared.Forms
{
    public class AccountResetForm
    {
        [Required(ErrorMessage = DataValidationResources.TECHNICAL_ERROR)]
        public int AccountId { get; set; }

        [Required(ErrorMessage = DataValidationResources.TECHNICAL_ERROR)]
        public long ForgotPasswordKey { get; set; }

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
