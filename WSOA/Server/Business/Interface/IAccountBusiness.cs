using WSOA.Shared.Forms;
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
        APICallResultBase SignIn(SignInFormViewModel signInFormVM, ISession currentSession);

        /// <summary>
        /// Create a link account creation.
        /// </summary>
        APICallResultBase CreateLinkAccountCreation(LinkAccountCreationFormViewModel link, ISession currentSession);

        /// <summary>
        /// Load all data for the Invite page.
        /// </summary>
        APICallResult<InviteViewModel> LoadInviteDatas(int subSectionId, ISession currentSession);

        /// <summary>
        /// Create a new account.
        /// </summary>
        APICallResultBase CreateAccount(AccountCreationFormViewModel form);

        /// <summary>
        /// Log out the user.
        /// </summary>
        APICallResultBase LogOut(ISession currentSession);

        /// <summary>
        /// Clear all token in session.
        /// </summary>
        APICallResultBase ClearSession(ISession currentSession);

        APICallResultBase SendResetAccountLoginMail(MailForm form);

        APICallResultBase ResetAccountLogin(AccountResetForm form);

        APICallResult<AccountViewModel> GetAccountViewModel(int accountId, long? forgotPasswordKey = null);

        APICallResult<List<AccountViewModel>> GetAllAccountViewModels(ISession session, string baseUrl);
    }
}
