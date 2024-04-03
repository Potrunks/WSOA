using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Prompts.Prompt.Components
{
    public class LittlePromptCardComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public string LogoPath { get; set; }

        [Parameter]
        [EditorRequired]
        public string Message { get; set; }

        [Parameter]
        public bool IsErrorMessage { get; set; }
    }
}
