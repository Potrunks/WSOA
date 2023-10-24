using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class TournamentInProgressDto
    {
        public TournamentInProgressDto()
        {

        }

        public TournamentDto TournamentDto { get; set; }

        public IEnumerable<BonusTournament> WinnableBonus { get; set; }
    }
}
