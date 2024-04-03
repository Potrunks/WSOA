using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Services.Interface
{
    public interface IAccountService
    {
        /// <summary>
        /// Call SignIn method from API controller.
        /// </summary>
        Task<APICallResultBase> SignIn(SignInFormViewModel signInFormVM);

        /// <summary>
        /// Call for loading all Invite page datas.
        /// </summary>
        Task<APICallResult<InviteViewModel>> LoadInviteDatas(int subSectionId);

        /// <summary>
        /// Create and send a link account creation for new user.
        /// </summary>
        Task<APICallResultBase> CreateLinkAccountCreation(LinkAccountCreationFormViewModel formViewModel);

        /// <summary>
        /// Create account.
        /// </summary>
        Task<APICallResultBase> CreateAccount(AccountCreationFormViewModel form);

        /// <summary>
        /// Log out the user.
        /// </summary>
        Task<APICallResultBase> LogOut();

        /// <summary>
        /// Clear all token in session.
        /// </summary>
        Task<APICallResultBase> ClearSession();
    }
}
