using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;

namespace WSOA.Client.Shared.Components
{
    public class ActionMenuComponentBase : ComponentBase
    {
        [Inject]
        public IAccountService AccountService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int SubSectionId { get; set; }

        public bool IsLoading { get; set; }
    }
}
