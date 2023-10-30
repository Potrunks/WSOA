using Microsoft.JSInterop;

namespace WSOA.Client.Shared.Popups.Components
{
    public class PopupMessageComponent : PopupComponentBase
    {
        public string Message { get; set; }

        public bool IsError { get; set; }

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
                    Message = currentPopupOpen.Message;
                    IsError = currentPopupOpen.IsError;
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
    }
}
