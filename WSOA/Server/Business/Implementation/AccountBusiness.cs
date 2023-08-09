﻿using log4net;
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

        private readonly ILog _log = LogManager.GetLogger(nameof(AccountBusiness));

        public AccountBusiness(IAccountRepository accountRepository, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
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

                User currentUser = _userRepository.GetUserByAccountId(account.Id);
                currentSession.SetString(SessionResources.USER_ID, currentUser.Id.ToString());

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