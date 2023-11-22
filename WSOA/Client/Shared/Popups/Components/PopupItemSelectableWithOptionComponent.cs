using Microsoft.AspNetCore.Components;
using WSOA.Shared.Forms;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.Popups.Components
{
    public class PopupItemSelectableWithOptionComponent : PopupComponentBase
    {
        public List<ItemSelectableViewModel> Items { get; set; }

        public ItemSelectableWithOptionForm Form { get; set; }

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
                    Items = new List<ItemSelectableViewModel>();
                    Items.AddRange(currentPopupOpen.SelectableItems!);

                    Option = currentPopupOpen.Option!;

                    OnValid = currentPopupOpen.OnValidTwoSelectedIdsWithOption!;

                    ConcernedItemId = currentPopupOpen.ConcernedItemId!.Value;

                    Form = new ItemSelectableWithOptionForm();
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

            Console.WriteLine(Form.IsOptionSelected);

            //OnValid.Invoke(ConcernedItemId, Form.SelectedItemId, Form.IsOptionSelected);
        });
    }
}
