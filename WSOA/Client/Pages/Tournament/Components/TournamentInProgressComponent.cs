﻿using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Client.Shared.EventHandlers;
using WSOA.Client.Shared.Resources;
using WSOA.Client.Shared.Stores;
using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
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

        public TournamentInProgressViewModel TournamentInProgressViewModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            TournamentInProgressDto? tournamentInProgressDto = TournamentInProgressStore.GetData();
            if (tournamentInProgressDto == null)
            {
                APICallResult<TournamentInProgressDto> result = await TournamentService.LoadTournamentInProgress(SubSectionId);
                if (!result.Success)
                {
                    NavigationManager.NavigateTo(result.RedirectUrl!);
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
            TournamentInProgressViewModel = new TournamentInProgressViewModel(tournamentInProgress);
        }

        public EventCallback<PlayerPlayingViewModel> OpenPlayerActionsPopup => EventCallback.Factory.Create(this, (PlayerPlayingViewModel player) =>
        {
            PopupEventHandler.Open(GeneratePlayerActions(player), StringFormatUtil.ToFormatFullName(player.FirstName, player.LastName), player.Id);
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
                            title: StringFormatUtil.ToFormatFullName(eliminatedPlayer.FirstName, eliminatedPlayer.LastName),
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
                        string.Format(TournamentMessageResources.WHO_ELIMINATE_PLAYER, StringFormatUtil.ToFormatFullName(eliminatedPlayer.FirstName, eliminatedPlayer.LastName)),
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
                        string.Format(TournamentMessageResources.WHICH_BONUS_HAS_WON, StringFormatUtil.ToFormatFullName(player.FirstName, player.LastName)),
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
                        string.Format(TournamentMessageResources.WHICH_BONUS_TO_DELETE, StringFormatUtil.ToFormatFullName(player.FirstName, player.LastName)),
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

            try
            {
                if (!player.IsEliminated)
                {
                    actions.Add(new PopupButtonViewModel
                    {
                        Action = OpenEliminationPopup(),
                        Label = PopupPlayerActionResources.ELIMINATION
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

                if (player.BonusTournamentEarnedByBonusTournamentCode.Any())
                {
                    actions.Add(new PopupButtonViewModel
                    {
                        Action = OpenDeleteBonusPopup(),
                        Label = PopupPlayerActionResources.DELETE_BONUS
                    });
                }

                if (TournamentInProgressStore.IsAddOn())
                {
                    actions.Add(new PopupButtonViewModel
                    {
                        Action = OpenEditAddonPopup(),
                        Label = PopupPlayerActionResources.EDIT_ADDON
                    });
                }

                if (TournamentInProgressStore.CanRemovePlayer(player.Id))
                {
                    actions.Add(new PopupButtonViewModel
                    {
                        Action = RemovePlayerNeverComeIntoTournamentInProgress(),
                        Label = PopupPlayerActionResources.REMOVE_PLAYER
                    });
                }
            }
            catch (FunctionalException e)
            {
                NavigationManager.NavigateTo(e.RedirectUrl!);
            }

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
                    TournamentInProgressDto tournamentInProgress = TournamentInProgressStore.CheckTournamentAlwaysInProgress();

                    EliminationEditionDto eliminationEditionDto = new EliminationEditionDto
                    {
                        EliminatedPlayerId = playerId!.Value,
                        IsAddOn = tournamentInProgress.IsAddOn,
                        IsFinalTable = tournamentInProgress.IsFinalTable,
                        TournamentId = tournamentInProgress.Id,
                        BuyIn = tournamentInProgress.BuyIn
                    };
                    APICallResult<CancelEliminationResultDto> result = await TournamentService.CancelLastPlayerEliminationByPlayerId(eliminationEditionDto);

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

                    tournamentInProgress = TournamentInProgressStore.Update(result.Data);

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

        private Action<int?> RemovePlayerNeverComeIntoTournamentInProgress()
        {
            return async (int? playerId) =>
            {
                try
                {
                    TournamentInProgressStore.CheckTournamentAlwaysInProgress();

                    APICallResult<IEnumerable<JackpotDistribution>> result = await TournamentService.RemovePlayerNeverComeIntoTournamentInProgress(playerId!.Value);

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

                    TournamentInProgressDto tournamentInProgress = TournamentInProgressStore.RemovePlayer(playerId!.Value, result.Data);

                    InitializedData(tournamentInProgress);

                    StateHasChanged();

                    if (!string.IsNullOrEmpty(result.WarningMessage))
                    {
                        PopupEventHandler.Open(
                                msg: result.WarningMessage,
                                isError: false,
                                title: MainLabelResources.WARNING,
                                onValid: null
                                );
                    }
                }
                catch (FunctionalException e)
                {
                    NavigationManager.NavigateTo(e.RedirectUrl!);
                    return;
                }
            };
        }

        private Action<int?> OpenEditAddonPopup()
        {
            return (int? playerId) =>
            {
                try
                {
                    TournamentInProgressDto tournamentInProgressDto = TournamentInProgressStore.CheckTournamentAlwaysInProgress();
                    PlayerPlayingDto player = tournamentInProgressDto.PlayerPlayings.Single(pla => pla.Id == playerId);
                    PopupEventHandler.OpenInputNumberPopup(player.TotalAddOn, string.Format(TournamentMessageResources.HOW_MUCH_ADDON_FOR_PLAYER, StringFormatUtil.ToFormatFullName(player.FirstName, player.LastName)), player.Id, EditPlayerTotalAddon());
                }
                catch (FunctionalException e)
                {
                    NavigationManager.NavigateTo(e.RedirectUrl!);
                    return;
                }
            };
        }

        private Action<int, int> EditPlayerTotalAddon()
        {
            return async (int playerId, int addonNb) =>
            {
                try
                {
                    TournamentInProgressDto tournamentInProgress = TournamentInProgressStore.CheckTournamentAlwaysInProgress();

                    APICallResult<PlayerAddonEditionResultDto> result = await TournamentService.EditPlayerTotalAddon(playerId, addonNb);

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

                    tournamentInProgress = TournamentInProgressStore.Update(result.Data);

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

        public EventCallback ShowTournamentInProgressMenu => EventCallback.Factory.Create(this, () =>
        {
            PopupEventHandler.Open
            (
                GenerateTournamentInProgressActions(),
                $"Saison {TournamentInProgressViewModel.Season} - Tournoi n°{TournamentInProgressViewModel.TournamentNumber}",
                TournamentInProgressViewModel.Id
            );
        });

        private List<PopupButtonViewModel> GenerateTournamentInProgressActions()
        {
            List<PopupButtonViewModel> actions = new List<PopupButtonViewModel>();

            try
            {
                TournamentInProgressDto tournamentInProgressDto = TournamentInProgressStore.CheckTournamentAlwaysInProgress();

                actions.Add(new PopupButtonViewModel
                {
                    Action = CancelTournamentInProgress(),
                    Label = PopupTournamentInProgressActionResources.CANCEL_TOURNAMENT_IN_PROGRESS
                });

                actions.Add(new PopupButtonViewModel
                {
                    Action = OpenAddPlayerPopup(),
                    Label = PopupTournamentInProgressActionResources.ADD_PLAYER
                });

                if (!tournamentInProgressDto.IsFinalTable)
                {
                    actions.Add(new PopupButtonViewModel
                    {
                        Action = GoToNextStep(),
                        Label = "Etape suivante"
                    });
                }

                if (tournamentInProgressDto.IsFinalTable || tournamentInProgressDto.IsAddOn)
                {
                    actions.Add(new PopupButtonViewModel
                    {
                        Action = GoToPreviousStep(),
                        Label = "Etape précédente"
                    });
                }

                // Changer repartition gain
                actions.Add(new PopupButtonViewModel
                {
                    Action = OpenDispatchJackpotPopup(),
                    Label = "Modifier répartition gains"
                });
            }
            catch (FunctionalException e)
            {
                NavigationManager.NavigateTo(e.RedirectUrl!);
            }

            return actions;
        }

        private Action<int?> OpenAddPlayerPopup()
        {
            return async (int? tournamentId) =>
            {
                try
                {
                    TournamentInProgressStore.CheckTournamentAlwaysInProgress();

                    APICallResult<PlayerSelectionViewModel> result = await TournamentService.LoadPlayersForPlayingTournamentInProgress(tournamentId!.Value);

                    if (!result.Success)
                    {
                        if (!string.IsNullOrEmpty(result.RedirectUrl))
                        {
                            throw new FunctionalException(result.ErrorMessage!, result.RedirectUrl);
                        }
                        PopupEventHandler.Open(
                                    msg: result.ErrorMessage!,
                                    isError: true,
                                    title: MainLabelResources.ERROR,
                                    onValid: null
                                    );
                        return;
                    }

                    IEnumerable<IdSelectableViewModel> selectableIds = result.Data.AvailablePlayers.Select(p => new IdSelectableViewModel(p));

                    PopupEventHandler.Open(selectableIds, PopupTournamentActionResources.ADD_PLAYERS, AddPlayers);
                }
                catch (FunctionalException e)
                {
                    NavigationManager.NavigateTo(e.RedirectUrl!);
                    return;
                }
            };
        }

        private EventCallback<IEnumerable<int>> AddPlayers => EventCallback.Factory.Create(this, async (IEnumerable<int> selectedItemIds) =>
        {
            try
            {
                TournamentInProgressDto tournamentInProgress = TournamentInProgressStore.CheckTournamentAlwaysInProgress();

                APICallResult<AddPlayersResultDto> result = await TournamentService.AddPlayersIntoTournamentInProgress(selectedItemIds, tournamentInProgress.Id);

                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.RedirectUrl))
                    {
                        throw new FunctionalException(result.ErrorMessage!, result.RedirectUrl);
                    }
                    PopupEventHandler.Open(
                                msg: result.ErrorMessage!,
                                isError: true,
                                title: MainLabelResources.ERROR,
                                onValid: null
                                );
                    return;
                }

                tournamentInProgress = TournamentInProgressStore.AddPlayers(result.Data);

                InitializedData(tournamentInProgress);
            }
            catch (FunctionalException ex)
            {
                NavigationManager.NavigateTo(ex.RedirectUrl!);
                return;
            }
        });

        private Action<int?> CancelTournamentInProgress()
        {
            return async (int? tournamentInProgressId) =>
            {
                try
                {
                    TournamentInProgressStore.CheckTournamentAlwaysInProgress();

                    APICallResultBase result = await TournamentService.CancelTournamentInProgress(tournamentInProgressId!.Value);

                    if (!result.Success)
                    {
                        if (!string.IsNullOrEmpty(result.RedirectUrl))
                        {
                            NavigationManager.NavigateTo(result.RedirectUrl);
                            return;
                        }
                        else
                        {
                            PopupEventHandler.Open(
                                msg: result.ErrorMessage!,
                                isError: true,
                                title: MainLabelResources.ERROR,
                                onValid: null
                                );
                            return;
                        }
                    }
                    else
                    {
                        TournamentInProgressStore.Clean();
                        NavigationManager.NavigateTo(string.Format(RouteResources.MAIN_ERROR, TournamentMessageResources.TOURNAMENT_IN_PROGRESS_CANCELLED));
                        return;
                    }
                }
                catch (FunctionalException e)
                {
                    NavigationManager.NavigateTo(e.RedirectUrl!);
                    return;
                }
            };
        }

        private Action<int?> GoToNextStep()
        {
            return async (int? tournamentInProgressId) =>
            {
                try
                {
                    TournamentInProgressStore.CheckTournamentAlwaysInProgress();

                    APICallResult<TournamentStepEnum> result = await TournamentService.GoToTournamentInProgressNextStep(tournamentInProgressId!.Value);

                    if (!result.Success)
                    {
                        if (!string.IsNullOrEmpty(result.RedirectUrl))
                        {
                            NavigationManager.NavigateTo(result.RedirectUrl);
                            return;
                        }
                        else
                        {
                            PopupEventHandler.Open(
                                msg: result.ErrorMessage!,
                                isError: true,
                                title: MainLabelResources.ERROR,
                                onValid: null
                                );
                            return;
                        }
                    }
                    else
                    {
                        TournamentInProgressDto tournamentInProgress = TournamentInProgressStore.GoToNextStep(result.Data);

                        InitializedData(tournamentInProgress);

                        StateHasChanged();
                    }
                }
                catch (FunctionalException e)
                {
                    NavigationManager.NavigateTo(e.RedirectUrl!);
                    return;
                }
            };
        }

        private Action<int?> GoToPreviousStep()
        {
            return async (int? tournamentInProgressId) =>
            {
                try
                {
                    TournamentInProgressStore.CheckTournamentAlwaysInProgress();

                    APICallResult<SwitchTournamentStepResultDto> result = await TournamentService.GoToTournamentInProgressPreviousStep(tournamentInProgressId!.Value);

                    if (!result.Success)
                    {
                        if (!string.IsNullOrEmpty(result.RedirectUrl))
                        {
                            NavigationManager.NavigateTo(result.RedirectUrl);
                            return;
                        }
                        else
                        {
                            PopupEventHandler.Open(
                                msg: result.ErrorMessage!,
                                isError: true,
                                title: MainLabelResources.ERROR,
                                onValid: null
                                );
                            return;
                        }
                    }
                    else
                    {
                        TournamentInProgressDto tournamentInProgress = TournamentInProgressStore.GoToPreviousStep(result.Data);

                        InitializedData(tournamentInProgress);

                        StateHasChanged();
                    }
                }
                catch (FunctionalException e)
                {
                    NavigationManager.NavigateTo(e.RedirectUrl!);
                    return;
                }
            };
        }

        private Action<int?> OpenDispatchJackpotPopup()
        {
            return (int? tournamentId) =>
            {
                try
                {
                    TournamentInProgressDto tournamentInProgressDto = TournamentInProgressStore.CheckTournamentAlwaysInProgress();

                    PopupEventHandler.OpenDispatchJackpotPopup(tournamentInProgressDto.WinnableMoneyByPosition, tournamentInProgressDto.CalculateTotalJackpot(), OnValidDispatchJackpot);
                }
                catch (FunctionalException e)
                {
                    NavigationManager.NavigateTo(e.RedirectUrl!);
                    return;
                }
            };
        }

        private EventCallback<IDictionary<int, int>> OnValidDispatchJackpot => EventCallback.Factory.Create(this, async (IDictionary<int, int> winnableMoneysByPosition) =>
        {
            try
            {
                TournamentInProgressDto tournamentInProgressDto = TournamentInProgressStore.CheckTournamentAlwaysInProgress();

                APICallResult<IEnumerable<JackpotDistribution>> result = await TournamentService.EditWinnableMoneysByPosition(winnableMoneysByPosition, tournamentInProgressDto.Id);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.RedirectUrl))
                    {
                        NavigationManager.NavigateTo(result.RedirectUrl);
                        return;
                    }
                    else
                    {
                        PopupEventHandler.Open(
                            msg: result.ErrorMessage!,
                            isError: true,
                            title: MainLabelResources.ERROR,
                            onValid: null
                            );
                        return;
                    }
                }

                tournamentInProgressDto = TournamentInProgressStore.UpdateWinnableMoneysByPosition(result.Data);

                InitializedData(tournamentInProgressDto);
            }
            catch (FunctionalException e)
            {
                NavigationManager.NavigateTo(e.RedirectUrl!);
                return;
            }
        });
    }
}
