using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Components
{
    public class SubSectionComponentBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int SubSectionId { get; set; }

        public bool IsLoading { get; set; }
    }
}
