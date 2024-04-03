using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSOA.Shared.Entity
{
    public class Player
    {
        public Player()
        {

        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        public int? TotalWinningsPoint { get; set; }

        public int? CurrentTournamentPosition { get; set; }

        public int? TotalReBuy { get; set; }

        public int? TotalAddOn { get; set; }

        [Required]
        [ForeignKey(nameof(PresenceState))]
        public string PresenceStateCode { get; set; }
        public PresenceState PresenceState { get; set; }

        public bool? WasFinalTable { get; set; }

        public bool? WasAddOn { get; set; }

        public int? TotalWinningsAmount { get; set; }

        [Required]
        [ForeignKey(nameof(PlayedTournament))]
        public int PlayedTournamentId { get; set; }
        public Tournament PlayedTournament { get; set; }
    }
}
