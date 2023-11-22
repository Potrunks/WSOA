using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Client.Shared.EventHandlers;
using WSOA.Client.Shared.Resources;
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

        public int TotalJackpot { get; set; }

        public IDictionary<int, int> WinnableMoneyByPosition { get; set; }

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

            InitializedData(tournamentInProgressDto);

            IsLoading = false;
        }

        private void InitializedData(TournamentInProgressDto tournamentInProgress)
        {
            PlayerPlayingsViewModel = tournamentInProgress.PlayerPlayings.Select(pla => new PlayerPlayingViewModel(pla));
            TotalJackpot = tournamentInProgress.TotalJackpot;
            WinnableMoneyByPosition = tournamentInProgress.WinnableMoneyByPosition;
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
                    OptionViewModel option = new OptionViewModel
                    {
                        Label = "Re-Buy ?",
                        Value = false
                    };

                    PopupEventHandler.Open(
                        eliminatorPlayers,
                        string.Format(TournamentMessageResources.WHO_ELIMINATE_PLAYER, StringFormatUtil.ToFullFirstNameAndFirstLetterLastName(eliminatedPlayer.FirstName, eliminatedPlayer.LastName)),
                        playerId.Value,
                        option,
                        EliminatePlayer()
                        );
                }
                catch (FunctionalException e)
                {
                    NavigationManager.NavigateTo(e.RedirectUrl);
                    return;
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

        private Action<int, int, bool> EliminatePlayer()
        {
            return async (int eliminatedPlayerId, int eliminatorPlayerId, bool hasReBuy) =>
            {
                try
                {
                    TournamentInProgressDto tournament = CheckTournamentAlwaysInProgress();

                    string? playerNameDefinitivelyEliminated = tournament.GetNameFirstPlayerDefinitivelyEliminated(new List<int> { eliminatedPlayerId, eliminatorPlayerId });
                    if (playerNameDefinitivelyEliminated != null)
                    {
                        PopupEventHandler.Open(
                                msg: TournamentMessageResources.PLAYER_ALREADY_ELIMINATED,
                                isError: false,
                                title: playerNameDefinitivelyEliminated,
                                onValid: null
                                );
                        return;
                    }

                    EliminationDto elimination = new EliminationDto
                    {
                        EliminatedPlayerId = eliminatedPlayerId,
                        EliminatorPlayerId = eliminatorPlayerId,
                        HasReBuy = hasReBuy,
                        WinnableMoneyByPosition = tournament.WinnableMoneyByPosition,
                        IsAddOn = tournament.IsAddOn,
                        IsFinalTable = tournament.IsFinalTable
                    };

                    APICallResult<EliminationResultDto> result = await TournamentService.EliminatePlayer(elimination);
                    if (!result.Success)
                    {
                        if (string.IsNullOrEmpty(result.RedirectUrl))
                        {
                            PopupEventHandler.Open(
                                msg: result.ErrorMessage!,
                                isError: true,
                                title: MainLabelResources.ERROR,
                                onValid: null
                                );
                            return;
                        }
                        else
                        {
                            NavigationManager.NavigateTo(result.RedirectUrl);
                            return;
                        }
                    }

                    tournament = TournamentInProgressStore.Update(elimination, result.Data);

                    InitializedData(tournament);

                    StateHasChanged();
                }
                catch (FunctionalException e)
                {
                    NavigationManager.NavigateTo(e.RedirectUrl!);
                    return;
                }
            };
        }
    }
}
