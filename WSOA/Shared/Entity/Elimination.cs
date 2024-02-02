using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSOA.Shared.Entity
{
    public class Elimination
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(PlayerEliminator))]
        public int PlayerEliminatorId { get; set; }
        public Player PlayerEliminator { get; set; }

        [Required]
        [ForeignKey(nameof(PlayerVictim))]
        public int PlayerVictimId { get; set; }
        public Player PlayerVictim { get; set; }

        [Required]
        public bool IsDefinitive { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
