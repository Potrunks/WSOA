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
            CurrentPopupOpen.OnValidSelectedItemIds = onValid;
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

        public void Open(IEnumerable<ItemSelectableViewModel> selectableItems, string title, EventCallback<IEnumerable<int>> onValid)
        {
            if (!selectableItems.Any())
            {
                Open(PopupErrorMessageResources.NO_SELECTABLE_ITEM, false, title, null);
            }
            else
            {
                CurrentPopupOpen.SelectableItems = selectableItems;

                Open(PopupKeyResources.ITEM_SELECT, title, onValid);
            }
        }

        public void Open(IEnumerable<ItemSelectableViewModel> selectableItems, string title, int playerId)
        {
            if (!selectableItems.Any())
            {
                Open(PopupErrorMessageResources.NO_SELECTABLE_ITEM, false, title, null);
            }
            else
            {
                CurrentPopupOpen.SelectableItems = selectableItems;
                CurrentPopupOpen.ConcernedItemId = playerId;

                Open(PopupKeyResources.PLAYER_ELIMINATION, title);
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
                CurrentPopupOpen.ConcernedItemId = concernedItemId;

                Open(PopupKeyResources.MENU, title);
            }
        }

        public void Close()
        {
            OnPopupClose.Invoke(this, CurrentPopupOpen);

            CurrentPopupOpen.Key = null;
            CurrentPopupOpen.Title = null;
            CurrentPopupOpen.Messages = null;
            CurrentPopupOpen.SelectableItems = null;
            CurrentPopupOpen.OnValid = null;
            CurrentPopupOpen.OnValidSelectedItemIds = null;
            CurrentPopupOpen.Buttons = null;
            CurrentPopupOpen.ConcernedItemId = null;
        }
    }
}
