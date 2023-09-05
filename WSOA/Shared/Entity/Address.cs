using System.ComponentModel.DataAnnotations;

namespace WSOA.Shared.Entity
{
    public class Address
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
