using System.ComponentModel.DataAnnotations;

namespace WSOA.Shared.Entity
{
    public class Profile
    {
        [Key]
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
