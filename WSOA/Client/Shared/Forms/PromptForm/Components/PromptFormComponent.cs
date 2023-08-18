using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace WSOA.Client.Shared.Forms.PromptForm.Components
{
    public class PromptFormComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public EventCallback<EditContext> OnSubmit { get; set; }

        [Parameter]
        [EditorRequired]
        public EditContext EditContext { get; set; }

        [Parameter]
        [EditorRequired]
        public RenderFragment Fields { get; set; }

        [Parameter]
        public RenderFragment? AdditionnalButtons { get; set; }

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
