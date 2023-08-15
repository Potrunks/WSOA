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

        public bool _isLoading = false;

        public EditContext _editContext;

        protected override async Task OnInitializedAsync()
        {
            _editContext = new EditContext(_signInFormVM);
            _editContext.EnableDataAnnotationsValidation();

            GeneratePokerHand();
        }

        public async Task SignIn()
        {
            _isLoading = true;
            ErrorMessage = null;

            if (!_editContext.Validate())
            {
                _isLoading = false;
                return;
            }

            APICallResult result = await AccountService.SignIn(_signInFormVM);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                _isLoading = false;
                return;
            }

            _isLoading = false;

            NavigationManager.NavigateTo(result.RedirectUrl);
        }

        public void GeneratePokerHand()
        {
            _cardLeftRenderObject = new CardRenderObject();
            _cardRightRenderObject = new CardRenderObject(_cardLeftRenderObject);
        }
    }
}
