using Microsoft.AspNetCore.Mvc;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Utils;
using WSOA.Shared.Forms;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountBusiness _accountBusiness;

        public AccountController(IAccountBusiness accountBusiness)
        {
            _accountBusiness = accountBusiness;
        }

        /// <summary>
        /// Sign In the user.
        /// </summary>
        [HttpPost]
        [Route("api/account/signIn")]
        public APICallResultBase SignIn([FromBody] SignInFormViewModel signInFormVM)
        {
            return _accountBusiness.SignIn(signInFormVM, HttpContext.Session);
        }

        /// <summary>
        /// Load Invite page datas.
        /// </summary>
        [HttpGet]
        [Route("api/account/invite/{subSectionId}")]
        public APICallResult<InviteViewModel> LoadInviteDatas(int subSectionId)
        {
            return _accountBusiness.LoadInviteDatas(subSectionId, HttpContext.Session);
        }

        /// <summary>
        /// Create and send a link to new user for account creation.
        /// </summary>
        [HttpPost]
        [Route("api/account/invite/createLink")]
        public APICallResultBase CreateLinkAccountCreation([FromBody] LinkAccountCreationFormViewModel formViewModel)
        {
            return _accountBusiness.CreateLinkAccountCreation(formViewModel, HttpContext.Session);
        }

        /// <summary>
        /// Create account.
        /// </summary>
        [HttpPost]
        [Route("api/account/create")]
        public APICallResultBase CreateAccount([FromBody] AccountCreationFormViewModel form)
        {
            return _accountBusiness.CreateAccount(form);
        }

        /// <summary>
        /// Log out the user.
        /// </summary>
        [HttpGet]
        [Route("api/account/logOut")]
        public APICallResultBase LogOut()
        {
            return _accountBusiness.LogOut(HttpContext.Session);
        }

        /// <summary>
        /// Clear all token in session.
        /// </summary>
        [HttpGet]
        [Route("api/account/clearSession")]
        public APICallResultBase ClearSession()
        {
            return _accountBusiness.ClearSession(HttpContext.Session);
        }

        [HttpPost]
        [Route("api/account/reset/send/mail")]
        public APICallResultBase SendResetAccountLoginMail([FromBody] MailForm form)
        {
            return _accountBusiness.SendResetAccountLoginMail(form);
        }

        [HttpPost]
        [Route("api/account/reset/login")]
        public APICallResultBase ResetAccountLogin([FromBody] AccountResetForm form)
        {
            return _accountBusiness.ResetAccountLogin(form);
        }

        [HttpGet]
        [Route("api/account/{accountId}/{forgotPasswordKey}/get")]
        public APICallResult<AccountViewModel> GetResetPasswordAccountViewModel(int accountId, long forgotPasswordKey)
        {
            return _accountBusiness.GetAccountViewModel(accountId, forgotPasswordKey);
        }

        [HttpGet]
        [Route("api/account/get/all")]
        public APICallResult<List<AccountViewModel>> GetAllAccountViewModels()
        {
            return _accountBusiness.GetAllAccountViewModels(HttpContext.Session, HttpContext.Request.GeBasetUrl());
        }
    }
}
