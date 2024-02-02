using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WSOA.Client.Shared.EventHandlers;
using WSOA.Shared.Resources;

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
        public PopupKeyResources Key { get; set; }

        public Action? OnValid { get; set; }

        public string Title { get; set; }

        public bool IsDisplay { get; set; } = false;

        public EventCallback ClosePopup => EventCallback.Factory.Create(this, PerformClosePopup);

        public EventCallback ValidPopup => EventCallback.Factory.Create(this, PerformValidPopup);

        protected override void OnInitialized()
        {
            PopupEventHandler.OnPopupOpen += (obj, currentPopupOpen) =>
            {
                if (IsDisplay && currentPopupOpen.Key != Key)
                {
                    UnDisplayPopup();
                }

                if (!IsDisplay && currentPopupOpen.Key == Key)
                {
                    DisplayPopup(currentPopupOpen);
                }
            };

            PopupEventHandler.OnPopupClose += (obj, currentPopupOpen) =>
            {
                if (IsDisplay)
                {
                    UnDisplayPopup();
                }
            };
        }

        private void PerformClosePopup()
        {
            PopupEventHandler.Close();
        }

        private void PerformValidPopup()
        {
            PopupEventHandler.Close();

            OnValid.Invoke();
        }

        public void DisplayPopup(PopupEventArgs currentPopupOpen)
        {
            IsDisplay = true;
            Title = currentPopupOpen.Title;
            OnValid = currentPopupOpen.OnValid;
            StateHasChangedOverride();
        }

        public void UnDisplayPopup()
        {
            IsDisplay = false;
            StateHasChangedOverride();
        }

        private async void StateHasChangedOverride()
        {
            StateHasChanged();
            await JSRuntime.InvokeVoidAsync("blurring", "app_layout");
        }
    }
}
