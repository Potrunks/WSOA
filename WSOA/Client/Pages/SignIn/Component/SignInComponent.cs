using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WSOA.Client.Services.Interface;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.SignIn.Component
{
    public class SignInComponent : ComponentBase
    {
        [Inject]
        public IAccountService AccountService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public SignInFormViewModel _signInFormVM = new SignInFormViewModel();

        public string? _errorMessage = null;

        public bool _isLoading = false;

        public EditContext _editContext;

        protected override async Task OnInitializedAsync()
        {
            _editContext = new EditContext(_signInFormVM);
            _editContext.EnableDataAnnotationsValidation();
        }

        public async Task SignIn()
        {
            _isLoading = true;
            _errorMessage = null;

            if (!_editContext.Validate())
            {
                _isLoading = false;
                return;
            }

            APICallResult result = await AccountService.SignIn(_signInFormVM);
            _errorMessage = result.Success ? null : result.ErrorMessage;

            _isLoading = false;

            // Redirection vers Home
            NavigationManager.NavigateTo("/home");
        }
    }
}
