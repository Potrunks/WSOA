using System.ComponentModel;
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

        [ForeignKey(nameof(EliminatorPlayer))]
        public int? EliminatorPlayerId { get; set; }
        public Player EliminatorPlayer { get; set; }

        public int? TotalReBuy { get; set; }

        public int? TotalAddOn { get; set; }

        [Required]
        [ForeignKey(nameof(PresenceState))]
        public string PresenceStateCode { get; set; }
        public PresenceState PresenceState { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool WasPresent { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool WasFinalTable { get; set; }

        public int? TotalWinningsAmount { get; set; }

        [Required]
        [ForeignKey(nameof(PlayedTournament))]
        public int PlayedTournamentId { get; set; }
        public Tournament PlayedTournament { get; set; }
    }
}
