﻿using Microsoft.AspNetCore.Components;
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

                    TournamentInProgressDto tournamentInProgressDto = TournamentInProgressStore.CheckTournamentAlwaysInProgress();

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

                    IEnumerable<IdSelectableViewModel> eliminatorPlayers = PlayerPlayingsViewModel.Where(pla => !pla.IsEliminated && pla.Id != playerId)
                                                                                                    .Select(pla => new IdSelectableViewModel(pla));
                    OptionViewModel option = new OptionViewModel
                    {
                        Label = TournamentMessageResources.WANT_REBUY,
                        Value = false
                    };

                    PopupEventHandler.Open(
                        eliminatorPlayers,
                        string.Format(TournamentMessageResources.WHO_ELIMINATE_PLAYER, StringFormatUtil.ToFullFirstNameAndFirstLetterLastName(eliminatedPlayer.FirstName, eliminatedPlayer.LastName)),
                        playerId!.Value,
                        option,
                        EliminatePlayer()
                        );
                }
                catch (FunctionalException e)
                {
                    NavigationManager.NavigateTo(e.RedirectUrl!);
                    return;
                }
            };
        }

        private Action<int?> OpenWinBonusPopup()
        {
            return (int? playerId) =>
            {
                try
                {
                    TournamentInProgressDto tournamentInProgressDto = TournamentInProgressStore.CheckTournamentAlwaysInProgress();
                    PlayerPlayingDto player = tournamentInProgressDto.PlayerPlayings.Single(pla => pla.Id == playerId);
                    IEnumerable<CodeSelectableViewModel> items = tournamentInProgressDto.WinnableBonus.Where(bonus => !new string[] { BonusTournamentResources.FIRST_RANKED_KILLED, BonusTournamentResources.PREVIOUS_WINNER_KILLED }.Contains(bonus.Code))
                                                                                                      .Select(bonus => new CodeSelectableViewModel(bonus));
                    PopupEventHandler.Open
                    (
                        items,
                        string.Format(TournamentMessageResources.WHICH_BONUS_HAS_WON, StringFormatUtil.ToFullFirstNameAndFirstLetterLastName(player.FirstName, player.LastName)),
                        playerId!.Value,
                        EditBonus(false)
                    );
                }
                catch (FunctionalException e)
                {
                    NavigationManager.NavigateTo(e.RedirectUrl!);
                    return;
                }
            };
        }

        private Action<int?> OpenDeleteBonusPopup()
        {
            return (int? playerId) =>
            {
                try
                {
                    TournamentInProgressDto tournamentInProgressDto = TournamentInProgressStore.CheckTournamentAlwaysInProgress();
                    PlayerPlayingDto player = tournamentInProgressDto.PlayerPlayings.Single(pla => pla.Id == playerId);
                    IEnumerable<CodeSelectableViewModel> items = player.BonusTournamentEarnedsByBonusTournamentCode.Select(bonus => new CodeSelectableViewModel(bonus.Value));
                    PopupEventHandler.Open
                    (
                        items,
                        string.Format(TournamentMessageResources.WHICH_BONUS_TO_DELETE, StringFormatUtil.ToFullFirstNameAndFirstLetterLastName(player.FirstName, player.LastName)),
                        playerId!.Value,
                        EditBonus(true)
                    );
                }
                catch (FunctionalException e)
                {
                    NavigationManager.NavigateTo(e.RedirectUrl!);
                    return;
                }
            };
        }

        private Action<string, int> EditBonus(bool isDelete)
        {
            return async (string selectedCode, int concernedId) =>
            {
                try
                {
                    TournamentInProgressDto tournament = TournamentInProgressStore.CheckTournamentAlwaysInProgress();
                    BonusTournamentEarnedEditDto dto = new BonusTournamentEarnedEditDto
                    {
                        ConcernedPlayerId = concernedId,
                        ConcernedBonusTournament = tournament.WinnableBonus.Single(bonus => bonus.Code == selectedCode)
                    };

                    APICallResult<BonusTournamentEarnedEditResultDto> result = isDelete ? await TournamentService.DeleteBonusTournamentEarned(dto) : await TournamentService.SaveBonusTournamentEarned(dto);
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

                    tournament = TournamentInProgressStore.Update(result.Data);
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
            }

            if (player.BonusTournamentEarnedByBonusTournamentCode.Any())
            {
                actions.Add(new PopupButtonViewModel
                {
                    Action = OpenDeleteBonusPopup(),
                    Label = PopupPlayerActionResources.DELETE_BONUS
                });
            }

            if (player.TotalRebuy > 0 || player.IsEliminated)
            {
                actions.Add(new PopupButtonViewModel
                {
                    Action = CancelLastPlayerEliminationByPlayerId(),
                    Label = PopupPlayerActionResources.CANCEL_LAST_ELIMINATION
                });
            }

            actions.Add(new PopupButtonViewModel
            {
                Action = OpenWinBonusPopup(),
                Label = PopupPlayerActionResources.WIN_BONUS
            });

            return actions;
        }

        private Action<int, int, bool> EliminatePlayer()
        {
            return async (int eliminatedPlayerId, int eliminatorPlayerId, bool hasReBuy) =>
            {
                try
                {
                    TournamentInProgressDto tournament = TournamentInProgressStore.CheckTournamentAlwaysInProgress();

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

                    EliminationCreationDto elimination = new EliminationCreationDto
                    {
                        EliminatedPlayerId = eliminatedPlayerId,
                        EliminatorPlayerId = eliminatorPlayerId,
                        HasReBuy = hasReBuy,
                        WinnableMoneyByPosition = tournament.WinnableMoneyByPosition,
                        IsAddOn = tournament.IsAddOn,
                        IsFinalTable = tournament.IsFinalTable
                    };

                    APICallResult<EliminationCreationResultDto> result = await TournamentService.EliminatePlayer(elimination);
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

        private Action<int?> CancelLastPlayerEliminationByPlayerId()
        {
            return async (int? playerId) =>
            {
                try
                {
                    APICallResult<CancelEliminationResultDto> result = await TournamentService.CancelLastPlayerEliminationByPlayerId(playerId!.Value);

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

                    TournamentInProgressDto tournamentInProgress = TournamentInProgressStore.Update(result.Data);

                    InitializedData(tournamentInProgress);

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
