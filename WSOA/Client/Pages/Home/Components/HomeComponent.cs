using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Dtos;
using WSOA.Shared.Result;

namespace WSOA.Client.Pages.Home.Components
{
    public class HomeComponent : SubSectionComponentBase
    {
        [Inject]
        public ITournamentService TournamentService { get; set; }

        public SeasonResultDto SeasonResult { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            APICallResult<SeasonResultDto> result = await TournamentService.LoadSeasonResult(DateTime.UtcNow.Year);
            if (!result.Success)
            {
                string redirectUrl = !string.IsNullOrEmpty(result.RedirectUrl) ?
                                     result.RedirectUrl :
                                     string.Format("/main/error/{0}", "Une erreur est survenue pendant le chargement des resultats de la saison. Contactez un administrateur");
                NavigationManager.NavigateTo(redirectUrl);
                return;
            }

            SeasonResult = result.Data;

            IsLoading = false;
        }
    }
}
