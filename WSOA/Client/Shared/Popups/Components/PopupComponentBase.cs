using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WSOA.Client.Shared.EventHandlers;

namespace WSOA.Client.Shared.Popups.Components
{
    public class PopupComponentBase : ComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [CascadingParameter(Name = "PopupEventHandler")]
        [EditorRequired]
        public PopupEventHandler PopupEventHandler { get; set; }

        [Parameter]
        [EditorRequired]
        public string Key { get; set; }

        public bool IsDisplay { get; set; } = false;

        public EventCallback ClosePopup => EventCallback.Factory.Create(this, OnClosePopup);

        protected override void OnInitialized()
        {
            PopupEventHandler.OnPopupOpen += async (obj, currentPopupOpen) =>
            {
                if (IsDisplay && currentPopupOpen.Key != Key)
                {
                    IsDisplay = false;
                    StateHasChanged();
                    await JSRuntime.InvokeVoidAsync("blurring", "app_layout");
                }

                if (!IsDisplay && currentPopupOpen.Key == Key)
                {
                    IsDisplay = true;
                    StateHasChanged();
                    await JSRuntime.InvokeVoidAsync("blurring", "app_layout");
                }
            };

            PopupEventHandler.OnPopupClose += async (obj, currentPopupOpen) =>
            {
                if (IsDisplay)
                {
                    IsDisplay = false;
                    StateHasChanged();
                    await JSRuntime.InvokeVoidAsync("blurring", "app_layout");
                }
            };
        }

        private void OnClosePopup()
        {
            PopupEventHandler.Close();
        }
    }
}
