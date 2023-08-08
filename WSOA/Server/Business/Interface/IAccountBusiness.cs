using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Business.Interface
{
    public interface IAccountBusiness
    {
        /// <summary>
        /// Sign In the user.
        /// </summary>
        /// <returns>Status of the sign in attemption.</returns>
        APICallResult SignIn(SignInFormViewModel signInFormVM);
    }
}
