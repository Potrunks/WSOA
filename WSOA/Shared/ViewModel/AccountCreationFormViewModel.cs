using System.ComponentModel.DataAnnotations;
using WSOA.Shared.Resources;

namespace WSOA.Shared.ViewModel
{
    public class AccountCreationFormViewModel
    {
        [Required(ErrorMessage = DataValidationResources.FIRSTNAME_MISSING)]
        [RegularExpression(RegexResources.NAME, ErrorMessage = DataValidationResources.FIRSTNAME_FORMAT_NO_VALID)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = DataValidationResources.LASTNAME_MISSING)]
        [RegularExpression(RegexResources.NAME, ErrorMessage = DataValidationResources.LASTNAME_FORMAT_NO_VALID)]
        public string LastName { get; set; }

        [Required(ErrorMessage = DataValidationResources.MAIL_MISSING)]
        [RegularExpression(RegexResources.MAIL, ErrorMessage = DataValidationResources.MAIL_FORMAT_NO_VALID)]
        public string Email { get; set; }

        [Required(ErrorMessage = DataValidationResources.LOGIN_MISSING)]
        [RegularExpression(RegexResources.LOGIN, ErrorMessage = DataValidationResources.LOGIN_FORMAT_NO_VALID)]
        public string Login { get; set; }

        [Required(ErrorMessage = DataValidationResources.PASSWORD_MISSING)]
        [RegularExpression(RegexResources.PASSWORD, ErrorMessage = DataValidationResources.PASSWORD_FORMAT_NO_VALID)]
        public string Password { get; set; }

        [Required(ErrorMessage = DataValidationResources.PASSWORD_CONFIRMATION_MISSING)]
        [Compare(nameof(Password), ErrorMessage = DataValidationResources.PASSWORD_CONFIRMATION_NO_VALID)]
        public string PasswordConfirmation { get; set; }
    }
}
