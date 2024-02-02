using Microsoft.AspNetCore.Components;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.Popups.Components
{
    public class PopupIdsSelectComponent : PopupComponentBase
    {
        public List<IdSelectableViewModel> Items { get; set; }

        public new EventCallback<IEnumerable<int>> OnValid { get; set; }

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
                    Items = new List<IdSelectableViewModel>();
                    Items.AddRange(currentPopupOpen.SelectableIds);

                    OnValid = currentPopupOpen.OnValidSelectedIds.Value;

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

        public EventCallback<IdSelectableViewModel> SwitchSelectionStatus => EventCallback.Factory.Create(this, (IdSelectableViewModel item) =>
        {
            item.IsSelected = !item.IsSelected;
        });

        public new EventCallback ValidPopup => EventCallback.Factory.Create(this, () =>
        {
            IEnumerable<int> selectedItemIds = Items.Where(item => item.IsSelected).Select(sel => sel.Id);

            OnValid.InvokeAsync(selectedItemIds);

            PopupEventHandler.Close();
        });
    }
}
