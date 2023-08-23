using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Counters.Components
{
    public class PageFormCounterComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public int MaxPages { get; set; }

        [Parameter]
        [EditorRequired]
        public int CurrentPageListener { get; set; }

        public string IsSelected(int pageCounter)
        {
            return CurrentPageListener >= pageCounter ? "selected" : "";
        }
    }
}
