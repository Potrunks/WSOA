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

        [HttpPost]
        [Route("api/account/signIn")]
        public APICallResult SignIn([FromBody] SignInFormViewModel signInFormVM)
        {
            return _accountBusiness.SignIn(signInFormVM, HttpContext.Session);
        }
    }
}
