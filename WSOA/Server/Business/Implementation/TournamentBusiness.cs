using log4net;
using Microsoft.IdentityModel.Tokens;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Business.Utils;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
using WSOA.Shared.Exceptions;
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

        public APICallResult<FutureTournamentsViewModel> LoadFutureTournamentDatas(int subSectionId, ISession session)
        {
            APICallResult<FutureTournamentsViewModel> result = new APICallResult<FutureTournamentsViewModel>(true);

            try
            {
                MainNavSubSection mainNavSubSection = session.CanUserPerformAction(_menuRepository, subSectionId);
                int currentUserId = session.GetCurrentUserId();
                List<TournamentDto> tournamentDtos = _tournamentRepository.GetTournamentDtosByIsOver(false);
                result.Data = new FutureTournamentsViewModel(tournamentDtos, currentUserId, mainNavSubSection.Description);
            }
            catch (FunctionalException e)
            {
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                return new APICallResult<FutureTournamentsViewModel>(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                return new APICallResult<FutureTournamentsViewModel>(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
            }

            return result;
        }

        public APICallResult<PlayerDataViewModel> SignUpTournament(SignUpTournamentFormViewModel formVM, ISession session)
        {
            APICallResult<PlayerDataViewModel> result = new APICallResult<PlayerDataViewModel>(true);

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

                result.Data = new PlayerDataViewModel(_userRepository.GetUserById(currentUsrId), presenceStateCode);

                _transactionManager.CommitTransaction();
            }
            catch (FunctionalException e)
            {
                _transactionManager.RollbackTransaction();
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                return new APICallResult<PlayerDataViewModel>(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                _transactionManager.RollbackTransaction();
                _log.Error(e.Message);
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                return new APICallResult<PlayerDataViewModel>(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
            }
            return result;
        }

        public APICallResult<PlayableTournamentsViewModel> LoadPlayableTournaments(int subSectionId, ISession session)
        {
            APICallResult<PlayableTournamentsViewModel> result = new APICallResult<PlayableTournamentsViewModel>(true);

            try
            {
                MainNavSubSection subSection = session.CanUserPerformAction(_menuRepository, subSectionId);
                List<TournamentDto> tournamentDtos = _tournamentRepository.GetTournamentDtosByIsOver(false);
                result.Data = new PlayableTournamentsViewModel(tournamentDtos, subSection.Description);
            }
            catch (FunctionalException e)
            {
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                return new APICallResult<PlayableTournamentsViewModel>(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                _log.Error(e.Message);
                return new APICallResult<PlayableTournamentsViewModel>(errorMsg, string.Format(RouteBusinessResources.MAIN_ERROR, errorMsg));
            }

            return result;
        }
    }
}
