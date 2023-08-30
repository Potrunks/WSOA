using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WSOA.Client.Services.Interface;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Account.Create.Components
{
    public class AccountCreateComponent : ComponentBase
    {
        [Inject]
        public IAccountService AccountService { get; set; }

        public AccountCreationFormViewModel AccountCreationFormVM { get; set; }

        public EditContext EditContext { get; set; }

        public bool IsLoading { get; set; }

        protected override void OnInitialized()
        {
            IsLoading = true;
            AccountCreationFormVM = new AccountCreationFormViewModel();
            EditContext = new EditContext(AccountCreationFormVM);
            EditContext.EnableDataAnnotationsValidation();
            IsLoading = false;
        }

        public Func<Task<APICallResult>> CreateNewAccount()
        {
            return () => AccountService.CreateAccount(AccountCreationFormVM);
        }
    }
}
