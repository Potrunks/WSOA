using System.ComponentModel.DataAnnotations;

namespace WSOA.Shared.ViewModel
{
    public class SignInFormViewModel
    {
        [Required(ErrorMessage = "Login manquant")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Mot de passe manquant")]
        public string? Password { get; set; }
    }
}
