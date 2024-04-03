using System.ComponentModel.DataAnnotations;

namespace WSOA.Shared.Entity
{
    public class LinkAccountCreation
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string RecipientMail { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        public string ProfileCode { get; set; }
    }
}
