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
                return _login?.Trim();
            }
            set
            {
                _login = value;
            }
        }
        private string? _login;

        [Required(ErrorMessage = DataValidationResources.PASSWORD_MISSING)]
        public string? Password
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
        private string? _password;
    }
}
