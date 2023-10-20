using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Dtos;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Tournament.Components
{
    public class SelectPresentPlayersComponent : WSOAComponentBase
    {
        [Inject]
        public ITournamentService TournamentService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public int TournamentId { get; set; }

        public List<PlayerViewModel> SelectedPlayers { get; set; }

        public List<PlayerViewModel> AvailablePlayers { get; set; }

        public EventCallback<PlayerViewModel> OnPaymentClick => EventCallback.Factory.Create(this, (PlayerViewModel player) => SwitchPaymentStatus(player));

        public EventCallback<PlayerViewModel> OnTrashPlayerClick => EventCallback.Factory.Create(this, (PlayerViewModel player) => UnSelectPlayer(player));

        public EventCallback OnAddPlayersClick => EventCallback.Factory.Create(this, () => ShowAvailablePlayersPopup());

        public EventCallback OnAddPlayersPopupExit => EventCallback.Factory.Create(this, () => HideAvailablePlayersPopup());

        public EventCallback<PlayerViewModel> OnPreSelectPlayerClick => EventCallback.Factory.Create(this, (PlayerViewModel player) => SwitchPreSelectStatus(player));

        public EventCallback OnValidPreSelectedPlayers => EventCallback.Factory.Create(this, () => SelectPlayers());

        public EventCallback OnValidSelectedPlayers => EventCallback.Factory.Create(this, () => SaveTournamentPreparation());

        public EventCallback OnConfirmPlayersPayment => EventCallback.Factory.Create(this, () => ConfirmPlayersPayment());

        public EventCallback OnPlayersPaymentMissingPopupExit => EventCallback.Factory.Create(this, () => ClosePlayersPaymentMissingPopup());

        public bool IsDisplayingAddPlayersPopup { get; set; }

        public bool IsDisplayingPaymentMissingPopup { get; set; }

        public bool IsDisplayingPopup { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            APICallResult<PlayerSelectionViewModel> result = await TournamentService.LoadPlayersForPlayingTournament(TournamentId);
            if (!string.IsNullOrWhiteSpace(result.RedirectUrl))
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
                return;
            }

            SelectedPlayers = result.Data.PresentPlayers.OrderBy(pla => pla.LastName).ToList();
            AvailablePlayers = result.Data.AvailablePlayers.OrderBy(pla => pla.LastName).ToList();

            IsDisplayingAddPlayersPopup = false;
            IsDisplayingPaymentMissingPopup = false;
            IsDisplayingPopup = false;

            IsLoading = false;
        }

        private void SwitchPaymentStatus(PlayerViewModel player)
        {
            player.HasPaid = !player.HasPaid;
        }

        private void UnSelectPlayer(PlayerViewModel player)
        {
            player.IsPreSelected = false;
            SelectedPlayers.Remove(player);
            AvailablePlayers.Add(player);

            OrderPlayersListsByLastName();
        }

        private async void ShowAvailablePlayersPopup()
        {
            IsDisplayingAddPlayersPopup = true;
            IsDisplayingPopup = true;
            await JSRuntime.InvokeVoidAsync("switchMainNavMenuDisplayStatus");
        }

        private async void HideAvailablePlayersPopup()
        {
            foreach (PlayerViewModel player in AvailablePlayers)
            {
                player.IsPreSelected = false;
            }

            IsDisplayingAddPlayersPopup = false;
            IsDisplayingPopup = false;

            await JSRuntime.InvokeVoidAsync("switchMainNavMenuDisplayStatus");
        }

        private void SwitchPreSelectStatus(PlayerViewModel player)
        {
            player.IsPreSelected = !player.IsPreSelected;
        }

        private void SelectPlayers()
        {
            List<PlayerViewModel> selectedPlayers = AvailablePlayers.Where(pla => pla.IsPreSelected).ToList();
            foreach (PlayerViewModel player in selectedPlayers)
            {
                player.HasPaid = false;
                AvailablePlayers.Remove(player);
            }
            SelectedPlayers.AddRange(selectedPlayers);

            OrderPlayersListsByLastName();
            HideAvailablePlayersPopup();
        }

        private void OrderPlayersListsByLastName()
        {
            SelectedPlayers = SelectedPlayers.OrderBy(pla => pla.LastName).ToList();
            AvailablePlayers = AvailablePlayers.OrderBy(pla => pla.LastName).ToList();
        }

        private async void SaveTournamentPreparation()
        {
            if (SelectedPlayers.Any(pla => !pla.HasPaid))
            {
                IsDisplayingPaymentMissingPopup = true;
                IsDisplayingPopup = true;
                await JSRuntime.InvokeVoidAsync("switchMainNavMenuDisplayStatus");
            }
            else
            {
                TournamentPreparedDto tournamentPrepared = new TournamentPreparedDto
                {
                    TournamentId = TournamentId,
                    SelectedUserIds = SelectedPlayers.Select(pla => pla.UserId)
                };

                APICallResultBase result = await TournamentService.PlayTournamentPrepared(tournamentPrepared);

                NavigationManager.NavigateTo(result.RedirectUrl);
            }
        }

        private void ConfirmPlayersPayment()
        {
            foreach (PlayerViewModel player in SelectedPlayers)
            {
                player.HasPaid = true;
            }

            IsDisplayingPaymentMissingPopup = false;
            IsDisplayingPopup = false;

            SaveTournamentPreparation();
        }

        private async void ClosePlayersPaymentMissingPopup()
        {
            IsDisplayingPaymentMissingPopup = false;
            IsDisplayingPopup = false;
            await JSRuntime.InvokeVoidAsync("switchMainNavMenuDisplayStatus");
        }
    }
}
