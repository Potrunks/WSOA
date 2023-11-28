using Microsoft.AspNetCore.Components;
using WSOA.Shared.Forms;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.Popups.Components
{
    public class PopupCodeSelectComponent : PopupComponentBase
    {
        public IEnumerable<CodeSelectableViewModel> Items { get; set; }

        public CodeSelectedForm Form { get; set; }

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
                    Items = currentPopupOpen.SelectableCodes!;
                    Form = new CodeSelectedForm();
                    Form.ConcernedId = currentPopupOpen.ConcernedId!.Value;
                    Form.SelectedCode = Items.First().Value;
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
            Action<string, int> onValid = PopupEventHandler.CurrentPopupOpen.OnValidSelectedCode!;
            PopupEventHandler.Close();
            onValid.Invoke(Form.SelectedCode, Form.ConcernedId);
        });
    }
}
