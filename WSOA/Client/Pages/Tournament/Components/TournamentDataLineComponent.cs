using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Tournament.Components
{
    public class TournamentDataLineComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public TournamentViewModel Data { get; set; }

        [CascadingParameter(Name = "TournamentService")]
        [EditorRequired]
        public ITournamentService TournamentService { get; set; }

        [CascadingParameter(Name = "NavigationManager")]
        [EditorRequired]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        [EditorRequired]
        public TournamentActionMode Mode { get; set; }

        [Parameter]
        public EventCallback<int> OnDeletePlayableTournament { get; set; }

        public IEnumerable<PlayerViewModel> PresencePlayers { get; set; }

        public IEnumerable<PlayerViewModel> MaybePlayers { get; set; }

        public bool IsCollapse { get; set; }

        public bool IsProcess { get; set; }

        public SignUpTournamentFormViewModel SignUpForm { get; set; }

        protected override void OnInitialized()
        {
            IsProcess = false;
            LoadPlayersByPresenceStateCode();
            IsCollapse = true;
            SignUpForm = new SignUpTournamentFormViewModel
            {
                TournamentId = Data.TournamentId,
                PresenceStateCode = Data.CurrentUserPresenceStateCode
            };
        }

        public void SwitchCollapseState()
        {
            IsCollapse = !IsCollapse;
        }

        public async Task SignUpPresentTournament()
        {
            await SignUpTournament(PresenceStateResources.PRESENT_CODE);
        }

        public async Task SignUpMaybeTournament()
        {
            await SignUpTournament(PresenceStateResources.MAYBE_CODE);
        }

        public async Task SignUpAbsentTournament()
        {
            await SignUpTournament(PresenceStateResources.ABSENT_CODE);
        }

        private async Task SignUpTournament(string presenceStateCode)
        {
            IsProcess = true;

            SignUpForm.PresenceStateCode = presenceStateCode;

            APICallResult<PlayerViewModel> result = await TournamentService.SignUpTournament(SignUpForm);
            if (!string.IsNullOrWhiteSpace(result.RedirectUrl))
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
                return;
            }

            if (result.Success)
            {
                Data.CurrentUserPresenceStateCode = result.Data.PresenceStateCode;
                PlayerViewModel? currentPlayer = Data.PlayerDatasVM.SingleOrDefault(p => p.UserId == result.Data.UserId);
                if (currentPlayer != null)
                {
                    currentPlayer.PresenceStateCode = result.Data.PresenceStateCode;
                }
                else
                {
                    Data.PlayerDatasVM.Add(result.Data);
                }
                LoadPlayersByPresenceStateCode();
            }

            IsProcess = false;
        }

        public async Task UpdateSignUpTournament()
        {
            SignUpForm.PresenceStateCode = null;
        }

        private void LoadPlayersByPresenceStateCode()
        {
            PresencePlayers = Data.PlayerDatasVM.Where(p => p.PresenceStateCode == PresenceStateResources.PRESENT_CODE);
            MaybePlayers = Data.PlayerDatasVM.Where(p => p.PresenceStateCode == PresenceStateResources.MAYBE_CODE);
        }

        public async Task SelectPlayers()
        {
            NavigationManager.NavigateTo($"/tournament/select/players/{Data.TournamentId}");
        }

        public EventCallback DeletePlayableTournament => EventCallback.Factory.Create(this, async () =>
        {
            APICallResultBase result = await TournamentService.DeletePlayableTournament(Data.TournamentId);

            if (!result.Success)
            {
                string redirectUrl = !string.IsNullOrEmpty(result.RedirectUrl) ?
                                     result.RedirectUrl :
                                     string.Format("/main/error/{0}", "Une erreur est survenue pendant la suppression du tournoi. Contactez un administrateur");
                NavigationManager.NavigateTo(redirectUrl);
                return;
            }

            await OnDeletePlayableTournament.InvokeAsync(Data.TournamentId);
        });
    }
}
