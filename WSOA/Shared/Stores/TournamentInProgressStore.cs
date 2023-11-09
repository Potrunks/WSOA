using System.Timers;
using WSOA.Shared.Dtos;
using WSOA.Shared.Exceptions;
using WSOA.Shared.Resources;
using Timer = System.Timers.Timer;

namespace WSOA.Shared.Stores
{
    public class TournamentInProgressStore
    {
        public TournamentInProgressStore()
        {

        }

        private Timer? Timer { get; set; }

        private DateTime? StartTime { get; set; }

        private DateTime? StartDate { get; set; }

        private TournamentInProgressDto? Data { get; set; }

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

        public TournamentInProgressDto? GetStoredData()
        {
            return Data;
        }

        public void Store(TournamentInProgressDto tournamentInProgress)
        {
            if (Data != null)
            {
                string errorMsg = TournamentErrorMessageResources.TOURNAMENT_IN_PROGRESS_ALREADY_STORED;
                throw new FunctionalException(errorMsg, string.Format(RouteResources.MAIN_ERROR, errorMsg));
            }

            Data = tournamentInProgress;
        }
    }
}
