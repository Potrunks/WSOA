using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSOA.Shared.Entity
{
    public class JackpotDistribution
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Tournament))]
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public int PlayerPosition { get; set; }
    }
}
