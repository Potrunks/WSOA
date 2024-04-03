using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.Popups.Components
{
    public class PopupMessageComponent : PopupComponentBase
    {
        public IEnumerable<MessageViewModel> Messages { get; set; }

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
                    Messages = currentPopupOpen.Messages;

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
    }
}
