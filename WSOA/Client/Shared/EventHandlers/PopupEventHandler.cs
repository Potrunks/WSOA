using Microsoft.AspNetCore.Components;
using WSOA.Shared.Resources;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.EventHandlers
{
    public class PopupEventHandler
    {
        public PopupEventArgs CurrentPopupOpen { get; set; } = new PopupEventArgs();

        public event EventHandler<PopupEventArgs> OnPopupOpen;

        public event EventHandler<PopupEventArgs> OnPopupClose;

        private void Open(PopupKeyResources key, string title, Action? onValid)
        {
            CurrentPopupOpen.OnValid = onValid;
            Open(key, title);
        }

        private void Open(PopupKeyResources key, string title, EventCallback<IEnumerable<int>>? onValid)
        {
            CurrentPopupOpen.OnValidSelectedIds = onValid;
            Open(key, title);
        }

        private void Open(PopupKeyResources key, string title)
        {
            CurrentPopupOpen.Key = key;
            CurrentPopupOpen.Title = title;

            OnPopupOpen.Invoke(this, CurrentPopupOpen);
        }

        public void Open(string msg, bool isError, string title, Action? onValid)
        {
            CurrentPopupOpen.Messages = new List<MessageViewModel>
            {
                new MessageViewModel
                {
                    IsError = isError,
                    Content = msg
                }
            };

            Open(PopupKeyResources.MESSAGE, title, onValid);
        }

        public void Open(IEnumerable<MessageViewModel> messages, string title, Action? onValid)
        {
            CurrentPopupOpen.Messages = messages;

            Open(PopupKeyResources.MESSAGE, title, onValid);
        }

        public void Open(IEnumerable<IdSelectableViewModel> selectableItems, string title, EventCallback<IEnumerable<int>> onValid)
        {
            if (!selectableItems.Any())
            {
                Open(PopupErrorMessageResources.NO_SELECTABLE_ITEM, false, title, null);
            }
            else
            {
                CurrentPopupOpen.SelectableIds = selectableItems;

                Open(PopupKeyResources.MULTI_ID_SELECT, title, onValid);
            }
        }

        public void Open(IEnumerable<IdSelectableViewModel> selectableItems, string title, int concernedId, OptionViewModel option, Action<int, int, bool> onValid)
        {
            if (!selectableItems.Any())
            {
                Open(PopupErrorMessageResources.NO_SELECTABLE_ITEM, false, title, null);
            }
            else
            {
                CurrentPopupOpen.SelectableIds = selectableItems;
                CurrentPopupOpen.Option = option;
                CurrentPopupOpen.ConcernedId = concernedId;
                CurrentPopupOpen.OnValidSelectedId = onValid;

                Open(PopupKeyResources.ID_SELECT_OPTION, title);
            }
        }

        public void Open(IEnumerable<PopupButtonViewModel> buttons, string title, int? concernedItemId)
        {
            if (!buttons.Any())
            {
                Open(PopupErrorMessageResources.NO_ACTION, false, title, null);
            }
            else
            {
                CurrentPopupOpen.Buttons = buttons;
                CurrentPopupOpen.ConcernedId = concernedItemId;

                Open(PopupKeyResources.MENU, title);
            }
        }

        public void Open(IEnumerable<CodeSelectableViewModel> items, string title, int concernedId, Action<string, int> onValid)
        {
            if (items.Any())
            {
                CurrentPopupOpen.SelectableCodes = items;
                CurrentPopupOpen.OnValidSelectedCode = onValid;
                CurrentPopupOpen.ConcernedId = concernedId;
                Open(PopupKeyResources.CODE_SELECT, title);
            }
            else
            {
                Open(PopupErrorMessageResources.NO_SELECTABLE_ITEM, false, title, null);
            }
        }

        public void OpenInputNumberPopup(int? inputNumber, string title, int concernedId, Action<int, int> onValidInputNumberForConcernedId)
        {
            CurrentPopupOpen.InputNumber = inputNumber == null ? 0 : inputNumber;
            CurrentPopupOpen.ConcernedId = concernedId;
            CurrentPopupOpen.OnValidInputNumberForConcernedId = onValidInputNumberForConcernedId;
            Open(PopupKeyResources.INPUT_NUMBER, title);
        }

        public void OpenDispatchJackpotPopup(IDictionary<int, int> winnableMoneyByPosition, int totalJackpot, EventCallback<IDictionary<int, int>> onValidDispatchJackpot)
        {
            CurrentPopupOpen.WinnableMoneysByPosition = winnableMoneyByPosition;
            CurrentPopupOpen.TotalJackpot = totalJackpot;
            CurrentPopupOpen.OnValidWinnableMoneysByPosition = onValidDispatchJackpot;
            Open(PopupKeyResources.DISPATCH_JACKPOT, "Répartition des gains");
        }

        public void Close()
        {
            OnPopupClose.Invoke(this, CurrentPopupOpen);

            CurrentPopupOpen.Key = null;
            CurrentPopupOpen.Title = null;
            CurrentPopupOpen.Messages = null;
            CurrentPopupOpen.SelectableIds = null;
            CurrentPopupOpen.OnValid = null;
            CurrentPopupOpen.OnValidSelectedIds = null;
            CurrentPopupOpen.Buttons = null;
            CurrentPopupOpen.ConcernedId = null;
            CurrentPopupOpen.Option = null;
            CurrentPopupOpen.OnValidSelectedId = null;
            CurrentPopupOpen.SelectableCodes = null;
            CurrentPopupOpen.OnValidSelectedCode = null;
            CurrentPopupOpen.OnValidInputNumberForConcernedId = null;
            CurrentPopupOpen.InputNumber = null;
            CurrentPopupOpen.WinnableMoneysByPosition = null;
            CurrentPopupOpen.OnValidWinnableMoneysByPosition = null;
            CurrentPopupOpen.TotalJackpot = null;
        }
    }
}
