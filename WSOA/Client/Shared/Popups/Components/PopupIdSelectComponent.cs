using Microsoft.AspNetCore.Components;
using WSOA.Shared.Forms;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.Popups.Components
{
    public class PopupIdSelectComponent : PopupComponentBase
    {
        public List<IdSelectableViewModel> Items { get; set; }

        public IdSelectedWithOptionForm Form { get; set; }

        public OptionViewModel Option { get; set; }

        public new Action<int, int, bool> OnValid { get; set; }

        public int ConcernedItemId { get; set; }

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
                    Items.AddRange(currentPopupOpen.SelectableIds!);

                    Option = currentPopupOpen.Option!;

                    OnValid = currentPopupOpen.OnValidSelectedId!;

                    ConcernedItemId = currentPopupOpen.ConcernedId!.Value;

                    Form = new IdSelectedWithOptionForm();
                    Form.SelectedItemId = Items.First().Id;
                    Form.IsOptionSelected = Option.Value;

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

        public new EventCallback ValidPopup => EventCallback.Factory.Create(this, () =>
        {
            PopupEventHandler.Close();

            OnValid.Invoke(ConcernedItemId, Form.SelectedItemId, Form.IsOptionSelected);
        });
    }
}
