using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Result;

namespace WSOA.Client.Pages.Account.LogOut.Components
{
    public class LogOutComponent : SubSectionComponentBase
    {
        [Inject]
        public IAccountService AccountService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            APICallResultBase result = await AccountService.LogOut();
            NavigationManager.NavigateTo(result.RedirectUrl);
        }
    }
}
