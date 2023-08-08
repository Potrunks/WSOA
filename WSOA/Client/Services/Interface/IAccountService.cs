using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Services.Interface
{
    public interface IAccountService
    {
        /// <summary>
        /// Call SignIn method from API controller.
        /// </summary>
        Task<APICallResult> SignIn(SignInFormViewModel signInFormVM);
    }
}
