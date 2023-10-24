using Microsoft.AspNetCore.Components;
using System.Timers;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Stores;

namespace WSOA.Client.Pages.Tournament.Components
{
    public class TournamentInProgressComponent : SubSectionComponentBase
    {
        [Inject]
        public TournamentInProgressStore TournamentTimerStore { get; set; }

        public string CurrentTime { get; set; }

        public DateTime? StartTime { get; set; }

        public EventCallback OnActivateTimer => EventCallback.Factory.Create(this, () => ActivateTimer());

        protected override async Task OnInitializedAsync()
        {
            CurrentTime = "NOT ACTIVATE";
            StartTime = TournamentTimerStore.GetTimer(RefreshTimerDisplay);
        }

        private void ActivateTimer()
        {
            StartTime = DateTime.Now;
            TournamentTimerStore.ActivateTimer(StartTime.Value, RefreshTimerDisplay);
        }

        public void RefreshTimerDisplay(object source, ElapsedEventArgs args)
        {
            CurrentTime = args.SignalTime.Subtract(StartTime.Value).ToString();
            StateHasChanged();
        }
    }
}
