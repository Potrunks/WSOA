using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Account.Create.Components
{
    public class AccountCreateComponent : ComponentBase
    {
        [Parameter]
        public int LinkId { get; set; }

        public AccountCreationFormViewModel AccountCreationFormVM { get; set; }

        public EditContext EditContext { get; set; }

        public int CurrentPage { get; set; }

        public bool IsProcessing { get; set; }

        public bool IsProcessDone { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public string WarningMessage { get; set; }

        protected override void OnInitialized()
        {
            CurrentPage = 1;
            IsProcessing = false;
            IsProcessDone = false;
            AccountCreationFormVM = new AccountCreationFormViewModel();
            EditContext = new EditContext(AccountCreationFormVM);
            EditContext.EnableDataAnnotationsValidation();
        }

        public void CreateNewAccount()
        {
            CurrentPage = 0;
            IsProcessing = true;

            IsSuccess = false;
            WarningMessage = "Un message d'erreur";

            IsProcessing = false;
            IsProcessDone = true;
        }

        public void NextPageForm()
        {
            CurrentPage++;
        }

        public void PreviousPageForm()
        {
            CurrentPage--;
        }

        public void RetryForm()
        {
            IsProcessDone = false;
            CurrentPage = 1;
        }
    }
}
