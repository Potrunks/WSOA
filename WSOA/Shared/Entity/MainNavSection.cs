using System.ComponentModel.DataAnnotations;

namespace WSOA.Shared.Entity
{
    public class MainNavSection
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Label { get; set; }

        [Required]
        public string ClassIcon { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
