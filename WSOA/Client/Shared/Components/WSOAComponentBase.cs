using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Components
{
    public class WSOAComponentBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public bool IsLoading { get; set; }
    }
}
