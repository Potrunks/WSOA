using Microsoft.AspNetCore.Components;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.Popups.Components
{
    public class PopupMenuComponent : PopupComponentBase
    {
        public List<PopupButtonViewModel> Buttons { get; set; }

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
                    Buttons = currentPopupOpen.Buttons.ToList();

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

        public EventCallback<Action<int?>> ExecutePopupAction => EventCallback.Factory.Create(this, (Action<int?> action) =>
        {
            int? concernedItemId = PopupEventHandler.CurrentPopupOpen.ConcernedItemId;
            PopupEventHandler.Close();
            action.Invoke(concernedItemId);
        });
    }
}
