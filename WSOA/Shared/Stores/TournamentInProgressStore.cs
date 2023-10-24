using System.Timers;
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
    }
}
