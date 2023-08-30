using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Forms.Components
{
    public class PagePromptFormComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public bool SuccessListener { get; set; }

        [Parameter]
        [EditorRequired]
        public string? ErrorMessageListener { get; set; }

        [Parameter]
        [EditorRequired]
        public string? WarningMessageListener { get; set; }

        [Parameter]
        [EditorRequired]
        public EventCallback OnExit { get; set; }

        public void Exit()
        {
            OnExit.InvokeAsync();
        }
    }
}
