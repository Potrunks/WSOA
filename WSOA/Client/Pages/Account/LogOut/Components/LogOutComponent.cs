using Microsoft.AspNetCore.Components;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Result;

namespace WSOA.Client.Pages.Account.LogOut.Components
{
    public class LogOutComponent : ActionMenuComponentBase
    {
        protected async override Task OnInitializedAsync()
        {
            APICallResult result = await AccountService.LogOut();
            NavigationManager.NavigateTo(result.RedirectUrl);
        }
    }
}
