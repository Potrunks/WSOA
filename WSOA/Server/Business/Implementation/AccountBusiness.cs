using log4net;
using Microsoft.IdentityModel.Tokens;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;
using WSOA.Shared.Result;
using WSOA.Shared.Utils;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Business.Implementation
{
    public class AccountBusiness : IAccountBusiness
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly IMailService _mailService;

        private readonly ILog _log = LogManager.GetLogger(nameof(AccountBusiness));

        public AccountBusiness
        (
            IAccountRepository accountRepository,
            IUserRepository userRepository,
            IMenuRepository menuRepository,
            ITransactionManager transactionManager,
            IMailService mailService
        )
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _menuRepository = menuRepository;
            _transactionManager = transactionManager;
            _mailService = mailService;
        }

        public APICallResult CreateLinkAccountCreation(LinkAccountCreationFormViewModel link, ISession currentSession)
        {
            APICallResult result = new APICallResult(null);

            try
            {
                _transactionManager.BeginTransaction();

                string currentProfileCode = currentSession.GetString(HttpSessionResources.KEY_PROFILE_CODE);
                if (currentProfileCode == null)
                {
                    string errorMsg = MainBusinessResources.USER_NOT_CONNECTED;
                    return new APICallResult(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
                }

                MainNavSubSection? subSection = _menuRepository.GetMainNavSubSectionByIdAndProfileCode(currentProfileCode, link.SubSectionIdConcerned);
                if (subSection == null)
                {
                    return new APICallResult(MainBusinessResources.USER_CANNOT_PERFORM_ACTION, null);
                }

                if (!link.RecipientMail.IsValidMailFormat())
                {
                    return new APICallResult(MainBusinessResources.MAIL_FORMAT_NO_VALID, null);
                }

                LinkAccountCreation currentLink = _accountRepository.GetLinkAccountCreationByMail(link.RecipientMail);
                if (currentLink == null)
                {
                    currentLink = new LinkAccountCreation
                    {
                        ProfileCode = link.ProfileCodeSelected,
                        RecipientMail = link.RecipientMail,
                        ExpirationDate = DateTime.UtcNow.AddDays(AccountBusinessResources.LINK_ACCOUNT_CREATION_EXPIRATION_DAY_DELAY)
                    };
                }
                else
                {
                    currentLink.ProfileCode = link.ProfileCodeSelected != currentLink.ProfileCode ? link.ProfileCodeSelected : currentLink.ProfileCode;
                    if (currentLink.ExpirationDate < DateTime.UtcNow)
                    {
                        currentLink.ExpirationDate = DateTime.UtcNow.AddDays(AccountBusinessResources.LINK_ACCOUNT_CREATION_EXPIRATION_DAY_DELAY);
                        result.WarningMessage = AccountBusinessResources.LINK_ACCOUNT_CREATION_EXTENDED;
                    }
                    else
                    {
                        result.WarningMessage = AccountBusinessResources.LINK_ACCOUNT_CREATION_RE_SEND;
                    }
                }

                _accountRepository.SaveLinkAccountCreation(currentLink);

                _mailService.SendMailAccountCreation(currentLink);

                _transactionManager.CommitTransaction();
            }
            catch (Exception ex)
            {
                _transactionManager.RollbackTransaction();
                _log.Error(string.Format(AccountBusinessResources.TECHNICAL_ERROR_LINK_ACCOUNT_CREATION, ex.Message));
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                return new APICallResult(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
            }

            return result;
        }

        public InviteCallResult LoadInviteDatas(int subSectionId, ISession currentSession)
        {
            InviteCallResult result = new InviteCallResult();

            try
            {
                string currentProfileCode = currentSession.GetString(HttpSessionResources.KEY_PROFILE_CODE);
                if (currentProfileCode == null)
                {
                    string errorMsg = MainBusinessResources.USER_NOT_CONNECTED;
                    return new InviteCallResult(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
                }

                MainNavSubSection? subSection = _menuRepository.GetMainNavSubSectionByIdAndProfileCode(currentProfileCode, subSectionId);
                if (subSection == null)
                {
                    return new InviteCallResult(MainBusinessResources.USER_CANNOT_PERFORM_ACTION, null);
                }

                Dictionary<string, string> allProfileNames = _userRepository.GetAllProfiles().ToDictionary(p => p.Code, p => p.Name);
                if (allProfileNames.IsNullOrEmpty())
                {
                    throw new Exception(string.Format(MainBusinessResources.NULL_OR_EMPTY_OBJ_NOT_ALLOWED, nameof(allProfileNames), nameof(AccountBusiness.LoadInviteDatas)));
                }

                result.InviteVM.ProfileLabelsByCode = allProfileNames;
                result.Success = true;
            }
            catch (Exception exception)
            {
                _log.Error(string.Format(AccountBusinessResources.TECHNICAL_ERROR_INVITE_PAGE_LOADING, exception.Message));
                return new InviteCallResult(MainBusinessResources.TECHNICAL_ERROR, null);
            }

            return result;
        }

        public APICallResult SignIn(SignInFormViewModel signInFormVM, ISession currentSession)
        {
            try
            {
                if (signInFormVM == null)
                {
                    throw new NullReferenceException(string.Format(MainBusinessResources.NULL_OBJ_NOT_ALLOWED, typeof(SignInFormViewModel), nameof(AccountBusiness.SignIn)));
                }

                Account? account = _accountRepository.GetByLoginAndPassword(signInFormVM);
                if (account == null)
                {
                    return new APICallResult(AccountBusinessResources.ERROR_SIGN_IN, null);
                }

                User currentUser = _userRepository.GetUserByAccountId(account.Id);
                currentSession.SetString(HttpSessionResources.KEY_USER_ID, currentUser.Id.ToString());
                currentSession.SetString(HttpSessionResources.KEY_PROFILE_CODE, currentUser.ProfileCode);

            }
            catch (Exception ex)
            {
                _log.Error(string.Format(AccountBusinessResources.TECHNICAL_ERROR_SIGN_IN, ex.Message));
                return new APICallResult(MainBusinessResources.TECHNICAL_ERROR, null);
            }

            return new APICallResult(RouteBusinessResources.HOME);
        }
    }
}
