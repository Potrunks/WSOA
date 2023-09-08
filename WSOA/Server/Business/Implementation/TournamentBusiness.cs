using log4net;
using Microsoft.IdentityModel.Tokens;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Business.Utils;
using WSOA.Server.Data.Interface;
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

        private readonly ILog _log = LogManager.GetLogger(nameof(TournamentBusiness));

        public TournamentBusiness
        (
            ITransactionManager transactionManager,
            IMenuRepository menuRepository,
            ITournamentRepository tournamentRepository,
            IMailService mailService,
            IUserRepository userRepository,
            IAddressRepository addressRepository
        )
        {
            _transactionManager = transactionManager;
            _menuRepository = menuRepository;
            _tournamentRepository = tournamentRepository;
            _mailService = mailService;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
        }

        public APICallResult CreateTournament(TournamentCreationFormViewModel form, ISession session)
        {
            APICallResult result = new APICallResult(null);

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
                return new APICallResult(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                _transactionManager.RollbackTransaction();
                _log.Error(e.Message);
                return new APICallResult(MainBusinessResources.TECHNICAL_ERROR, null);
            }

            return result;
        }

        public CreateTournamentCallResult LoadTournamentCreationDatas(int subSectionId, ISession session)
        {
            CreateTournamentCallResult result = new CreateTournamentCallResult(null);

            try
            {
                MainNavSubSection subSection = session.CanUserPerformAction(_menuRepository, subSectionId);

                IEnumerable<Address> addresses = _addressRepository.GetAllAddresses();
                if (addresses.IsNullOrEmpty())
                {
                    string errorMsg = string.Format(MainBusinessResources.NULL_OR_EMPTY_OBJ_NOT_ALLOWED, nameof(addresses), nameof(LoadTournamentCreationDatas));
                    throw new FunctionalException(errorMsg, string.Format(RouteBusinessResources.ERROR, errorMsg));
                }

                result.Data = new TournamentCreationDataViewModel(addresses, subSection.Description);
            }
            catch (FunctionalException e)
            {
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                return new CreateTournamentCallResult(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                return new CreateTournamentCallResult(errorMsg, string.Format(RouteBusinessResources.ERROR, errorMsg));
            }

            return result;
        }

        public LoadFutureTournamentCallResult LoadFutureTournamentDatas(int subSectionId, ISession session)
        {
            LoadFutureTournamentCallResult result = new LoadFutureTournamentCallResult();

            try
            {
                session.CanUserPerformAction(_menuRepository, subSectionId);
                // TODO : Charger les données
            }
            catch (FunctionalException e)
            {
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                return new LoadFutureTournamentCallResult(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                return new LoadFutureTournamentCallResult(errorMsg, string.Format(RouteBusinessResources.ERROR, errorMsg));
            }

            return result;
        }
    }
}
