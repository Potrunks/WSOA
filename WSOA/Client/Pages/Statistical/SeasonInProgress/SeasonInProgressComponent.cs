using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Dtos;
using WSOA.Shared.Result;

namespace WSOA.Client.Pages.Statistical.SeasonInProgress
{
    public class SeasonInProgressComponent : SubSectionComponentBase
    {
        [Inject]
        public ITournamentService TournamentService { get; set; }

        public SeasonMyResultDto SeasonMyResultDto { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            APICallResult<SeasonMyResultDto> result = await TournamentService.LoadMySeasonInProgressResultDto();
            if (!result.Success)
            {
                string redirectUrl = !string.IsNullOrEmpty(result.RedirectUrl) ?
                                     result.RedirectUrl :
                                     string.Format("/main/error/{0}", "Une erreur est survenue pendant le chargement des resultats de la saison. Contactez un administrateur");
                NavigationManager.NavigateTo(redirectUrl);
                return;
            }

            SeasonMyResultDto = result.Data;

            IsLoading = false;
        }
    }
}
