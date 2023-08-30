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
        APICallResult SignIn(SignInFormViewModel signInFormVM, ISession currentSession);

        /// <summary>
        /// Create a link account creation.
        /// </summary>
        APICallResult CreateLinkAccountCreation(LinkAccountCreationFormViewModel link, ISession currentSession);

        /// <summary>
        /// Load all data for the Invite page.
        /// </summary>
        InviteCallResult LoadInviteDatas(int subSectionId, ISession currentSession);

        /// <summary>
        /// Create a new account.
        /// </summary>
        APICallResult CreateAccount(AccountCreationFormViewModel form);

        /// <summary>
        /// Log out the user.
        /// </summary>
        APICallResult LogOut(ISession currentSession);
    }
}
