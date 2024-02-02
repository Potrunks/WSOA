using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class TournamentToCancelDto
    {
        public TournamentToCancelDto() { }

        public Tournament TournamentToCancel { get; set; }

        public IEnumerable<Player> PlayersToUpdate { get; set; }

        public IEnumerable<Elimination> EliminationsToDelete { get; set; }

        public IEnumerable<BonusTournamentEarned> BonusToDelete { get; set; }
    }
}
