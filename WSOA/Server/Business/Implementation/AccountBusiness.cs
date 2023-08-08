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

        public AccountBusiness(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public APICallResult SignIn(SignInFormViewModel signInFormVM)
        {
            Account? account = _accountRepository.GetByLoginAndPassword(signInFormVM);

            APICallResult result = new APICallResult
            (
                account != null,
                account == null ? AccountResources.ERROR_SIGN_IN : null
            );

            return result;
        }
    }
}
