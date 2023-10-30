using WSOA.Shared.Entity;

namespace WSOA.Shared.ViewModel
{
    public class TournamentInProgressViewModel
    {
        public TournamentInProgressViewModel()
        {

        }

        public int TournamentId { get; set; }

        public DateTime StartDate { get; set; }

        public string Season { get; set; }

        public IEnumerable<BonusTournament> WinnableBonus { get; set; }

        public IEnumerable<PlayerPlayingViewModel> PlayingPlayers { get; set; }
    }
}
