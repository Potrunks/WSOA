using WSOA.Shared.Resources;

namespace WSOA.Client.Shared.EventHandlers
{
    public class PopupEventHandler
    {
        public PopupEventArgs CurrentPopupOpen { get; set; } = new PopupEventArgs();

        public event EventHandler<PopupEventArgs> OnPopupOpen;

        public event EventHandler<PopupEventArgs> OnPopupClose;

        public void Open(string popupKey)
        {
            CurrentPopupOpen.Key = popupKey;

            OnPopupOpen.Invoke(this, CurrentPopupOpen);
        }

        public void Open(string msg, bool isError)
        {
            CurrentPopupOpen.IsError = isError;
            CurrentPopupOpen.Message = msg;

            Open(PopupKeyResources.MESSAGE);
        }

        public void Close()
        {
            OnPopupClose.Invoke(this, CurrentPopupOpen);

            CurrentPopupOpen.Key = null;
        }
    }
}
