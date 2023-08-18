using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Prompts.Prompt.Components
{
    public class PromptComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public bool IsProcessingListener { get; set; }

        [Parameter]
        [EditorRequired]
        public bool IsProcessSuccessListener { get; set; }

        [Parameter]
        [EditorRequired]
        public string ErrorMessageListerner { get; set; }

        [Parameter]
        public string? WarningMessageListerner { get; set; }
    }
}
