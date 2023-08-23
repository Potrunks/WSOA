using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Forms.Components
{
    public class PageFormContainerComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public RenderFragment Fields { get; set; }

        [CascadingParameter(Name = "MaxPages")]
        [EditorRequired]
        public int MaxPages { get; set; }

        [Parameter]
        [EditorRequired]
        public int PageForm { get; set; }

        [CascadingParameter(Name = "CurrentPageDisplay")]
        [EditorRequired]
        public int CurrentPageDisplay { get; set; }

        [CascadingParameter(Name = "ChangePage")]
        [EditorRequired]
        public EventCallback<int> OnPageChange { get; set; }

        [CascadingParameter(Name = "Submit")]
        [EditorRequired]
        public EventCallback OnSubmit { get; set; }

        public void NextPage()
        {
            int newPage = PageForm + 1;
            OnPageChange.InvokeAsync(newPage);
        }

        public void PreviousPage()
        {
            int newPage = PageForm - 1;
            OnPageChange.InvokeAsync(newPage);
        }

        public void Submit()
        {
            OnSubmit.InvokeAsync();
        }
    }
}
