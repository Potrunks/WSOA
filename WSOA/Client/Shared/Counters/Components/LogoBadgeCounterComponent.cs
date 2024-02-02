using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Counters.Components
{
    public class LogoBadgeCounterComponent : ComponentBase
    {
        [CascadingParameter(Name = "LogoBadgeCounterValue")]
        public int CounterValue { get; set; }

        [Parameter]
        [EditorRequired]
        public int LogoPixelSize { get; set; }

        [Parameter]
        [EditorRequired]
        public string LogoPath { get; set; }
    }
}
