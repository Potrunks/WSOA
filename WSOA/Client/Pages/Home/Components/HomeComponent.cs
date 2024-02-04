using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Home.Components
{
    public class HomeComponent : SubSectionComponentBase
    {
        [Inject]
        public ITournamentService TournamentService { get; set; }

        public SeasonResultViewModel SeasonResultViewModel { get; set; }

        public IDictionary<int, PlayerPointViewModel> PlayerPointsOrdered { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            APICallResult<SeasonResultViewModel> result = await TournamentService.LoadSeasonResult(DateTime.UtcNow.Year);
            if (!result.Success)
            {
                string redirectUrl = !string.IsNullOrEmpty(result.RedirectUrl) ?
                                     result.RedirectUrl :
                                     string.Format("/main/error/{0}", "Une erreur est survenue pendant le chargement des resultats de la saison. Contactez un administrateur");
                NavigationManager.NavigateTo(redirectUrl);
                return;
            }

            SeasonResultViewModel = result.Data;

            List<PlayerPointViewModel> playerPointsOrdered = result.Data.PlayerPointList.OrderByDescending(pla => pla.Point).ToList();
            PlayerPointsOrdered = new Dictionary<int, PlayerPointViewModel>();
            for (int i = 0; i < playerPointsOrdered.Count; i++)
            {
                PlayerPointsOrdered.Add(i + 1, playerPointsOrdered[i]);
            }

            IsLoading = false;
        }
    }
}
