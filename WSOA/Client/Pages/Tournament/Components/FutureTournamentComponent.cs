using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Tournament.Components
{
    public class FutureTournamentComponent : SubSectionComponentBase
    {
        [Inject]
        public ITournamentService TournamentService { get; set; }

        public List<TournamentViewModel> FutureTournamentDatasVM { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            APICallResult<TournamentsViewModel> result = await TournamentService.LoadFutureTournamentDatas(SubSectionId);
            if (!string.IsNullOrWhiteSpace(result.RedirectUrl))
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
                return;
            }
            FutureTournamentDatasVM = result.Data.TournamentsVM;
            Description = result.Data.Description;

            IsLoading = false;
        }
    }
}
