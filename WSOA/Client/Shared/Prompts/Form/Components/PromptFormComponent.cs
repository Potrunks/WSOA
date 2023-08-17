using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Prompts.Form.Components
{
    public class PromptFormComponent : ComponentBase
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
