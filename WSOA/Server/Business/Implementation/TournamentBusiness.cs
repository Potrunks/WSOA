using log4net;
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

        private readonly ILog _log = LogManager.GetLogger(nameof(TournamentBusiness));

        public TournamentBusiness
        (
            ITransactionManager transactionManager,
            IMenuRepository menuRepository,
            ITournamentRepository tournamentRepository,
            IMailService mailService,
            IUserRepository userRepository,
            IAddressRepository addressRepository,
            IPlayerRepository playerRepository
        )
        {
            _transactionManager = transactionManager;
            _menuRepository = menuRepository;
            _tournamentRepository = tournamentRepository;
            _mailService = mailService;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _playerRepository = playerRepository;
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
                CanExecuteTournament(tournamentId);
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

        private void CanExecuteTournament(int tournamentId)
        {
            bool existsTournamentInProgress = _tournamentRepository.ExistsTournamentByIsInProgress(true);
            if (existsTournamentInProgress)
            {
                string errorMsg = TournamentBusinessResources.EXISTS_TOURNAMENT_IN_PROGRESS;
                throw new FunctionalException(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
            }

            bool canExecute = _tournamentRepository.ExistsTournamentByTournamentIdIsOverAndIsInProgress(tournamentId, false, false);
            if (!canExecute)
            {
                string errorMsg = TournamentBusinessResources.CANNOT_EXECUTE_TOURNAMENT;
                throw new FunctionalException(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
            }
        }

        public APICallResultBase SaveTournamentPrepared(TournamentPreparedDto tournamentPrepared, ISession session)
        {
            APICallResultBase result = new APICallResultBase(true);

            try
            {
                // Recup tournoi avec les joueurs
                // Mettre a jour presence joueurs
                // Passer le tournoi en "En cours"
            }
            catch (FunctionalException e)
            {
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                return new APICallResultBase(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                _log.Error(e.Message);
                return new APICallResultBase(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
            }

            return result;
        }
    }
}
