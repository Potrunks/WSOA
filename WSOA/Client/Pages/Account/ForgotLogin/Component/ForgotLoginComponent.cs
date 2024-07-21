using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Forms;
using WSOA.Shared.Result;

namespace WSOA.Client.Pages.Account.ForgotLogin.Component
{
    public class ForgotLoginComponent : WSOAComponentBase
    {
        [Inject]
        public IAccountService AccountService { get; set; }

        public MailForm Form { get; set; }

        protected override void OnInitialized()
        {
            IsLoading = true;
            Form = new MailForm
            {
                BaseURL = NavigationManager.BaseUri
            };
            IsLoading = false;
        }

        public Func<Task<APICallResultBase>> SendResetAccountLoginMail()
        {
            return () => AccountService.SendResetAccountLoginMail(Form);
        }
    }
}
