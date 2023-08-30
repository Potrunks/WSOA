using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Shared.Result;

namespace WSOA.Client.Pages.Account.LogOut.Components
{
    public class LogOutComponent : ComponentBase
    {
        [Inject]
        public IAccountService AccountService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int SubSectionId { get; set; }

        protected async override Task OnInitializedAsync()
        {
            APICallResult result = await AccountService.LogOut();
            NavigationManager.NavigateTo(result.RedirectUrl);
        }
    }
}
