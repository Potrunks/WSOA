using Microsoft.AspNetCore.Components;
using WSOA.Shared.Resources;

namespace WSOA.Client.Shared.Popups.Components
{
    public class PopupInputNumberComponent : PopupComponentBase
    {
        public int InputNumber { get; set; }

        public int ConcernedId { get; set; }

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
                    InputNumber = currentPopupOpen.InputNumber!.Value;
                    ConcernedId = currentPopupOpen.ConcernedId!.Value;
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

        public EventCallback<NumberModificatorType> ChangeInputNumberValue => EventCallback.Factory.Create(this, (NumberModificatorType modificator) =>
        {
            switch (modificator)
            {
                case NumberModificatorType.INCREASE:
                    InputNumber++;
                    break;
                case NumberModificatorType.DECREASE:
                    if (InputNumber > 0)
                    {
                        InputNumber--;
                    }
                    break;
                default:
                    break;
            }
        });

        public new EventCallback ValidPopup => EventCallback.Factory.Create(this, () =>
        {
            Action<int, int> onValid = PopupEventHandler.CurrentPopupOpen.OnValidInputNumberForConcernedId!;
            PopupEventHandler.Close();
            onValid.Invoke(ConcernedId, InputNumber);
        });
    }
}
