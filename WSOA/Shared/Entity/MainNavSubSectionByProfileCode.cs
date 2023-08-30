using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSOA.Shared.Entity
{
    public class MainNavSubSectionByProfileCode
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Profile))]
        public string ProfileCode { get; set; }
        public Profile Profile { get; set; }

        [Required]
        [ForeignKey(nameof(MainNavSubSection))]
        public int MainNavSubSectionId { get; set; }
        public MainNavSubSection MainNavSubSection { get; set; }
    }
}
