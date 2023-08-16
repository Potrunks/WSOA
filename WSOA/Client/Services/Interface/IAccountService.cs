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

        /// <summary>
        /// Call for loading all Invite page datas.
        /// </summary>
        Task<InviteCallResult> LoadInviteDatas(int subSectionId);

        /// <summary>
        /// Create and send a link account creation for new user.
        /// </summary>
        Task<APICallResult> CreateLinkAccountCreation(LinkAccountCreationFormViewModel formViewModel);
    }
}
