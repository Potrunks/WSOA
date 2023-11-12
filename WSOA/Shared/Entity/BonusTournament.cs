using System.ComponentModel.DataAnnotations;

namespace WSOA.Shared.Entity
{
    public class BonusTournament
    {
        public BonusTournament()
        {

        }

        [Key]
        [Required]
        public string Code { get; set; }

        [Required]
        public string Label { get; set; }

        [Required]
        public int PointAmount { get; set; }

        [Required]
        public string LogoPath { get; set; }
    }
}
