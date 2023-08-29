using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Account.Create.Components
{
    public class AccountCreateComponent : ComponentBase
    {
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
            return () => Task.Run(() => { return new APICallResult(null); });
        }
    }
}
