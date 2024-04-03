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
        private readonly IJackpotDistributionRepository _jackpotDistributionRepository;

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
            IBonusTournamentEarnedRepository bonusTournamentEarnedRepository,
            IJackpotDistributionRepository jackpotDistributionRepository
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
            _jackpotDistributionRepository = jackpotDistributionRepository;
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
                try
                {
                    _mailService.SendMails
                    (
                        allUsers.Select(usr => usr.Email),
                        TournamentBusinessResources.MAIL_SUBJECT_NEW_TOURNAMENT,
                        string.Format(TournamentBusinessResources.MAIL_BODY_NEW_TOURNAMENT, form.BaseUri)
                    );
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }

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
            string presenceStateCode = formVM.PresenceStateCode!;

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

        public APICallResult<PlayerSelectionViewModel> LoadPlayersForPlayingTournamentInProgress(int tournamentId, ISession session)
        {
            APICallResult<PlayerSelectionViewModel> result = new APICallResult<PlayerSelectionViewModel>(true);

            try
            {
                session.CanUserPerformAction(_userRepository, BusinessActionResources.EDIT_TOURNAMENT_IN_PROGRESS);

                Tournament tournament = _tournamentRepository.GetTournamentById(tournamentId);
                if (!tournament.IsInProgress)
                {
                    throw new FunctionalException(TournamentMessageResources.NO_TOURNAMENT_IN_PROGRESS, string.Format(RouteBusinessResources.MAIN_ERROR, TournamentMessageResources.NO_TOURNAMENT_IN_PROGRESS));
                }

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
                    Player? selectedPlayer;
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

                List<JackpotDistribution> jackpotDistributions = _jackpotDistributionRepository.GetJackpotDistributionsByTournamentId(tournamentInProgress.Id);
                if (!jackpotDistributions.Any())
                {
                    int totalJackpot = tournamentInProgress.BuyIn * (presentPlayers.Count()
                                                + presentPlayers.Where(pla => pla.Player.TotalReBuy.HasValue && pla.Player.TotalReBuy > 0).Sum(pla => pla.Player.TotalReBuy!.Value)
                                                + presentPlayers.Where(pla => pla.Player.TotalAddOn.HasValue && pla.Player.TotalAddOn > 0).Sum(pla => pla.Player.TotalAddOn!.Value))
                                                - 20;
                    JackpotDistribution newJackpotDistribution = new JackpotDistribution
                    {
                        TournamentId = tournamentInProgress.Id,
                        Amount = totalJackpot,
                        PlayerPosition = 1
                    };
                    _jackpotDistributionRepository.SaveJackpotDistributions(new List<JackpotDistribution> { newJackpotDistribution });
                    jackpotDistributions.Add(newJackpotDistribution);
                }

                result.Data = new TournamentInProgressDto(
                    tournamentInProgress,
                    presentPlayers,
                    winnableBonusByCode,
                    bonusEarnedsByPlayerId,
                    usersBestDto.TotalSeasonTournamentPlayed,
                    usersBestDto.WinnerPreviousTournament,
                    usersBestDto.FirstRanked,
                    jackpotDistributions
                    );

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
            Tournament? tournamentPrevious = _tournamentRepository.GetPreviousTournament(tournament);
            if (tournamentPrevious != null)
            {
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
                Tournament currentTournament = _tournamentRepository.GetTournamentById(eliminatedPlayer.PlayedTournamentId);
                if (eliminationDto.HasReBuy)
                {
                    eliminatedPlayer.TotalReBuy = eliminatedPlayer.TotalReBuy == null ? 1 : eliminatedPlayer.TotalReBuy + 1;

                    List<JackpotDistribution> jackpotDistributions = _jackpotDistributionRepository.GetJackpotDistributionsByTournamentId(currentTournament.Id);
                    result.Data.UpdatedJackpotDistributions = AddAmountIntoJackpotDistributions(currentTournament.BuyIn, jackpotDistributions);
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

                Elimination lastElimination = existingPlayerWithEliminations.EliminatedPlayerEliminations.OrderByDescending(elim => elim.CreationDate).First();
                Player eliminatorPlayer = existingPlayerWithEliminations.EliminatorPlayersById[lastElimination.PlayerEliminatorId];
                Player eliminatedPlayer = existingPlayerWithEliminations.EliminatedPlayer;

                List<JackpotDistribution> jackpotDistributions = _jackpotDistributionRepository.GetJackpotDistributionsByTournamentId(eliminationEditionDto.TournamentId);

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

                    jackpotDistributions = RemoveAmountIntoJackpotDistributions(eliminationEditionDto.BuyIn, jackpotDistributions).ToList();
                }

                eliminatedPlayer.TotalReBuy = eliminatedPlayer.TotalReBuy == 0 ? null : eliminatedPlayer.TotalReBuy;

                _eliminationRepository.DeleteElimination(lastElimination);

                _transactionManager.CommitTransaction();

                result.Data = new CancelEliminationResultDto
                {
                    PlayerEliminated = eliminatedPlayer,
                    PlayerEliminator = eliminatorPlayer,
                    BonusTournamentsLostByEliminator = bonusTournamentsLostByEliminator,
                    EliminationCanceled = lastElimination,
                    UpdatedJackpotDistributions = jackpotDistributions
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

        private IEnumerable<JackpotDistribution> RemoveAmountIntoJackpotDistributions(int amountToRemove, IEnumerable<JackpotDistribution> jackpotDistributionsToUpdate)
        {
            List<int> jackpotDistributionIdsToRemove = new List<int>();

            for (int i = jackpotDistributionsToUpdate.Max(jac => jac.PlayerPosition); i > 0; i--)
            {
                if (amountToRemove <= 0)
                {
                    break;
                }

                JackpotDistribution jackpotDistribution = jackpotDistributionsToUpdate.Single(jac => jac.PlayerPosition == i);

                int amountRemoved = Math.Min(jackpotDistribution.Amount, amountToRemove);
                jackpotDistribution.Amount -= amountRemoved;
                amountToRemove -= amountRemoved;

                if (jackpotDistribution.Amount == 0)
                {
                    jackpotDistributionIdsToRemove.Add(jackpotDistribution.Id);
                }
            }

            _jackpotDistributionRepository.RemoveJackpotDistributions(jackpotDistributionsToUpdate.Where(jac => jackpotDistributionIdsToRemove.Contains(jac.Id)));
            return jackpotDistributionsToUpdate.Where(jac => !jackpotDistributionIdsToRemove.Contains(jac.Id));
        }

        private IEnumerable<JackpotDistribution> AddAmountIntoJackpotDistributions(int amountToAdd, IEnumerable<JackpotDistribution> jackpotDistributionsToUpdate)
        {
            JackpotDistribution firstJackpotDistribution = jackpotDistributionsToUpdate.Single(j => j.PlayerPosition == 1);
            firstJackpotDistribution.Amount += amountToAdd;
            _jackpotDistributionRepository.SaveJackpotDistributions(new List<JackpotDistribution> { firstJackpotDistribution });
            return jackpotDistributionsToUpdate;
        }

        public APICallResult<PlayerAddonEditionResultDto> EditPlayerTotalAddon(int playerId, int addonNb, ISession session)
        {
            APICallResult<PlayerAddonEditionResultDto> result = new APICallResult<PlayerAddonEditionResultDto>(false);

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

                List<JackpotDistribution> jackpotDistributions = _jackpotDistributionRepository.GetJackpotDistributionsByTournamentId(playerConcerned.PlayedTournamentId);
                Tournament tournament = _tournamentRepository.GetTournamentById(playerConcerned.PlayedTournamentId);

                if (playerConcerned.TotalAddOn != addonNb)
                {
                    int addOnDiff = addonNb - playerConcerned.TotalAddOn.GetValueOrDefault();
                    if (addOnDiff < 0)
                    {
                        jackpotDistributions = RemoveAmountIntoJackpotDistributions(Math.Abs(addOnDiff) * tournament.BuyIn, jackpotDistributions).ToList();
                    }
                    else
                    {
                        jackpotDistributions = AddAmountIntoJackpotDistributions(addOnDiff * tournament.BuyIn, jackpotDistributions).ToList();
                    }

                    playerConcerned.TotalAddOn = addonNb;
                    _playerRepository.SavePlayer(playerConcerned);
                }

                _transactionManager.CommitTransaction();

                result.Success = true;
                result.Data = new PlayerAddonEditionResultDto
                {
                    PlayerUpdated = playerConcerned,
                    JackpotDistributionsUpdated = jackpotDistributions
                };
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

        public APICallResult<IEnumerable<JackpotDistribution>> RemovePlayerNeverComeIntoTournamentInProgress(int playerId, ISession session)
        {
            APICallResult<IEnumerable<JackpotDistribution>> result = new APICallResult<IEnumerable<JackpotDistribution>>(false);

            try
            {
                _transactionManager.BeginTransaction();

                session.CanUserPerformAction(_userRepository, BusinessActionResources.EDIT_PLAYER_PRESENCE);

                PlayerDto playerNeverCome = _playerRepository.GetPlayerDtosByPlayerIds(new List<int> { playerId }).Single();

                if (playerNeverCome.Player.PresenceStateCode != PresenceStateResources.PRESENT_CODE)
                {
                    _log.Warn($"Player ID {playerId} is declared with a presence state code {playerNeverCome.Player.PresenceStateCode}. It should not be functionnaly possible to allow this presence state code when player need to be remove from the tournament in progress.");
                    result.WarningMessage = TournamentMessageResources.PLAYER_ALREADY_NOT_PRESENT;
                }

                if (playerNeverCome.Player.TotalReBuy != null)
                {
                    throw new FunctionalException(TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_REBUY_ERROR, null);
                }

                if (playerNeverCome.Player.TotalAddOn != null || playerNeverCome.Player.WasAddOn.GetValueOrDefault())
                {
                    throw new FunctionalException(TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_ADDON_ERROR, null);
                }

                if (playerNeverCome.Player.WasFinalTable.GetValueOrDefault())
                {
                    throw new FunctionalException(TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_FINAL_TABLE_ERROR, null);
                }

                if (playerNeverCome.Player.TotalWinningsPoint != null)
                {
                    throw new FunctionalException(TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_PTS_EARNED_ERROR, null);
                }

                if (playerNeverCome.Player.CurrentTournamentPosition != null)
                {
                    throw new FunctionalException(TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_POSITION_EARNED_ERROR, null);
                }

                if (!playerNeverCome.Tournament.IsInProgress)
                {
                    throw new FunctionalException(TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_IN_PROGRESS_ERROR, null);
                }

                if (playerNeverCome.EliminationsAsEliminator.Any())
                {
                    throw new FunctionalException(TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_ELIMINATOR_ERROR, null);
                }

                if (playerNeverCome.BonusTournamentEarneds.Any())
                {
                    throw new FunctionalException(TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_BONUS_ERROR, null);
                }

                playerNeverCome.Player.PresenceStateCode = PresenceStateResources.ABSENT_CODE;

                _playerRepository.SavePlayer(playerNeverCome.Player);

                IEnumerable<JackpotDistribution> jackpotDistributions = _jackpotDistributionRepository.GetJackpotDistributionsByTournamentId(playerNeverCome.Tournament.Id);

                result.Data = RemoveAmountIntoJackpotDistributions(playerNeverCome.Tournament.BuyIn, jackpotDistributions);

                _transactionManager.CommitTransaction();

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

        public APICallResultBase CancelTournamentInProgress(int tournamentInProgressId, ISession session)
        {
            APICallResultBase result = new APICallResultBase(false);

            try
            {
                _transactionManager.BeginTransaction();

                session.CanUserPerformAction(_userRepository, BusinessActionResources.CANCEL_TOURNAMENT_IN_PROGRESS);

                TournamentToCancelDto tournamentToCancelDto = _tournamentRepository.GetTournamentToCancelDtoByTournamentId(tournamentInProgressId);

                if (!tournamentToCancelDto.TournamentToCancel.IsInProgress)
                {
                    throw new FunctionalException(TournamentBusinessResources.TOURNAMENT_NOT_IN_PROGRESS, null);
                }

                tournamentToCancelDto.TournamentToCancel.IsInProgress = false;

                foreach (Player playerToUpdate in tournamentToCancelDto.PlayersToUpdate)
                {
                    playerToUpdate.TotalWinningsPoint = null;
                    playerToUpdate.CurrentTournamentPosition = null;
                    playerToUpdate.TotalReBuy = null;
                    playerToUpdate.TotalAddOn = null;
                    playerToUpdate.WasFinalTable = null;
                    playerToUpdate.WasAddOn = null;
                    playerToUpdate.TotalWinningsAmount = null;
                }

                _bonusTournamentEarnedRepository.DeleteBonusTournamentEarneds(tournamentToCancelDto.BonusToDelete);
                _eliminationRepository.DeleteEliminations(tournamentToCancelDto.EliminationsToDelete);
                _tournamentRepository.SaveTournament(tournamentToCancelDto.TournamentToCancel);
                _playerRepository.SavePlayers(tournamentToCancelDto.PlayersToUpdate);

                _transactionManager.CommitTransaction();

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

        public APICallResult<AddPlayersResultDto> AddPlayersIntoTournamentInProgress(IEnumerable<int> usrIds, int tournamentId, ISession session)
        {
            APICallResult<AddPlayersResultDto> result = new APICallResult<AddPlayersResultDto>(false);

            try
            {
                _transactionManager.BeginTransaction();

                session.CanUserPerformAction(_userRepository, BusinessActionResources.EDIT_TOURNAMENT_IN_PROGRESS);

                IEnumerable<User> usersToAddIntoTournament = _userRepository.GetUsersByIds(usrIds);
                if (usersToAddIntoTournament.IsNullOrEmpty() || usersToAddIntoTournament.Count() != usrIds.Count())
                {
                    throw new FunctionalException(
                        UserBusinessMessageResources.USER_NO_EXISTS_IN_DB,
                        string.Format(RouteBusinessResources.MAIN_ERROR, UserBusinessMessageResources.USER_NO_EXISTS_IN_DB));
                }

                Tournament tournamentInProgress = _tournamentRepository.GetTournamentById(tournamentId);
                if (!tournamentInProgress.IsInProgress)
                {
                    throw new FunctionalException(
                        TournamentMessageResources.NO_TOURNAMENT_IN_PROGRESS,
                        string.Format(RouteBusinessResources.MAIN_ERROR, TournamentMessageResources.NO_TOURNAMENT_IN_PROGRESS));
                }

                IEnumerable<Player> playersAlreadyAttachedIntoTournament = _playerRepository.GetPlayersByTournamentIdAndUserIds(tournamentId, usrIds);
                foreach (Player player in playersAlreadyAttachedIntoTournament)
                {
                    player.PresenceStateCode = PresenceStateResources.PRESENT_CODE;
                }

                IEnumerable<Player> playersToAddIntoTournament = usersToAddIntoTournament.Where(usr => !playersAlreadyAttachedIntoTournament.Select(pla => pla.UserId).Contains(usr.Id))
                                                                                         .Select(usr => new Player
                                                                                         {
                                                                                             UserId = usr.Id,
                                                                                             PlayedTournamentId = tournamentId,
                                                                                             PresenceStateCode = PresenceStateResources.PRESENT_CODE
                                                                                         });

                IEnumerable<JackpotDistribution> jackpotDistributions = _jackpotDistributionRepository.GetJackpotDistributionsByTournamentId(tournamentInProgress.Id);

                _playerRepository.SavePlayers(playersToAddIntoTournament.Concat(playersAlreadyAttachedIntoTournament));

                result.Data = new AddPlayersResultDto
                {
                    AddedPlayersPlaying = _playerRepository.GetPlayerPlayingDtosByUserIdsAndTournamentId(usrIds, tournamentId),
                    UpdatedJackpotDistributions = AddAmountIntoJackpotDistributions(usrIds.Count() * tournamentInProgress.BuyIn, jackpotDistributions)
                };

                _transactionManager.CommitTransaction();

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

        public APICallResult<TournamentStepEnum> GoToTournamentInProgressNextStep(int tournamentId, ISession session)
        {
            APICallResult<TournamentStepEnum> result = new APICallResult<TournamentStepEnum>(false);

            try
            {
                _transactionManager.BeginTransaction();

                session.CanUserPerformAction(_userRepository, BusinessActionResources.EDIT_TOURNAMENT_IN_PROGRESS);

                TournamentDto tournamentInProgressDto = _tournamentRepository.GetTournamentDtoById(tournamentId);

                if (!tournamentInProgressDto.Tournament.IsInProgress)
                {
                    string errorMsg = TournamentMessageResources.NO_TOURNAMENT_IN_PROGRESS;
                    throw new FunctionalException(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
                }

                IEnumerable<Player> playersPresent = tournamentInProgressDto.Players.Select(pla => pla.Player).Where(pla => pla.PresenceStateCode == PresenceStateResources.PRESENT_CODE);
                TournamentStepEnum currentTournamentStep = GetCurrentTournamentStep(playersPresent);
                List<Player> playersUpdated = new List<Player>();

                foreach (Player player in playersPresent)
                {
                    switch (currentTournamentStep)
                    {
                        case TournamentStepEnum.NORMAL:
                            player.WasAddOn = true;
                            player.TotalAddOn = 0;
                            result.Data = TournamentStepEnum.ADDON;
                            break;
                        case TournamentStepEnum.ADDON:
                            player.WasFinalTable = true;
                            result.Data = TournamentStepEnum.FINAL_TABLE;
                            break;
                    }
                    playersUpdated.Add(player);
                }

                _playerRepository.SavePlayers(playersUpdated);

                _transactionManager.CommitTransaction();

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

        public APICallResult<SwitchTournamentStepResultDto> GoToTournamentInProgressPreviousStep(int tournamentId, ISession session)
        {
            APICallResult<SwitchTournamentStepResultDto> result = new APICallResult<SwitchTournamentStepResultDto>(false);

            try
            {
                _transactionManager.BeginTransaction();

                session.CanUserPerformAction(_userRepository, BusinessActionResources.EDIT_TOURNAMENT_IN_PROGRESS);

                TournamentDto tournamentInProgressDto = _tournamentRepository.GetTournamentDtoById(tournamentId);

                if (!tournamentInProgressDto.Tournament.IsInProgress)
                {
                    string errorMsg = TournamentMessageResources.NO_TOURNAMENT_IN_PROGRESS;
                    throw new FunctionalException(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
                }

                IEnumerable<Player> playersPresent = tournamentInProgressDto.Players.Select(pla => pla.Player).Where(pla => pla.PresenceStateCode == PresenceStateResources.PRESENT_CODE);
                TournamentStepEnum currentTournamentStep = GetCurrentTournamentStep(playersPresent);
                List<Player> playersUpdated = new List<Player>();

                foreach (Player player in playersPresent)
                {
                    switch (currentTournamentStep)
                    {
                        case TournamentStepEnum.FINAL_TABLE:
                            player.WasFinalTable = null;
                            break;
                        case TournamentStepEnum.ADDON:
                            player.WasAddOn = null;
                            player.TotalAddOn = null;
                            break;
                    }
                    playersUpdated.Add(player);
                }

                switch (currentTournamentStep)
                {
                    case TournamentStepEnum.FINAL_TABLE:
                        result.Data = new SwitchTournamentStepResultDto
                        {
                            NewTournamentStep = TournamentStepEnum.ADDON
                        };
                        break;
                    case TournamentStepEnum.ADDON:
                        List<JackpotDistribution> jackpotDistributions = _jackpotDistributionRepository.GetJackpotDistributionsByTournamentId(tournamentInProgressDto.Tournament.Id);
                        int totalJackpot = tournamentInProgressDto.Tournament.BuyIn * (playersPresent.Count()
                                                + playersPresent.Where(pla => pla.TotalReBuy.HasValue && pla.TotalReBuy > 0).Sum(pla => pla.TotalReBuy!.Value)
                                                + playersPresent.Where(pla => pla.TotalAddOn.HasValue && pla.TotalAddOn > 0).Sum(pla => pla.TotalAddOn!.Value))
                                                - 20;
                        JackpotDistribution jackpotDistribution = jackpotDistributions.Single(j => j.PlayerPosition == 1);
                        jackpotDistribution.Amount = totalJackpot;
                        _jackpotDistributionRepository.RemoveJackpotDistributions(jackpotDistributions.Where(j => j.PlayerPosition != 1));
                        List<JackpotDistribution> updatedJackpotDistributions = new List<JackpotDistribution> { jackpotDistribution };
                        _jackpotDistributionRepository.SaveJackpotDistributions(updatedJackpotDistributions);
                        result.Data = new SwitchTournamentStepResultDto
                        {
                            NewTournamentStep = TournamentStepEnum.NORMAL,
                            UpdatedJackpotDistributions = updatedJackpotDistributions
                        };
                        break;
                }

                _playerRepository.SavePlayers(playersUpdated);

                _transactionManager.CommitTransaction();

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

        private TournamentStepEnum GetCurrentTournamentStep(IEnumerable<Player> players)
        {
            if (players.Any(pla => pla.WasFinalTable.GetValueOrDefault() && pla.PresenceStateCode == PresenceStateResources.PRESENT_CODE))
            {
                return TournamentStepEnum.FINAL_TABLE;
            }

            if (players.Any(pla => pla.WasAddOn.GetValueOrDefault() && pla.PresenceStateCode == PresenceStateResources.PRESENT_CODE))
            {
                return TournamentStepEnum.ADDON;
            }

            return TournamentStepEnum.NORMAL;
        }

        public APICallResultBase DeletePlayableTournament(int tournamentIdToDelete, ISession session)
        {
            APICallResultBase result = new APICallResultBase(false);

            try
            {
                _transactionManager.BeginTransaction();

                session.CanUserPerformAction(_userRepository, BusinessActionResources.EDIT_PLAYABLE_TOURNAMENT);

                TournamentDto tournamentToDeleteDto = _tournamentRepository.GetTournamentDtoById(tournamentIdToDelete);

                if (tournamentToDeleteDto.Tournament.IsInProgress || tournamentToDeleteDto.Tournament.IsOver)
                {
                    string errorMsg = "Le tournoi ne peut pas être supprimer car il est soit en cours soit déjà fini.";
                    throw new FunctionalException(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
                }

                _playerRepository.DeletePlayers(tournamentToDeleteDto.Players.Select(pla => pla.Player));
                _tournamentRepository.DeleteTournaments(new List<Tournament> { tournamentToDeleteDto.Tournament });

                _transactionManager.CommitTransaction();

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

        public APICallResult<SeasonResultViewModel> LoadSeasonResult(int season, ISession session)
        {
            APICallResult<SeasonResultViewModel> result = new APICallResult<SeasonResultViewModel>(false);

            try
            {
                result.Data = new SeasonResultViewModel(season);

                session.CanUserPerformAction(_userRepository, BusinessActionResources.COMMON_DASHBOARD);

                SeasonResultDto? seasonResultDto = _tournamentRepository.GetSeasonResultDto(season.ToString());

                result.Data = seasonResultDto != null ? new SeasonResultViewModel(seasonResultDto) : result.Data;

                result.Success = true;
            }
            catch (FunctionalException e)
            {
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                result.ErrorMessage = errorMsg;
                result.RedirectUrl = e.RedirectUrl;
            }
            catch (Exception e)
            {
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                _log.Error(e.Message);
                result.ErrorMessage = errorMsg;
                result.RedirectUrl = string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg);
            }

            return result;
        }

        public APICallResult<IEnumerable<JackpotDistribution>> EditWinnableMoneysByPosition(IDictionary<int, int> winnableMoneysByPosition, int tournamentId, ISession session)
        {
            APICallResult<IEnumerable<JackpotDistribution>> result = new APICallResult<IEnumerable<JackpotDistribution>>(false);

            try
            {
                _transactionManager.BeginTransaction();

                session.CanUserPerformAction(_userRepository, BusinessActionResources.EDIT_WINNABLE_MONEYS);

                Tournament tournament = _tournamentRepository.GetTournamentById(tournamentId);
                if (!tournament.IsInProgress)
                {
                    string errorMsg = TournamentMessageResources.NO_TOURNAMENT_IN_PROGRESS;
                    throw new FunctionalException(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
                }

                IDictionary<int, JackpotDistribution> existingJackpotDistributions = _jackpotDistributionRepository.GetJackpotDistributionsByTournamentId(tournamentId).ToDictionary(j => j.PlayerPosition);

                List<JackpotDistribution> jackpotDistributionsToCreate = new List<JackpotDistribution>();
                List<JackpotDistribution> jackpotDistributionsToUpdate = new List<JackpotDistribution>();
                List<JackpotDistribution> jackpotDistributionsToRemove = new List<JackpotDistribution>();

                foreach (KeyValuePair<int, int> winnableMoneyByPosition in winnableMoneysByPosition)
                {
                    JackpotDistribution? jackpotDistribution;
                    if (existingJackpotDistributions.TryGetValue(winnableMoneyByPosition.Key, out jackpotDistribution))
                    {
                        if (jackpotDistribution.Amount != winnableMoneyByPosition.Value)
                        {
                            jackpotDistribution.Amount = winnableMoneyByPosition.Value;
                        }
                    }
                    else
                    {
                        jackpotDistribution = new JackpotDistribution
                        {
                            Amount = winnableMoneyByPosition.Value,
                            PlayerPosition = winnableMoneyByPosition.Key,
                            TournamentId = tournamentId
                        };
                    }
                    jackpotDistributionsToCreate.Add(jackpotDistribution);
                }

                jackpotDistributionsToRemove.AddRange(existingJackpotDistributions.Where(j => !winnableMoneysByPosition.ContainsKey(j.Key)).Select(j => j.Value));

                _jackpotDistributionRepository.SaveJackpotDistributions(jackpotDistributionsToCreate.Concat(jackpotDistributionsToUpdate));
                _jackpotDistributionRepository.RemoveJackpotDistributions(jackpotDistributionsToRemove);

                _transactionManager.CommitTransaction();

                result.Data = jackpotDistributionsToCreate.Concat(jackpotDistributionsToUpdate);

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
    }
}
