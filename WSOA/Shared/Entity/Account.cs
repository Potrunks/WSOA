using System.ComponentModel.DataAnnotations;

namespace WSOA.Shared.Entity
{
    public class Account
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime? ForgotPasswordExpirationDate { get; set; }

        public long? ForgotPasswordKey { get; set; }
    }
}
