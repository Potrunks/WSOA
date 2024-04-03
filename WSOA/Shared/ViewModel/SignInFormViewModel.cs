using System.ComponentModel.DataAnnotations;
using WSOA.Shared.Resources;

namespace WSOA.Shared.ViewModel
{
    public class SignInFormViewModel
    {
        [Required(ErrorMessage = DataValidationResources.LOGIN_MISSING)]
        public string? Login
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
        private string? _login;

        [Required(ErrorMessage = DataValidationResources.PASSWORD_MISSING)]
        public string? Password
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
        private string? _password;
    }
}
