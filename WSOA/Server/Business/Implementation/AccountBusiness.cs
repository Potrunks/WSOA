using log4net;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Business.Implementation
{
    public class AccountBusiness : IAccountBusiness
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMenuRepository _menuRepository;

        private readonly ILog _log = LogManager.GetLogger(nameof(AccountBusiness));

        public AccountBusiness
        (
            IAccountRepository accountRepository,
            IUserRepository userRepository,
            IMenuRepository menuRepository
        )
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _menuRepository = menuRepository;
        }

        public APICallResult CreateLinkAccountCreation(LinkAccountCreationViewModel link, ISession currentSession)
        {
            APICallResult result = new APICallResult(RouteBusinessResources.SUCCESS);

            try
            {
                string currentProfileCode = currentSession.GetString(HttpSessionResources.KEY_PROFILE_CODE);
                if (currentProfileCode == null)
                {
                    string errorMsg = MainBusinessResources.USER_NO_CONNECTED;
                    return new APICallResult(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
                }

                MainNavSubSection? subSection = _menuRepository.GetMainNavSubSectionByIdAndProfileCode(currentProfileCode, link.SubSectionIdConcerned);
                if (subSection == null)
                {
                    string errorMsg = MainBusinessResources.USER_CANNOT_PERFORM_ACTION;
                    return new APICallResult(errorMsg, string.Format(RouteBusinessResources.ACCOUNT_INVITE_WITH_ERROR_MESSAGE, errorMsg));
                }

                if (link == null)
                {
                    throw new NullReferenceException(string.Format(MainBusinessResources.NULL_OBJ_NOT_ALLOWED, typeof(LinkAccountCreationViewModel), nameof(AccountBusiness.CreateLinkAccountCreation)));
                }

                LinkAccountCreation newLink = new LinkAccountCreation
                {
                    ProfileCode = link.ProfileCodeSelected,
                    RecipientMail = link.RecipientMail,
                    ExpirationDate = DateTime.UtcNow.AddDays(AccountBusinessResources.LINK_ACCOUNT_CREATION_EXPIRATION_DAY_DELAY)
                };

                _accountRepository.SaveLinkAccountCreation(newLink);
            }
            catch (Exception ex)
            {
                _log.Error(string.Format(AccountBusinessResources.TECHNICAL_ERROR_LINK_ACCOUNT_CREATION, ex.Message));
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                return new APICallResult(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
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
