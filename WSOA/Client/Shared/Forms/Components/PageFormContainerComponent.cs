using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Forms.Components
{
    public class PageFormContainerComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public RenderFragment Fields { get; set; }

        [Parameter]
        [EditorRequired]
        public RenderFragment Buttons { get; set; }
    }
}
