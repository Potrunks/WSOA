using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WSOA.Shared.Result;

namespace WSOA.Client.Shared.Forms.Components
{
    public class SinglePageFormComponent : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        [EditorRequired]
        public string Title { get; set; }

        [Parameter]
        [EditorRequired]
        public object Model { get; set; }

        [Parameter]
        [EditorRequired]
        public Func<Task<APICallResult>> OnSubmit { get; set; }

        [Parameter]
        [EditorRequired]
        public RenderFragment Fields { get; set; }

        public EditContext EditContext { get; set; }

        public bool IsProcessing { get; set; }

        public bool HaveProcessDone { get; set; }

        public bool IsSuccess { get; set; }

        public string? ErrorMessage { get; set; }

        public string? WarningMessage { get; set; }

        protected override void OnInitialized()
        {
            EditContext = new EditContext(Model);
            EditContext.EnableDataAnnotationsValidation();
        }

        public async Task Submit()
        {
            IsProcessing = true;

            if (EditContext.Validate())
            {
                APICallResult result = await OnSubmit.Invoke();
                if (!string.IsNullOrWhiteSpace(result.RedirectUrl))
                {
                    NavigationManager.NavigateTo(result.RedirectUrl);
                    return;
                }
                IsSuccess = result.Success;
                ErrorMessage = result.ErrorMessage;
                WarningMessage = result.WarningMessage;
            }

            HaveProcessDone = true;
            IsProcessing = false;
        }

        public void Reset()
        {
            HaveProcessDone = false;
            StateHasChanged();
        }
    }
}
