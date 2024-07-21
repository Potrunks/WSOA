using log4net;
using Microsoft.IdentityModel.Tokens;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Business.Utils;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;
using WSOA.Shared.Exceptions;
using WSOA.Shared.Forms;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;
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

        public APICallResultBase CreateAccount(AccountCreationFormViewModel form)
        {
            APICallResultBase result = new APICallResultBase(true, RouteBusinessResources.SIGN_IN);

            try
            {
                _transactionManager.BeginTransaction();

                LinkAccountCreation? link = _accountRepository.GetLinkAccountCreationByMail(form.Email);
                if (link == null || link.ExpirationDate < DateTime.UtcNow)
                {
                    _transactionManager.RollbackTransaction();
                    return new APICallResultBase(AccountBusinessResources.LINK_ACCOUNT_CREATION_NOT_EXIST_OR_EXPIRED);
                }

                if (_accountRepository.ExistsAccountByLogin(form.Login))
                {
                    _transactionManager.RollbackTransaction();
                    string errorMsg = AccountBusinessResources.LOGIN_ALREADY_EXISTS;
                    return new APICallResultBase(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
                }

                if (_userRepository.ExistsUserByMail(form.Email))
                {
                    _transactionManager.RollbackTransaction();
                    string errorMsg = UserBusinessResources.MAIL_ALREADY_EXISTS;
                    return new APICallResultBase(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
                }

                Account newAccount = new Account
                {
                    Login = form.Login,
                    Password = form.Password.ToSha256()
                };
                _accountRepository.SaveAccount(newAccount);

                User newUser = new User
                {
                    AccountId = newAccount.Id,
                    Email = form.Email,
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    ProfileCode = link.ProfileCode
                };
                _userRepository.SaveUser(newUser);

                _accountRepository.DeleteLinkAccountCreation(link);

                _transactionManager.CommitTransaction();
            }
            catch (Exception ex)
            {
                _transactionManager.RollbackTransaction();
                _log.Error(string.Format(AccountBusinessResources.TECHNICAL_ERROR_ACCOUNT_CREATION, ex.Message));
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                return new APICallResultBase(errorMsg);
            }

            return result;
        }

        public APICallResultBase CreateLinkAccountCreation(LinkAccountCreationFormViewModel link, ISession currentSession)
        {
            APICallResultBase result = new APICallResultBase(true);

            try
            {
                _transactionManager.BeginTransaction();

                string currentProfileCode = currentSession.GetString(HttpSessionResources.KEY_PROFILE_CODE);
                if (currentProfileCode == null)
                {
                    _transactionManager.RollbackTransaction();
                    string errorMsg = MainBusinessResources.USER_NOT_CONNECTED;
                    return new APICallResultBase(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
                }

                MainNavSubSection? subSection = _menuRepository.GetMainNavSubSectionByIdAndProfileCode(currentProfileCode, link.SubSectionIdConcerned);
                if (subSection == null)
                {
                    _transactionManager.RollbackTransaction();
                    return new APICallResultBase(MainBusinessResources.USER_CANNOT_PERFORM_ACTION);
                }

                if (_userRepository.ExistsUserByMail(link.RecipientMail))
                {
                    _transactionManager.RollbackTransaction();
                    return new APICallResultBase(UserBusinessResources.MAIL_ALREADY_EXISTS);
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

                _transactionManager.CommitTransaction();

                _mailService.SendMailAccountCreationLink(currentLink.RecipientMail, link.BaseUri);
            }
            catch (WarningException e)
            {
                _log.Error(string.Format(AccountBusinessResources.TECHNICAL_ERROR_LINK_ACCOUNT_CREATION, e.Message));
                result.WarningMessage = e.Message;
            }
            catch (Exception ex)
            {
                _transactionManager.RollbackTransaction();
                _log.Error(string.Format(AccountBusinessResources.TECHNICAL_ERROR_LINK_ACCOUNT_CREATION, ex.Message));
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                return new APICallResultBase(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
            }

            return result;
        }

        public APICallResult<InviteViewModel> LoadInviteDatas(int subSectionId, ISession currentSession)
        {
            APICallResult<InviteViewModel> result = new APICallResult<InviteViewModel>(true);

            try
            {
                string currentProfileCode = currentSession.GetString(HttpSessionResources.KEY_PROFILE_CODE);
                if (currentProfileCode == null)
                {
                    string errorMsg = MainBusinessResources.USER_NOT_CONNECTED;
                    return new APICallResult<InviteViewModel>(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
                }

                MainNavSubSection? subSection = _menuRepository.GetMainNavSubSectionByIdAndProfileCode(currentProfileCode, subSectionId);
                if (subSection == null)
                {
                    return new APICallResult<InviteViewModel>(MainBusinessResources.USER_CANNOT_PERFORM_ACTION);
                }

                Dictionary<string, string> allProfileNames = _userRepository.GetAllProfiles().ToDictionary(p => p.Code, p => p.Name);
                if (allProfileNames.IsNullOrEmpty())
                {
                    throw new Exception(string.Format(MainBusinessResources.NULL_OR_EMPTY_OBJ_NOT_ALLOWED, nameof(allProfileNames), nameof(AccountBusiness.LoadInviteDatas)));
                }

                result.Data = new InviteViewModel(allProfileNames, subSection.Description);
            }
            catch (Exception exception)
            {
                _log.Error(string.Format(AccountBusinessResources.TECHNICAL_ERROR_INVITE_PAGE_LOADING, exception.Message));
                return new APICallResult<InviteViewModel>(MainBusinessResources.TECHNICAL_ERROR);
            }

            return result;
        }

        public APICallResultBase LogOut(ISession currentSession)
        {
            APICallResultBase result = new APICallResultBase(true, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, AccountBusinessResources.LOG_OUT));

            ClearSession(currentSession);

            return result;
        }

        public APICallResultBase ClearSession(ISession currentSession)
        {
            APICallResultBase result = new APICallResultBase(true);

            if (!currentSession.Keys.IsNullOrEmpty())
            {
                currentSession.Clear();
            }

            return result;
        }

        public APICallResultBase SignIn(SignInFormViewModel signInFormVM, ISession currentSession)
        {
            try
            {
                if (signInFormVM == null)
                {
                    throw new NullReferenceException(string.Format(MainBusinessResources.NULL_OBJ_NOT_ALLOWED, typeof(SignInFormViewModel), nameof(AccountBusiness.SignIn)));
                }

                Account? account = _accountRepository.GetByLoginAndPassword(signInFormVM.Login, signInFormVM.Password.ToSha256());
                if (account == null)
                {
                    return new APICallResultBase(AccountBusinessResources.ERROR_SIGN_IN);
                }

                User currentUser = _userRepository.GetUserByAccountId(account.Id);
                currentSession.SetString(HttpSessionResources.KEY_USER_ID, currentUser.Id.ToString());
                currentSession.SetString(HttpSessionResources.KEY_PROFILE_CODE, currentUser.ProfileCode);

            }
            catch (Exception ex)
            {
                _log.Error(string.Format(AccountBusinessResources.TECHNICAL_ERROR_SIGN_IN, ex.Message));
                return new APICallResultBase(MainBusinessResources.TECHNICAL_ERROR);
            }

            return new APICallResultBase(true, RouteBusinessResources.HOME);
        }

        public APICallResultBase SendResetAccountLoginMail(MailForm form)
        {
            APICallResultBase result = new APICallResultBase(true);

            try
            {
                _transactionManager.BeginTransaction();

                if (string.IsNullOrWhiteSpace(form.Mail))
                {
                    throw new NullReferenceException(string.Format(MainBusinessResources.NULL_OBJ_NOT_ALLOWED, typeof(string), nameof(AccountBusiness.SendResetAccountLoginMail)));
                }

                Account? account = _accountRepository.GetAccountsByMails(new List<string> { form.Mail }).SingleOrDefault();
                if (account == null)
                {
                    string errorMsg = AccountBusinessResources.ACCOUNT_NOT_EXISTS;
                    throw new FunctionalException(errorMsg, string.Format("/signIn/error/{0}", errorMsg));
                }

                if (!account.ForgotPasswordExpirationDate.HasValue || account.ForgotPasswordExpirationDate < DateTime.UtcNow)
                {
                    account.ForgotPasswordExpirationDate = DateTime.UtcNow.AddHours(AccountBusinessResources.ACCOUNT_RESET_LOGIN_EXPIRATION_HOUR);
                    account.ForgotPasswordKey = account.ForgotPasswordExpirationDate.Value.Ticks;
                }

                _accountRepository.SaveAccount(account);

                _transactionManager.CommitTransaction();

                _mailService.SendResetAccountLoginMail(form.Mail, account, form.BaseURL);
            }
            catch (WarningException e)
            {
                _log.Error(string.Format(AccountBusinessResources.TECHNICAL_ERROR_SEND_MAIL_RESET_PWD, e.Message));
                result.WarningMessage = e.Message;
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

        public APICallResultBase ResetAccountLogin(AccountResetForm form)
        {
            try
            {
                _transactionManager.BeginTransaction();

                APICallResultBase result = new APICallResultBase(true);

                Account? account = _accountRepository.GetAccountsByIds(new List<int> { form.AccountId }).SingleOrDefault();

                if (account == null)
                {
                    string errorMsg = AccountBusinessResources.ACCOUNT_NOT_EXISTS;
                    throw new FunctionalException(errorMsg, string.Format("/signIn/error/{0}", errorMsg));
                }

                if (account.ForgotPasswordExpirationDate < DateTime.UtcNow || account.ForgotPasswordKey != form.ForgotPasswordKey)
                {
                    string errorMsg = AccountBusinessResources.RESET_PWD_LINK_EXPIRED;
                    throw new FunctionalException(errorMsg, string.Format("/signIn/error/{0}", errorMsg));
                }

                account.Password = form.Password.ToSha256();
                account.ForgotPasswordExpirationDate = null;
                account.ForgotPasswordKey = null;

                _accountRepository.SaveAccount(account);

                _transactionManager.CommitTransaction();

                return result;
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
        }

        public APICallResult<AccountViewModel> GetAccountViewModel(int accountId, long? forgotPasswordKey = null)
        {
            try
            {
                APICallResult<AccountViewModel> result = new APICallResult<AccountViewModel>(true);

                Account? account = _accountRepository.GetAccountsByIds(new List<int> { accountId }).SingleOrDefault();

                if (account == null)
                {
                    string errorMsg = AccountBusinessResources.ACCOUNT_NOT_EXISTS;
                    throw new FunctionalException(errorMsg, string.Format("/signIn/error/{0}", errorMsg));
                }

                if (forgotPasswordKey != null && (account.ForgotPasswordExpirationDate < DateTime.UtcNow || account.ForgotPasswordKey != forgotPasswordKey))
                {
                    string errorMsg = AccountBusinessResources.RESET_PWD_LINK_EXPIRED;
                    throw new FunctionalException(errorMsg, string.Format("/signIn/error/{0}", errorMsg));
                }

                result.Data = new AccountViewModel
                {
                    Login = account.Login
                };

                return result;
            }
            catch (FunctionalException e)
            {
                string errorMsg = e.Message;
                _log.Error(errorMsg);
                return new APICallResult<AccountViewModel>(errorMsg, e.RedirectUrl);
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                return new APICallResult<AccountViewModel>(MainBusinessResources.TECHNICAL_ERROR);
            }
        }

        public APICallResult<List<AccountViewModel>> GetAllAccountViewModels(ISession session, string baseUrl)
        {
            APICallResult<List<AccountViewModel>> result = new APICallResult<List<AccountViewModel>>(true);

            try
            {
                session.CanUserPerformAction(_menuRepository, MainNavSubSectionResources.GET_ALL_ACCOUNTS_ID);

                result.Data = _accountRepository.GetAllAccountDtos().Select(acc => new AccountViewModel(acc, baseUrl)).ToList();
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                return new APICallResult<List<AccountViewModel>>(MainBusinessResources.TECHNICAL_ERROR);
            }

            return result;
        }
    }
}
