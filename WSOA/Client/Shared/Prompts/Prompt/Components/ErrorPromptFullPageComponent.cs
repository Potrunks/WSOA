using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Prompts.Prompt.Components
{
    public class ErrorPromptFullPageComponent : ComponentBase
    {
        [Parameter]
        public string ErrorMessage { get; set; }
    }
}
