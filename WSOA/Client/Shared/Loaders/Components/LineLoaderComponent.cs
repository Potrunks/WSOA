using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Loaders.Components
{
    public class LineLoaderComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public int Height { get; set; }
    }
}
