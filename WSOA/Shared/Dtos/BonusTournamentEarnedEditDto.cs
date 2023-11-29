using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class BonusTournamentEarnedEditDto
    {
        public BonusTournamentEarnedEditDto()
        {

        }

        public int ConcernedPlayerId { get; set; }

        public BonusTournament ConcernedBonusTournament { get; set; }
    }
}
