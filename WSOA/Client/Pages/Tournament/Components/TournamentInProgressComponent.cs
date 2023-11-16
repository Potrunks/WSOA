using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Client.Shared.EventHandlers;
using WSOA.Client.Shared.Stores;
using WSOA.Shared.Dtos;
using WSOA.Shared.Exceptions;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;
using WSOA.Shared.Utils;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Tournament.Components
{
    public class TournamentInProgressComponent : SubSectionComponentBase
    {
        [Inject]
        public TournamentInProgressStore TournamentInProgressStore { get; set; }

        [Inject]
        public ITournamentService TournamentService { get; set; }

        [CascadingParameter(Name = "PopupEventHandler")]
        [EditorRequired]
        public PopupEventHandler PopupEventHandler { get; set; }

        public IEnumerable<PlayerPlayingViewModel> PlayerPlayingsViewModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            TournamentInProgressDto? tournamentInProgressDto = TournamentInProgressStore.GetData();
            if (tournamentInProgressDto == null)
            {
                APICallResult<TournamentInProgressDto> result = await TournamentService.LoadTournamentInProgress(SubSectionId);
                if (!result.Success)
                {
                    NavigationManager.NavigateTo(result.RedirectUrl);
                    return;
                }

                tournamentInProgressDto = TournamentInProgressStore.SetData(result.Data);
            }

            PlayerPlayingsViewModel = tournamentInProgressDto.PlayerPlayings.Select(pla => new PlayerPlayingViewModel(pla));

            IsLoading = false;
        }

        public EventCallback<PlayerPlayingViewModel> OpenPlayerActionsPopup => EventCallback.Factory.Create(this, (PlayerPlayingViewModel player) =>
        {
            PopupEventHandler.Open(GeneratePlayerActions(player), StringFormatUtil.ToFullFirstNameAndFirstLetterLastName(player.FirstName, player.LastName), player.Id);
        });

        private Action<int?> OpenEliminationPopup()
        {
            return (int? playerId) =>
            {
                try
                {
                    PopupEventHandler.Close();

                    TournamentInProgressDto tournamentInProgressDto = CheckTournamentAlwaysInProgress();

                    PlayerPlayingDto eliminatedPlayer = tournamentInProgressDto.PlayerPlayings.Single(pla => pla.Id == playerId);
                    if (eliminatedPlayer.IsEliminated)
                    {
                        PopupEventHandler.Open(
                            msg: TournamentMessageResources.PLAYER_ALREADY_ELIMINATED,
                            isError: false,
                            title: StringFormatUtil.ToFullFirstNameAndFirstLetterLastName(eliminatedPlayer.FirstName, eliminatedPlayer.LastName),
                            onValid: null
                            );
                    }

                    IEnumerable<ItemSelectableViewModel> eliminatorPlayers = PlayerPlayingsViewModel.Where(pla => !pla.IsEliminated && pla.Id != playerId)
                                                                                                    .Select(pla => new ItemSelectableViewModel(pla));

                    PopupEventHandler.Open(
                        selectableItems: eliminatorPlayers,
                        title: string.Format(TournamentMessageResources.WHO_ELIMINATE_PLAYER, StringFormatUtil.ToFullFirstNameAndFirstLetterLastName(eliminatedPlayer.FirstName, eliminatedPlayer.LastName)),
                        playerId: playerId.Value
                        );
                }
                catch (FunctionalException e)
                {
                    NavigationManager.NavigateTo(e.RedirectUrl);
                }
            };
        }

        public Action<int?> OpenWinBonusPopup()
        {
            return (int? playerId) =>
            {
                // Ouvrir la popup des bonus gagné
                PopupEventHandler.Close();
            };
        }

        private List<PopupButtonViewModel> GeneratePlayerActions(PlayerPlayingViewModel player)
        {
            List<PopupButtonViewModel> actions = new List<PopupButtonViewModel>();

            if (!player.IsEliminated)
            {
                actions.Add(new PopupButtonViewModel
                {
                    Action = OpenEliminationPopup(),
                    Label = PopupPlayerActionResources.ELIMINATION
                });
                actions.Add(new PopupButtonViewModel
                {
                    Action = OpenWinBonusPopup(),
                    Label = PopupPlayerActionResources.WIN_BONUS
                });
            }

            return actions;
        }

        private TournamentInProgressDto CheckTournamentAlwaysInProgress()
        {
            TournamentInProgressDto? tournamentInProgressDto = TournamentInProgressStore.GetData();
            if (tournamentInProgressDto == null)
            {
                string errorMsg = TournamentMessageResources.NO_TOURNAMENT_IN_PROGRESS;
                string redirectUrl = string.Format(RouteResources.MAIN_ERROR, errorMsg);
                throw new FunctionalException(errorMsg, redirectUrl);
            }
            return tournamentInProgressDto;
        }
    }
}
