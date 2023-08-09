using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSOA.Shared.Entity
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
