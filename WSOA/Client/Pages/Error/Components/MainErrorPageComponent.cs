using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Pages.Error.Components
{
    public class MainErrorPageComponent : ComponentBase
    {
        [Parameter]
        public string ErrorMessage { get; set; }
    }
}
