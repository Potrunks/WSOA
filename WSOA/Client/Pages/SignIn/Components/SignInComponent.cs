using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WSOA.Client.Services.Interface;
using WSOA.Shared.RenderObject;
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

        public CardRenderObject _cardLeftRenderObject;

        public CardRenderObject _cardRightRenderObject;

        [Parameter]
        public string? ErrorMessage { get; set; }

        public bool _isProcessing = false;

        public bool _isProcessSuccess = false;

        public bool _isLoading = false;

        public EditContext _editContext;

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;

            AccountService.ClearSession();
            _editContext = new EditContext(_signInFormVM);
            _editContext.EnableDataAnnotationsValidation();

            GeneratePokerHand();

            _isLoading = false;
        }

        public async Task SignIn()
        {
            _isProcessing = true;
            _isProcessSuccess = false;
            ErrorMessage = null;

            if (!_editContext.Validate())
            {
                _isProcessing = false;
                return;
            }

            APICallResultBase result = await AccountService.SignIn(_signInFormVM);

            if (!string.IsNullOrWhiteSpace(result.RedirectUrl))
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
            }

            ErrorMessage = result.ErrorMessage;
            _isProcessSuccess = result.Success;
            _isProcessing = false;
        }

        public void GeneratePokerHand()
        {
            _cardLeftRenderObject = new CardRenderObject();
            _cardRightRenderObject = new CardRenderObject(_cardLeftRenderObject);
        }

        public EventCallback GoToForgotLoginPage => EventCallback.Factory.Create(this, () =>
        {
            NavigationManager.NavigateTo("/account/forgot/login");
        });
    }
}
