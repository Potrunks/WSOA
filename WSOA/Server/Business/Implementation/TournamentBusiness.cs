using log4net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Business.Utils;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
using WSOA.Shared.Exceptions;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;
using WSOA.Shared.Utils;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Business.Implementation
{
    public class TournamentBusiness : ITournamentBusiness
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IMenuRepository _menuRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMailService _mailService;
        private readonly IUserRepository _userRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IBonusTournamentRepository _bonusTournamentRepository;
        private readonly IEliminationRepository _eliminationRepository;
        private readonly IBonusTournamentEarnedRepository _bonusTournamentEarnedRepository;

        private readonly ILog _log = LogManager.GetLogger(nameof(TournamentBusiness));

        public TournamentBusiness
        (
            ITransactionManager transactionManager,
            IMenuRepository menuRepository,
            ITournamentRepository tournamentRepository,
            IMailService mailService,
            IUserRepository userRepository,
            IAddressRepository addressRepository,
            IPlayerRepository playerRepository,
            IBonusTournamentRepository bonusTournamentRepository,
            IEliminationRepository eliminationRepository,
            IBonusTournamentEarnedRepository bonusTournamentEarnedRepository
        )
        {
            _transactionManager = transactionManager;
            _menuRepository = menuRepository;
            _tournamentRepository = tournamentRepository;
            _mailService = mailService;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _playerRepository = playerRepository;
            _bonusTournamentRepository = bonusTournamentRepository;
            _eliminationRepository = eliminationRepository;
            _bonusTournamentEarnedRepository = bonusTournamentEarnedRepository;
        }

        public APICallResultBase CreateTournament(TournamentCreationFormViewModel form, ISession session)
        {
            APICallResultBase result = new APICallResultBase(true);

            try
            {
                _transactionManager.BeginTransaction();

                session.CanUserPerformAction(_menuRepository, form.SubSectionId);
                form.StartDate.IsAfterOrEqualUtcNow();

                Tournament newTournament = new Tournament(form);
                _tournamentRepository.SaveTournament(newTournament);

                IEnumerable<User> allUsers = _userRepository.GetAllUsers();
                _mailService.SendMails
                (
                    allUsers.Select(usr => usr.Email),
                    TournamentBusinessResources.MAIL_SUBJECT_NEW_TOURNAMENT,
                    string.Format(TournamentBusinessResources.MAIL_BODY_NEW_TOURNAMENT, form.BaseUri)
                );

                _transactionManager.CommitTransaction();
            }
            catch (FunctionalException e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                return new APICallResultBase(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                _transactionManager.RollbackTransaction();
                _log.Error(e.Message);
                return new APICallResultBase(MainBusinessResources.TECHNICAL_ERROR);
            }

            return result;
        }

        public APICallResult<TournamentCreationDataViewModel> LoadTournamentCreationDatas(int subSectionId, ISession session)
        {
            APICallResult<TournamentCreationDataViewModel> result = new APICallResult<TournamentCreationDataViewModel>(true);

            try
            {
                MainNavSubSection subSection = session.CanUserPerformAction(_menuRepository, subSectionId);

                IEnumerable<Address> addresses = _addressRepository.GetAllAddresses();
                if (addresses.IsNullOrEmpty())
                {
                    string errorMsg = string.Format(MainBusinessResources.NULL_OR_EMPTY_OBJ_NOT_ALLOWED, nameof(addresses), nameof(LoadTournamentCreationDatas));
                    throw new FunctionalException(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
                }

                result.Data = new TournamentCreationDataViewModel(addresses, subSection.Description);
            }
            catch (FunctionalException e)
            {
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                return new APICallResult<TournamentCreationDataViewModel>(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                return new APICallResult<TournamentCreationDataViewModel>(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
            }

            return result;
        }

        public APICallResult<TournamentsViewModel> LoadTournamentsNotOver(int subSectionId, ISession session)
        {
            APICallResult<TournamentsViewModel> result = new APICallResult<TournamentsViewModel>(true);

            try
            {
                MainNavSubSection mainNavSubSection = session.CanUserPerformAction(_menuRepository, subSectionId);
                int currentUserId = session.GetCurrentUserId();
                List<TournamentDto> tournamentDtos = _tournamentRepository.GetTournamentDtosByIsOverAndIsInProgress(false, false);
                result.Data = new TournamentsViewModel(tournamentDtos, mainNavSubSection.Description, currentUserId);
            }
            catch (FunctionalException e)
            {
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                return new APICallResult<TournamentsViewModel>(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                return new APICallResult<TournamentsViewModel>(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
            }

            return result;
        }

        public APICallResult<PlayerViewModel> SignUpTournament(SignUpTournamentFormViewModel formVM, ISession session)
        {
            APICallResult<PlayerViewModel> result = new APICallResult<PlayerViewModel>(true);

            int tournamentId = formVM.TournamentId;
            string presenceStateCode = formVM.PresenceStateCode;

            try
            {
                _transactionManager.BeginTransaction();

                int currentUsrId = session.GetCurrentUserId();

                Tournament currentTournament = _tournamentRepository.GetTournamentById(tournamentId);
                if (currentTournament.IsOver)
                {
                    string errorMsg = TournamentBusinessResources.TOURNAMENT_ALREADY_OVER;
                    throw new Exception(errorMsg);
                }

                Player? player = _playerRepository.GetPlayerByTournamentIdAndUserId(tournamentId, currentUsrId);
                if (player == null)
                {
                    player = new Player
                    {
                        PlayedTournamentId = tournamentId,
                        UserId = currentUsrId
                    };
                }
                player.PresenceStateCode = presenceStateCode;
                _playerRepository.SavePlayer(player);

                result.Data = new PlayerViewModel(_userRepository.GetUserById(currentUsrId), player);

                _transactionManager.CommitTransaction();
            }
            catch (FunctionalException e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                return new APICallResult<PlayerViewModel>(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                _transactionManager.RollbackTransaction();
                _log.Error(e.Message);
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                return new APICallResult<PlayerViewModel>(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
            }
            return result;
        }

        public APICallResult<PlayerSelectionViewModel> LoadPlayersForPlayingTournament(int tournamentId, ISession session)
        {
            APICallResult<PlayerSelectionViewModel> result = new APICallResult<PlayerSelectionViewModel>(true);

            try
            {
                session.CanUserPerformAction(_userRepository, BusinessActionResources.EXECUTE_TOURNAMENT);
                Tournament tournament = _tournamentRepository.GetTournamentById(tournamentId);
                CanExecuteTournament(tournament);
                IEnumerable<PlayerDto> presentPlayers = _playerRepository.GetPlayersByTournamentIdAndPresenceStateCode(tournamentId, PresenceStateResources.PRESENT_CODE);
                IEnumerable<User> availableUsers = _userRepository.GetAllUsers(blacklistUserIds: presentPlayers.Select(pla => pla.User.Id));
                result.Data = new PlayerSelectionViewModel(presentPlayers, availableUsers);
            }
            catch (FunctionalException e)
            {
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                return new APICallResult<PlayerSelectionViewModel>(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                return new APICallResult<PlayerSelectionViewModel>(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
            }

            return result;
        }

        private void CanExecuteTournament(Tournament tournament)
        {
            try
            {
                bool existsTournamentInProgress = _tournamentRepository.ExistsTournamentByIsInProgress(true);
                if (existsTournamentInProgress)
                {
                    throw new FunctionalException(TournamentBusinessResources.EXISTS_TOURNAMENT_IN_PROGRESS, null);
                }

                bool canExecute = _tournamentRepository.ExistsTournamentByTournamentIdIsOverAndIsInProgress(tournament.Id, false, false);
                if (!canExecute)
                {
                    throw new FunctionalException(TournamentBusinessResources.CANNOT_EXECUTE_TOURNAMENT, null);
                }

                Tournament? lastTournament = _tournamentRepository.GetLastFinishedTournamentBySeason(tournament.Season);
                if (lastTournament != null && tournament.StartDate <= lastTournament.StartDate)
                {
                    throw new FunctionalException(TournamentMessageResources.TOURNAMENT_PAST, null);
                }
            }
            catch (Exception e)
            {
                throw new FunctionalException(e.Message, string.Format(RouteBusinessResources.MAIN_ERROR, e.Message));
            }
        }

        public APICallResultBase SaveTournamentPrepared(TournamentPreparedDto tournamentPrepared, ISession session)
        {
            APICallResultBase result = new APICallResultBase(false);

            try
            {
                _transactionManager.BeginTransaction();

                session.CanUserPerformAction(_userRepository, BusinessActionResources.EXECUTE_TOURNAMENT);

                if (!tournamentPrepared.SelectedUserIds.Any())
                {
                    string errorMsg = TournamentMessageResources.TOURNAMENT_NO_PLAYER_SELECTED;
                    throw new FunctionalException(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
                }

                TournamentDto currentTournament = _tournamentRepository.GetTournamentDtoById(tournamentPrepared.TournamentId);

                CanExecuteTournament(currentTournament.Tournament);

                List<Player> newPlayers = new List<Player>();
                List<Player> updatedPlayers = new List<Player>();
                IDictionary<int, Player> selectedPlayersAlreadySignUpByUsrId = currentTournament.Players.Where(pla => tournamentPrepared.SelectedUserIds.Contains(pla.Player.UserId)).ToDictionary(pla => pla.User.Id, pla => pla.Player);
                foreach (int selectedUsrId in tournamentPrepared.SelectedUserIds)
                {
                    Player selectedPlayer;
                    if (selectedPlayersAlreadySignUpByUsrId.TryGetValue(selectedUsrId, out selectedPlayer))
                    {
                        selectedPlayer.PresenceStateCode = PresenceStateResources.PRESENT_CODE;
                        updatedPlayers.Add(selectedPlayer);
                    }
                    else
                    {
                        selectedPlayer = new Player
                        {
                            UserId = selectedUsrId,
                            PlayedTournamentId = currentTournament.Tournament.Id,
                            PresenceStateCode = PresenceStateResources.PRESENT_CODE
                        };
                        newPlayers.Add(selectedPlayer);
                    }
                }

                IEnumerable<Player> playersToDelete = currentTournament.Players.Where(pla => !tournamentPrepared.SelectedUserIds.Contains(pla.User.Id))
                                                                               .Select(pla => pla.Player);

                currentTournament.Tournament.IsInProgress = true;

                _playerRepository.DeletePlayers(playersToDelete);
                _playerRepository.SavePlayers(updatedPlayers.Concat(newPlayers));
                _tournamentRepository.SaveTournament(currentTournament.Tournament);

                MainNavSubSection tournamentInProgressSubSection = _menuRepository.GetMainNavSubSectionByUrl(RouteBusinessResources.TOURNAMENT_IN_PROGRESS);
                result.RedirectUrl = $"{tournamentInProgressSubSection.Url}/{tournamentInProgressSubSection.Id}";

                result.Success = true;

                _transactionManager.CommitTransaction();
            }
            catch (FunctionalException e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                return new APICallResultBase(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                _log.Error(e.Message);
                return new APICallResultBase(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
            }

            return result;
        }

        public APICallResult<TournamentInProgressDto> LoadTournamentInProgress(int subSectionId, ISession session)
        {
            APICallResult<TournamentInProgressDto> result = new APICallResult<TournamentInProgressDto>(false);

            try
            {
                session.CanUserPerformAction(_menuRepository, subSectionId);

                Tournament? tournamentInProgress = _tournamentRepository.GetTournamentInProgress();
                if (tournamentInProgress == null)
                {
                    string errorMsg = TournamentMessageResources.NO_TOURNAMENT_IN_PROGRESS;
                    throw new FunctionalException(errorMsg, string.Format(RouteResources.MAIN_ERROR, errorMsg));
                }

                IEnumerable<PlayerDto> presentPlayers = _playerRepository.GetPlayersByTournamentIdAndPresenceStateCode(tournamentInProgress.Id, PresenceStateResources.PRESENT_CODE);
                if (!presentPlayers.Any())
                {
                    string errorMsg = TournamentMessageResources.NO_PLAYERS_PRESENT;
                    throw new FunctionalException(errorMsg, string.Format(RouteResources.MAIN_ERROR, errorMsg));
                }

                IDictionary<string, BonusTournament> winnableBonusByCode = _bonusTournamentRepository.GetAll().ToDictionary(bonus => bonus.Code);
                IDictionary<int, IEnumerable<BonusTournamentEarned>> bonusEarnedsByPlayerId = _playerRepository.GetBonusTournamentEarnedsByPlayerIds(presentPlayers.Select(pla => pla.Player.Id));

                UsersBestDto usersBestDto = GetBestUsersByCurrentSeasonTournament(tournamentInProgress);

                result.Data = new TournamentInProgressDto(tournamentInProgress, presentPlayers, winnableBonusByCode, bonusEarnedsByPlayerId, usersBestDto.TotalSeasonTournamentPlayed, usersBestDto.WinnerPreviousTournament, usersBestDto.FirstRanked);

                result.Success = true;
            }
            catch (FunctionalException e)
            {
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                return new APICallResult<TournamentInProgressDto>(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                _log.Error(e.Message);
                return new APICallResult<TournamentInProgressDto>(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
            }

            return result;
        }

        private UsersBestDto GetBestUsersByCurrentSeasonTournament(Tournament tournament)
        {
            User? lastWinner = null;
            User? firstRankUser = null;

            int tournamentNb = _tournamentRepository.GetTournamentNumber(tournament);
            if (tournamentNb > 1)
            {
                Tournament tournamentPrevious = _tournamentRepository.GetPreviousTournament(tournament);
                lastWinner = _userRepository.GetUserWinnerByTournamentId(tournamentPrevious.Id);
                firstRankUser = _userRepository.GetFirstRankUserBySeasonCode(tournament.Season);
            }

            return new UsersBestDto
            {
                FirstRanked = firstRankUser,
                WinnerPreviousTournament = lastWinner,
                TotalSeasonTournamentPlayed = tournamentNb
            };
        }

        public APICallResult<EliminationCreationResultDto> EliminatePlayer(EliminationCreationDto eliminationDto, ISession session)
        {
            APICallResult<EliminationCreationResultDto> result = new APICallResult<EliminationCreationResultDto>(false);

            try
            {
                _transactionManager.BeginTransaction();

                result.Data = new EliminationCreationResultDto();

                session.CanUserPerformAction(_userRepository, BusinessActionResources.ELIMINATE_PLAYER);

                IEnumerable<int> playerConcernedIds = new List<int> { eliminationDto.EliminatedPlayerId, eliminationDto.EliminatorPlayerId };

                IDictionary<int, Player> players = _playerRepository.GetPlayersByIds(playerConcernedIds);
                if (players.Count != 2)
                {
                    throw new Exception
                        (
                            string.Format
                            (
                                TournamentMessageResources.PLAYERS_ELIMINATION_CONCERNED_MISSING,
                                eliminationDto.EliminatedPlayerId,
                                eliminationDto.EliminatorPlayerId
                            )
                        );
                }

                IEnumerable<Elimination> existingEliminations = _eliminationRepository.GetEliminationsByPlayerVictimIds(playerConcernedIds);
                if (existingEliminations.Any(elim => elim.IsDefinitive))
                {
                    throw new FunctionalException(TournamentMessageResources.PLAYERS_ALREADY_DEFINITIVELY_ELIMINATED, null);
                }

                Player eliminatedPlayer = players[eliminationDto.EliminatedPlayerId];
                if (eliminationDto.HasReBuy)
                {
                    eliminatedPlayer.TotalReBuy = eliminatedPlayer.TotalReBuy == null ? 1 : eliminatedPlayer.TotalReBuy + 1;
                }
                if (!eliminationDto.HasReBuy && eliminatedPlayer.TotalReBuy == null && !eliminatedPlayer.WasAddOn.GetValueOrDefault())
                {
                    eliminatedPlayer.TotalReBuy = 0;
                }
                _playerRepository.SavePlayer(eliminatedPlayer);

                result.Data.EliminatedPlayerTotalReBuy = eliminatedPlayer.TotalReBuy;

                Elimination elimination = new Elimination
                {
                    IsDefinitive = !eliminationDto.HasReBuy,
                    PlayerEliminatorId = eliminationDto.EliminatorPlayerId,
                    PlayerVictimId = eliminatedPlayer.Id,
                    CreationDate = DateTime.UtcNow
                };
                _eliminationRepository.SaveElimination(elimination);

                Tournament currentTournament = _tournamentRepository.GetTournamentById(eliminatedPlayer.PlayedTournamentId);
                IEnumerable<PlayerDto> allPlayers = _playerRepository.GetPlayersByTournamentIdAndPresenceStateCode(eliminatedPlayer.PlayedTournamentId, PresenceStateResources.PRESENT_CODE);
                int nbPlayers = allPlayers.Where(pla => pla.Player.CurrentTournamentPosition == null)
                                          .Count();

                if (elimination.IsDefinitive)
                {
                    eliminatedPlayer.CurrentTournamentPosition = nbPlayers;
                    eliminatedPlayer.WasAddOn = eliminationDto.IsAddOn;
                    eliminatedPlayer.WasFinalTable = eliminationDto.IsFinalTable;

                    if (eliminatedPlayer.CurrentTournamentPosition.Value > TournamentPointsResources.TournamentPointAmountByPosition.Keys.Max())
                    {
                        eliminatedPlayer.TotalWinningsPoint = TournamentPointsResources.MinimumPointAmount;
                    }
                    else
                    {
                        eliminatedPlayer.TotalWinningsPoint = TournamentPointsResources.TournamentPointAmountByPosition[eliminatedPlayer.CurrentTournamentPosition.Value];
                    }

                    if (eliminatedPlayer.CurrentTournamentPosition <= eliminationDto.WinnableMoneyByPosition.Keys.Max())
                    {
                        eliminatedPlayer.TotalWinningsAmount = eliminationDto.WinnableMoneyByPosition[eliminatedPlayer.CurrentTournamentPosition.Value];
                    }
                    else
                    {
                        eliminatedPlayer.TotalWinningsAmount = 0;
                    }

                    _playerRepository.SavePlayer(eliminatedPlayer);

                    UsersBestDto usersBestDto = GetBestUsersByCurrentSeasonTournament(currentTournament);
                    if (usersBestDto.FirstRanked != null && usersBestDto.FirstRanked.Id == eliminatedPlayer.UserId)
                    {
                        BonusTournament firstRankedKilledBonus = _bonusTournamentRepository.GetBonusTournamentByCode(BonusTournamentResources.FIRST_RANKED_KILLED);
                        BonusTournamentEarned newBonus = new BonusTournamentEarned
                        {
                            BonusTournamentCode = firstRankedKilledBonus.Code,
                            PlayerId = eliminationDto.EliminatorPlayerId,
                            PointAmount = firstRankedKilledBonus.PointAmount,
                            Occurrence = 1
                        };
                        _bonusTournamentEarnedRepository.SaveBonusTournamentEarned(newBonus);
                        result.Data.EliminatorPlayerWonBonusCodes.Add(newBonus.BonusTournamentCode);
                    }

                    if (usersBestDto.WinnerPreviousTournament != null && usersBestDto.WinnerPreviousTournament.Id == eliminatedPlayer.UserId)
                    {
                        BonusTournament firstRankedKilledBonus = _bonusTournamentRepository.GetBonusTournamentByCode(BonusTournamentResources.PREVIOUS_WINNER_KILLED);
                        BonusTournamentEarned newBonus = new BonusTournamentEarned
                        {
                            BonusTournamentCode = firstRankedKilledBonus.Code,
                            PlayerId = eliminationDto.EliminatorPlayerId,
                            PointAmount = firstRankedKilledBonus.PointAmount,
                            Occurrence = 1
                        };
                        _bonusTournamentEarnedRepository.SaveBonusTournamentEarned(newBonus);
                        result.Data.EliminatorPlayerWonBonusCodes.Add(newBonus.BonusTournamentCode);
                    }
                }

                if (nbPlayers == 2)
                {
                    Player winner = players[eliminationDto.EliminatorPlayerId];
                    winner.TotalWinningsPoint = TournamentPointsResources.TournamentPointAmountByPosition[1];
                    winner.CurrentTournamentPosition = 1;
                    winner.WasAddOn = eliminationDto.IsAddOn;
                    winner.WasFinalTable = eliminationDto.IsFinalTable;
                    winner.TotalWinningsAmount = eliminationDto.WinnableMoneyByPosition[1];
                    _playerRepository.SavePlayer(winner);

                    currentTournament.IsOver = true;
                    currentTournament.IsInProgress = false;
                    _tournamentRepository.SaveTournament(currentTournament);

                    result.Data.IsTournamentOver = currentTournament.IsOver;
                }

                result.Success = true;

                _transactionManager.CommitTransaction();
            }
            catch (FunctionalException e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                result.ErrorMessage = errorMsg;
                result.RedirectUrl = e.RedirectUrl;
            }
            catch (Exception e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                _log.Error(e.Message);
                result.ErrorMessage = errorMsg;
                result.RedirectUrl = string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg);
            }

            return result;
        }

        public APICallResult<BonusTournamentEarnedEditResultDto> SaveBonusTournamentEarned(BonusTournamentEarnedEditDto bonusEarnedDto, ISession session)
        {
            APICallResult<BonusTournamentEarnedEditResultDto> result = new APICallResult<BonusTournamentEarnedEditResultDto>(false);

            try
            {
                _transactionManager.BeginTransaction();

                session.CanUserPerformAction(_userRepository, BusinessActionResources.EDIT_BONUS_TOURNAMENT_EARNED);

                BonusTournamentEarned bonusTournamentEarned = new BonusTournamentEarned
                {
                    BonusTournamentCode = bonusEarnedDto.ConcernedBonusTournament.Code,
                    Occurrence = 1,
                    PlayerId = bonusEarnedDto.ConcernedPlayerId,
                    PointAmount = bonusEarnedDto.ConcernedBonusTournament.PointAmount
                };
                bonusTournamentEarned = _bonusTournamentEarnedRepository.SaveBonusTournamentEarned(bonusTournamentEarned);

                _transactionManager.CommitTransaction();

                result.Data = new BonusTournamentEarnedEditResultDto
                {
                    EditedBonusTournamentEarned = bonusTournamentEarned
                };
                result.Success = true;
            }
            catch (FunctionalException e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                result.ErrorMessage = errorMsg;
                result.RedirectUrl = e.RedirectUrl;
            }
            catch (Exception e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                _log.Error(e.Message);
                result.ErrorMessage = errorMsg;
                result.RedirectUrl = string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg);
            }

            return result;
        }

        public APICallResult<BonusTournamentEarnedEditResultDto> DeleteBonusTournamentEarned(BonusTournamentEarnedEditDto bonusTournamentEarnedEditDto, ISession session)
        {
            APICallResult<BonusTournamentEarnedEditResultDto> result = new APICallResult<BonusTournamentEarnedEditResultDto>(false);

            try
            {
                _transactionManager.BeginTransaction();

                session.CanUserPerformAction(_userRepository, BusinessActionResources.EDIT_BONUS_TOURNAMENT_EARNED);

                BonusTournamentEarned bonusTournamentEarnedUpdated = _bonusTournamentEarnedRepository.DeleteBonusTournamentEarned(bonusTournamentEarnedEditDto.ConcernedPlayerId, bonusTournamentEarnedEditDto.ConcernedBonusTournament.Code);

                _transactionManager.CommitTransaction();

                result.Data = new BonusTournamentEarnedEditResultDto
                {
                    EditedBonusTournamentEarned = bonusTournamentEarnedUpdated
                };
                result.Success = true;
            }
            catch (FunctionalException e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                result.ErrorMessage = errorMsg;
                result.RedirectUrl = e.RedirectUrl;
            }
            catch (Exception e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                _log.Error(e.Message);
                result.ErrorMessage = errorMsg;
                result.RedirectUrl = string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg);
            }

            return result;
        }

        public APICallResult<CancelEliminationResultDto> CancelLastPlayerElimination(EliminationEditionDto eliminationEditionDto, ISession session)
        {
            APICallResult<CancelEliminationResultDto> result = new APICallResult<CancelEliminationResultDto>(false);
            IEnumerable<BonusTournament>? bonusTournamentsLostByEliminator = null;

            try
            {
                _transactionManager.BeginTransaction();

                session.CanUserPerformAction(_userRepository, BusinessActionResources.ELIMINATE_PLAYER);

                PlayerEliminationsDto existingPlayerWithEliminations = _eliminationRepository.GetPlayerEliminationsDtosByPlayerVictimIds(new List<int> { eliminationEditionDto.EliminatedPlayerId }).Single();

                Elimination lastElimination = existingPlayerWithEliminations.Eliminations.OrderByDescending(elim => elim.CreationDate).First();
                Player eliminatorPlayer = existingPlayerWithEliminations.EliminatorPlayersById[lastElimination.PlayerEliminatorId];
                Player eliminatedPlayer = existingPlayerWithEliminations.EliminatedPlayer;

                if (lastElimination.IsDefinitive)
                {
                    eliminatedPlayer.CurrentTournamentPosition = null;
                    eliminatedPlayer.WasAddOn = eliminationEditionDto.IsAddOn;
                    eliminatedPlayer.WasFinalTable = eliminationEditionDto.IsFinalTable;
                    eliminatedPlayer.TotalWinningsPoint = null;
                    eliminatedPlayer.TotalWinningsAmount = null;

                    KeyValuePair<int, IEnumerable<BonusTournamentEarned>> eliminatorBonus = _playerRepository.GetBonusTournamentEarnedsByPlayerIds(new List<int> { eliminatorPlayer.Id }).SingleOrDefault();
                    if (eliminatorBonus.Value != null && eliminatorBonus.Value.Any(bon => new List<string> { BonusTournamentResources.FIRST_RANKED_KILLED, BonusTournamentResources.PREVIOUS_WINNER_KILLED }.Contains(bon.BonusTournamentCode)))
                    {
                        IDictionary<string, BonusTournamentEarned> eliminatorBonusByCode = eliminatorBonus.Value.ToDictionary(bon => bon.BonusTournamentCode);
                        UsersBestDto usersBestDto = GetBestUsersByCurrentSeasonTournament(existingPlayerWithEliminations.Tournament);
                        List<BonusTournamentEarned> eliminatorBonusToDelete = new List<BonusTournamentEarned>();

                        if (usersBestDto.FirstRanked != null
                            && usersBestDto.FirstRanked.Id == existingPlayerWithEliminations.EliminatedUser.Id
                            && eliminatorBonusByCode.TryGetValue(BonusTournamentResources.FIRST_RANKED_KILLED, out BonusTournamentEarned? firstRankedKilledBonus))
                        {
                            eliminatorBonusToDelete.Add(firstRankedKilledBonus);
                        }

                        if (usersBestDto.WinnerPreviousTournament != null
                            && usersBestDto.WinnerPreviousTournament.Id == existingPlayerWithEliminations.EliminatedUser.Id
                            && eliminatorBonusByCode.TryGetValue(BonusTournamentResources.PREVIOUS_WINNER_KILLED, out BonusTournamentEarned? previousWinnerKilledBonus))
                        {
                            eliminatorBonusToDelete.Add(previousWinnerKilledBonus);
                        }

                        _bonusTournamentEarnedRepository.DeleteBonusTournamentEarneds(eliminatorBonusToDelete);

                        bonusTournamentsLostByEliminator = _bonusTournamentRepository.GetBonusTournamentsByCodes(eliminatorBonusToDelete.Select(bonus => bonus.BonusTournamentCode));
                    }
                }
                else
                {
                    eliminatedPlayer.TotalReBuy--;
                }

                eliminatedPlayer.TotalReBuy = eliminatedPlayer.TotalReBuy == 0 ? null : eliminatedPlayer.TotalReBuy;

                _eliminationRepository.DeleteElimination(lastElimination);

                _transactionManager.CommitTransaction();

                result.Data = new CancelEliminationResultDto
                {
                    PlayerEliminated = eliminatedPlayer,
                    PlayerEliminator = eliminatorPlayer,
                    BonusTournamentsLostByEliminator = bonusTournamentsLostByEliminator,
                    EliminationCanceled = lastElimination
                };
                result.Success = true;
            }
            catch (FunctionalException e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                result.ErrorMessage = errorMsg;
                result.RedirectUrl = e.RedirectUrl;
            }
            catch (Exception e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                _log.Error(e.Message);
                result.ErrorMessage = errorMsg;
                result.RedirectUrl = string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg);
            }

            return result;
        }

        public APICallResult<Player> EditPlayerTotalAddon(int playerId, int addonNb, ISession session)
        {
            APICallResult<Player> result = new APICallResult<Player>(false);

            try
            {
                _transactionManager.BeginTransaction();

                session.CanUserPerformAction(_userRepository, BusinessActionResources.EDIT_TOTAL_ADDON);

                if (addonNb < 0)
                {
                    throw new FunctionalException(TournamentBusinessResources.TOTAL_ADDON_GIVEN_LESS_THAN_ZERO, null);
                }

                Player playerConcerned = _playerRepository.GetPlayersByIds(new List<int> { playerId }).Single().Value;
                if (!playerConcerned.WasAddOn.GetValueOrDefault())
                {
                    throw new FunctionalException(TournamentBusinessResources.PLAYER_NOT_IN_ADDON, null);
                }

                if (playerConcerned.TotalAddOn != addonNb)
                {
                    playerConcerned.TotalAddOn = addonNb;
                    _playerRepository.SavePlayer(playerConcerned);
                }

                _transactionManager.CommitTransaction();

                result.Success = true;
                result.Data = playerConcerned;
            }
            catch (FunctionalException e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                result.ErrorMessage = errorMsg;
                result.RedirectUrl = e.RedirectUrl;
            }
            catch (Exception e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                _log.Error(e.Message);
                result.ErrorMessage = errorMsg;
                result.RedirectUrl = string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg);
            }

            return result;
        }
    }
}
