using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class PlayerDto
    {
        public PlayerDto()
        {

        }

        public Player Player { get; set; }

        public User User { get; set; }

        public IEnumerable<Elimination> EliminationsAsEliminator { get; set; }

        public IEnumerable<Elimination> EliminationsAsVictim { get; set; }

        public IEnumerable<BonusTournamentEarned> BonusTournamentEarneds { get; set; }

        public Tournament Tournament { get; set; }
    }
}
