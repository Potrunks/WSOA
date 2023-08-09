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
        private readonly ILog _log = LogManager.GetLogger(nameof(AccountBusiness));

        public AccountBusiness(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public APICallResult SignIn(SignInFormViewModel signInFormVM, ISession currentSession)
        {
            APICallResult result = new APICallResult();

            try
            {
                if (signInFormVM == null)
                {
                    throw new NullReferenceException(string.Format(MainResources.NULL_OBJ_NOT_ALLOWED, typeof(SignInFormViewModel), nameof(AccountBusiness.SignIn)));
                }

                if (string.IsNullOrWhiteSpace(signInFormVM.Login) || string.IsNullOrWhiteSpace(signInFormVM.Password))
                {
                    result.Success = false;
                    result.ErrorMessage = AccountResources.LOGIN_PWD_MISSING;
                    return result;
                }

                Account? account = _accountRepository.GetByLoginAndPassword(signInFormVM);
                if (account == null)
                {
                    result.Success = false;
                    result.ErrorMessage = AccountResources.ERROR_SIGN_IN;
                    return result;
                }

                // Si connexion OK, ajouter l'utilisateur dans la session s'il ne l'est pas deja
                if (string.IsNullOrWhiteSpace(currentSession.GetString(SessionResources.USER_ID)))
                {

                }

            }
            catch (Exception ex)
            {
                _log.Error(string.Format(AccountResources.TECHNICAL_ERROR_SIGN_IN, ex.Message));
                result.Success = false;
                result.ErrorMessage = MainResources.TECHNICAL_ERROR;
            }

            return result;
        }
    }
}
