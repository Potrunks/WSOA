using Microsoft.AspNetCore.Mvc;
using WSOA.Server.Business.Interface;
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
        public APICallResult SignIn([FromBody] SignInFormViewModel signInFormVM)
        {
            return _accountBusiness.SignIn(signInFormVM, HttpContext.Session);
        }

        /// <summary>
        /// Load Invite page datas.
        /// </summary>
        [HttpGet]
        [Route("api/account/invite/{subSectionId}")]
        public InviteCallResult LoadInviteDatas(int subSectionId)
        {
            return _accountBusiness.LoadInviteDatas(subSectionId, HttpContext.Session);
        }

        /// <summary>
        /// Create and send a link to new user for account creation.
        /// </summary>
        [HttpPost]
        [Route("api/account/invite/createLink")]
        public APICallResult CreateLinkAccountCreation([FromBody] LinkAccountCreationFormViewModel formViewModel)
        {
            return _accountBusiness.CreateLinkAccountCreation(formViewModel, HttpContext.Session);
        }
    }
}
