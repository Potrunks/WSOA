namespace WSOA.Client.Shared.EventHandlers
{
    public class PopupEventArgs : EventArgs
    {
        public string? Key { get; set; }

        public string? Message { get; set; }

        public bool IsError { get; set; }
    }
}
