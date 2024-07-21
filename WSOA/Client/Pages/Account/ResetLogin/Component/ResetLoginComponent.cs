using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Forms;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Account.ResetLogin.Component
{
    public class ResetLoginComponent : WSOAComponentBase
    {
        [Inject]
        public IAccountService AccountService { get; set; }

        [Parameter]
        public int AccountId { get; set; }

        [Parameter]
        public long ForgotPasswordKey { get; set; }

        public AccountViewModel AccountViewModel { get; set; }

        public AccountResetForm AccountResetForm { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            APICallResult<AccountViewModel> result = await AccountService.GetResetPasswordAccountViewModel(AccountId, ForgotPasswordKey);

            if (!result.Success)
            {
                string redirectURL = !string.IsNullOrWhiteSpace(result.RedirectUrl) ? result.RedirectUrl : string.Format("/signIn/error/{0}", result.ErrorMessage);
                NavigationManager.NavigateTo(redirectURL);
                return;
            }

            AccountViewModel = result.Data;
            AccountResetForm = new AccountResetForm
            {
                AccountId = AccountId,
                ForgotPasswordKey = ForgotPasswordKey
            };

            IsLoading = false;
        }

        public Func<Task<APICallResultBase>> ResetAccountLogin()
        {
            return () => AccountService.ResetAccountLogin(AccountResetForm);
        }
    }
}
