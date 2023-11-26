using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class BonusTournamentEarnedCreationDto
    {
        public BonusTournamentEarnedCreationDto()
        {

        }

        public int ConcernedPlayerId { get; set; }

        public BonusTournament EarnedBonus { get; set; }
    }
}
