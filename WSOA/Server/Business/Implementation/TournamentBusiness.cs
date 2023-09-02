using log4net;
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

        private readonly ILog _log = LogManager.GetLogger(nameof(TournamentBusiness));

        public TournamentBusiness
        (
            ITransactionManager transactionManager,
            IMenuRepository menuRepository,
            ITournamentRepository tournamentRepository,
            IMailService mailService,
            IUserRepository userRepository
        )
        {
            _transactionManager = transactionManager;
            _menuRepository = menuRepository;
            _tournamentRepository = tournamentRepository;
            _mailService = mailService;
            _userRepository = userRepository;
        }

        public APICallResult CreateTournament(TournamentCreationFormViewModel form, ISession session)
        {
            APICallResult result = new APICallResult(null);

            try
            {
                _transactionManager.BeginTransaction();

                // Verifier User connecté
                // Verifier que le User peut faire l'action
                session.CanUserPerformAction(_menuRepository, form.SubSectionId);
                // Valider date de debut
                form.StartDate.IsAfterOrEqualUtcNow();
                // Creer le tournoi si tout va bien
                Tournament newTournament = new Tournament(form);
                _tournamentRepository.SaveTournament(newTournament);
                // Prevenir tous les utilisateur de l'application
                IEnumerable<User> allUsers = _userRepository.GetAllUsers();
                _mailService.SendMails(allUsers.Select(usr => usr.Email), TournamentBusinessResources.MAIL_SUBJECT_NEW_TOURNAMENT, string.Format(TournamentBusinessResources.MAIL_BODY_NEW_TOURNAMENT, form.BaseUri));

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
    }
}
