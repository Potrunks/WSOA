using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSOA.Shared.Entity
{
    public class BonusTournamentEarned
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(BonusTournament))]
        public string BonusTournamentCode { get; set; }
        public BonusTournament BonusTournament { get; set; }

        [Required]
        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        [Required]
        public int PointAmount { get; set; }
    }
}
