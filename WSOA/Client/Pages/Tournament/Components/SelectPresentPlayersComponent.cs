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

        public EventCallback<PlayerViewModel> SwitchPaymentStatus => EventCallback.Factory.Create(this, (PlayerViewModel player) =>
        {
            player.HasPaid = !player.HasPaid;
        });

        public EventCallback<PlayerViewModel> UnSelectPlayer => EventCallback.Factory.Create(this, (PlayerViewModel player) =>
        {
            player.IsPreSelected = false;
            SelectedPlayers.Remove(player);
            AvailablePlayers.Add(player);
            SelectedPlayers = SelectedPlayers.OrderBy(pla => pla.LastName).ToList();
            AvailablePlayers = AvailablePlayers.OrderBy(pla => pla.LastName).ToList();
        });

        public EventCallback OnValidSelectedPlayers => EventCallback.Factory.Create(this, () => SaveTournamentPreparation());

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

            IsLoading = false;
        }

        public EventCallback OpenSelectablePlayersPopup => EventCallback.Factory.Create(this, () =>
        {
            IEnumerable<ItemSelectableViewModel> selectablePlayers = AvailablePlayers.Select(pla => new ItemSelectableViewModel(pla));
            PopupEventHandler.Open(selectablePlayers, MainLabelResources.AVAILABLE_PLAYERS, SelectPlayers);
        });

        public EventCallback<IEnumerable<int>> SelectPlayers => EventCallback.Factory.Create(this, (IEnumerable<int> selectedItemIds) =>
        {
            List<PlayerViewModel> selectedPlayers = AvailablePlayers.Where(pla => selectedItemIds.Contains(pla.UserId)).ToList();
            foreach (PlayerViewModel player in selectedPlayers)
            {
                player.HasPaid = false;
                AvailablePlayers.Remove(player);
            }
            SelectedPlayers.AddRange(selectedPlayers);
            SelectedPlayers = SelectedPlayers.OrderBy(pla => pla.LastName).ToList();
            AvailablePlayers = AvailablePlayers.OrderBy(pla => pla.LastName).ToList();
        });

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
    }
}
