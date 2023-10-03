using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;

namespace WSOA.Client.Pages.Tournament.Components
{
    public class PlayableTournamentsComponent : SubSectionComponentBase
    {
        [Inject]
        public ITournamentService TournamentService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            // TODO : Charger les données

            IsLoading = false;
        }
    }
}
