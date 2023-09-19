using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Prompts.Prompt.Components
{
    public class LittlePromptCardComponent : ComponentBase
    {
        [Parameter]
        public string LogoPath { get; set; }

        [Parameter]
        public string Message { get; set; }
    }
}
