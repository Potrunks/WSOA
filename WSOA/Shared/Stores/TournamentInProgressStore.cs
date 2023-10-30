using System.Timers;
using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
using WSOA.Shared.ViewModel;
using Timer = System.Timers.Timer;

namespace WSOA.Shared.Stores
{
    public class TournamentInProgressStore
    {
        public TournamentInProgressStore()
        {

        }

        private Timer Timer { get; set; }

        private DateTime StartTime { get; set; }

        private DateTime StartDate { get; set; }

        private string Season { get; set; }

        private List<PlayerDto> PlayingPlayers { get; set; }

        private int TournamentId { get; set; }

        private IEnumerable<BonusTournament> WinnableBonus { get; set; }

        public void ActivateTimer(DateTime startTime, ElapsedEventHandler onElapsedTimer)
        {
            StartTime = startTime;
            Timer = new Timer(1000);
            Timer.Elapsed += onElapsedTimer;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }

        public DateTime? GetTimer(ElapsedEventHandler onElapsedTimer)
        {
            if (Timer != null)
            {
                Timer.Elapsed += onElapsedTimer;
                return StartTime;
            }

            return null;
        }

        public void Store(TournamentInProgressDto tournamentInProgress)
        {
            Tournament tournament = tournamentInProgress.TournamentDto.Tournament;

            StartDate = tournament.StartDate;
            Season = tournament.Season;
            TournamentId = tournament.Id;
            PlayingPlayers = tournamentInProgress.TournamentDto.Players.ToList();
            WinnableBonus = tournamentInProgress.WinnableBonus;
        }

        public TournamentInProgressViewModel GetViewModel()
        {
            return new TournamentInProgressViewModel
            {
                TournamentId = TournamentId,
                Season = Season,
                StartDate = StartDate,
                PlayingPlayers = PlayingPlayers.Select(pla => new PlayerPlayingViewModel(pla)),
                WinnableBonus = WinnableBonus
            };
        }
    }
}
