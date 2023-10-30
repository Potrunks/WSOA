using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Client.Shared.EventHandlers;
using WSOA.Client.Shared.Resources;
using WSOA.Shared.Dtos;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;
using WSOA.Shared.Stores;
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

        [Inject]
        public TournamentInProgressStore TournamentInProgressStore { get; set; }

        [CascadingParameter(Name = "PopupEventHandler")]
        [EditorRequired]
        public PopupEventHandler PopupEventHandler { get; set; }

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

        public bool IsDisplayingAddPlayersPopup { get; set; }

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

        private void ShowAvailablePlayersPopup()
        {
            IsDisplayingAddPlayersPopup = true;
            OnOpenPopup();
        }

        private void HideAvailablePlayersPopup()
        {
            foreach (PlayerViewModel player in AvailablePlayers)
            {
                player.IsPreSelected = false;
            }

            IsDisplayingAddPlayersPopup = false;
            OnClosePopup();
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
            if (!SelectedPlayers.Any())
            {
                PopupEventHandler.Open(TournamentErrorMessageResources.TOURNAMENT_NO_PLAYER_SELECTED, true, MainLabelResources.ERROR, null);
                return;
            }

            IEnumerable<PlayerViewModel> noPaymentPlayers = SelectedPlayers.Where(pla => !pla.HasPaid);
            if (noPaymentPlayers.Any())
            {
                IEnumerable<MessageViewModel> messages = noPaymentPlayers.Select(pla => new MessageViewModel(pla));
                PopupEventHandler.Open(messages, MainLabelResources.PLAYERS_PAYMENT_MISSING, ConfirmPlayersPayment());
                return;
            }

            IsLoading = true;

            TournamentPreparedDto tournamentPrepared = new TournamentPreparedDto
            {
                TournamentId = TournamentId,
                SelectedUserIds = SelectedPlayers.Select(pla => pla.UserId)
            };

            APICallResult<TournamentInProgressDto> result = await TournamentService.PlayTournamentPrepared(tournamentPrepared);

            if (result.Success)
            {
                TournamentInProgressStore.Store(result.Data);
            }

            NavigationManager.NavigateTo(result.RedirectUrl);

            IsLoading = false;
        }

        public Action ConfirmPlayersPayment()
        {
            return () =>
            {
                foreach (PlayerViewModel player in SelectedPlayers)
                {
                    player.HasPaid = true;
                }

                SaveTournamentPreparation();
            };
        }

        private async void OnClosePopup()
        {
            IsDisplayingPopup = false;
            await JSRuntime.InvokeVoidAsync("switchMainNavMenuDisplayStatus");
        }

        private async void OnOpenPopup()
        {
            IsDisplayingPopup = true;
            await JSRuntime.InvokeVoidAsync("switchMainNavMenuDisplayStatus");
        }
    }
}
