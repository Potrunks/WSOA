using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Tournament.Components
{
    public class CreateTournamentComponent : SubSectionComponentBase
    {
        [Inject]
        public ITournamentService TournamentService { get; set; }

        public TournamentCreationFormViewModel FormVM { get; set; }

        public TournamentCreationDataViewModel DataVM { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            APICallResult<TournamentCreationDataViewModel> result = await TournamentService.LoadTournamentCreationDatas(SubSectionId);
            if (!string.IsNullOrWhiteSpace(result.RedirectUrl))
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
                return;
            }
            DataVM = result.Data;

            FormVM = new TournamentCreationFormViewModel(NavigationManager.BaseUri, SubSectionId, DataVM);

            IsLoading = false;
        }

        public Func<Task<APICallResultBase>> CreateTournament()
        {
            return () => TournamentService.CreateTournament(FormVM);
        }
    }
}
