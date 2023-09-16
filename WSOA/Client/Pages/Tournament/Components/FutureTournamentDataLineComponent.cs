using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Tournament.Components
{
    public class FutureTournamentDataLineComponent : ComponentBase
    {
        [Parameter]
        public FutureTournamentDataViewModel Data { get; set; }

        [CascadingParameter(Name = "TournamentService")]
        [EditorRequired]
        public ITournamentService TournamentService { get; set; }

        [CascadingParameter(Name = "NavigationManager")]
        [EditorRequired]
        public NavigationManager NavigationManager { get; set; }

        public IEnumerable<PlayerDataViewModel> PresencePlayers { get; set; }

        public IEnumerable<PlayerDataViewModel> MaybePlayers { get; set; }

        public bool IsCollapse { get; set; }

        public SignUpTournamentFormViewModel SignUpForm { get; set; }

        protected override void OnInitialized()
        {
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
            SignUpForm.PresenceStateCode = presenceStateCode;

            SignUpTournamentCallResult result = await TournamentService.SignUpTournament(SignUpForm);
            if (!string.IsNullOrWhiteSpace(result.RedirectUrl))
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
                return;
            }

            if (result.Success)
            {
                Data.CurrentUserPresenceStateCode = result.PlayerSignedUp.PresenceStateCode;
                PlayerDataViewModel? currentPlayer = Data.PlayerDatasVM.SingleOrDefault(p => p.UserId == result.PlayerSignedUp.UserId);
                if (currentPlayer != null)
                {
                    currentPlayer.PresenceStateCode = result.PlayerSignedUp.PresenceStateCode;
                }
                else
                {
                    Data.PlayerDatasVM.Add(result.PlayerSignedUp);
                }
                LoadPlayersByPresenceStateCode();
            }
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
    }
}
