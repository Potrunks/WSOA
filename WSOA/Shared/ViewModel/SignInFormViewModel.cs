using System.ComponentModel.DataAnnotations;
using WSOA.Shared.Resources;

namespace WSOA.Shared.ViewModel
{
    public class SignInFormViewModel
    {
        [Required(ErrorMessage = DataValidationResources.LOGIN_MISSING)]
        public string? Login { get; set; }

        [Required(ErrorMessage = DataValidationResources.PASSWORD_MISSING)]
        public string? Password { get; set; }
    }
}
