using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WSOA.Shared.Result;

namespace WSOA.Client.Shared.Forms.Components
{
    public class MultiPageFormComponent : ComponentBase
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
        public Func<Task<APICallResultBase>> OnSubmit { get; set; }

        [Parameter]
        [EditorRequired]
        public RenderFragment PageForms { get; set; }

        [Parameter]
        [EditorRequired]
        public int MaxPages { get; set; }

        [Parameter]
        public int? HeightPercentage { get; set; }

        public EditContext EditContext { get; set; }

        public bool IsProcessing { get; set; }

        public bool HaveProcessDone { get; set; }

        public bool IsSuccess { get; set; }

        public string? ErrorMessage { get; set; }

        public string? WarningMessage { get; set; }

        public int CurrentPageDisplay { get; set; }

        public EventCallback SubmitCallback => EventCallback.Factory.Create(this, Submit);

        public EventCallback<int> ChangePageCallback => EventCallback.Factory.Create(this, (int newPage) => ChangePage(newPage));

        protected override void OnInitialized()
        {
            CurrentPageDisplay = 1;
            EditContext = new EditContext(Model);
            EditContext.EnableDataAnnotationsValidation();
        }

        private async Task Submit()
        {
            IsProcessing = true;
            IsSuccess = false;
            CurrentPageDisplay = 0;

            if (EditContext.Validate())
            {
                APICallResultBase result = await OnSubmit.Invoke();
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
            CurrentPageDisplay = 1;
        }

        public void ChangePage(int newPage)
        {
            CurrentPageDisplay = newPage;
        }
    }
}
