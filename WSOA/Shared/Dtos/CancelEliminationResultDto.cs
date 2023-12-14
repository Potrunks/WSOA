using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class CancelEliminationResultDto
    {
        public CancelEliminationResultDto() { }

        public Player PlayerEliminated { get; set; }

        public Player PlayerEliminator { get; set; }

        public Elimination EliminationCanceled { get; set; }

        public IEnumerable<BonusTournament>? BonusTournamentsLostByEliminator { get; set; }
    }
}
