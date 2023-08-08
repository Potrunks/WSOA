using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.SignIn.Component
{
    public class SignInComponent : ComponentBase
    {
        [Inject]
        public IAccountService AccountService { get; set; }

        public SignInFormViewModel _signInFormVM = new SignInFormViewModel();

        public string _isSignInResult;

        public async Task SignIn()
        {
            APICallResult result = await AccountService.SignIn(_signInFormVM);
            _isSignInResult = result.Success ? "OK" : "Pas OK";
        }
    }
}
