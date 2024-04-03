using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace WSOA.Client.Shared.Popups.Components
{
    public class PopupDispatchJackpotComponent : PopupComponentBase
    {
        public IDictionary<int, int> WinnableMoneysByPosition { get; set; }

        public int TotalJackpot { get; set; }

        public new EventCallback<IDictionary<int, int>> OnValid { get; set; }

        public string? ErrorMessage { get; set; }

        public EditContext EditContext { get; set; }

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
                    WinnableMoneysByPosition = currentPopupOpen.WinnableMoneysByPosition!.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    TotalJackpot = currentPopupOpen.TotalJackpot!.Value;
                    OnValid = currentPopupOpen.OnValidWinnableMoneysByPosition!.Value;
                    EditContext = new EditContext(WinnableMoneysByPosition);
                    ErrorMessage = null;
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

        public EventCallback CreateDispatchJackpotLine => EventCallback.Factory.Create(this, () =>
        {
            int maxCurrentPosition = WinnableMoneysByPosition.Keys.Max();
            WinnableMoneysByPosition.Add(maxCurrentPosition + 1, 0);
        });

        public EventCallback<int> RemoveDispatchJackpotLine => EventCallback.Factory.Create(this, (int positionToRemove) =>
        {
            IDictionary<int, int> newWinnableMoneysByPosition = new Dictionary<int, int>();
            int currentPosition = 1;

            foreach (KeyValuePair<int, int> moneyByPosition in WinnableMoneysByPosition)
            {
                if (moneyByPosition.Key != positionToRemove)
                {
                    newWinnableMoneysByPosition.Add(currentPosition, moneyByPosition.Value);
                    currentPosition++;
                }
            }

            WinnableMoneysByPosition = newWinnableMoneysByPosition;
        });

        public new EventCallback ValidPopup => EventCallback.Factory.Create(this, () =>
        {
            if (WinnableMoneysByPosition.Sum(money => money.Value) == TotalJackpot)
            {
                PopupEventHandler.Close();
                OnValid.InvokeAsync(WinnableMoneysByPosition);
            }
            else
            {
                ErrorMessage = "Le compte des répartitions ne correspond pas au total";
            }
        });
    }
}
