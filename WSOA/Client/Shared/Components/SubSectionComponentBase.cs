using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Components
{
    public class SubSectionComponentBase : WSOAComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int SubSectionId { get; set; }

        public string Description { get; set; }
    }
}
