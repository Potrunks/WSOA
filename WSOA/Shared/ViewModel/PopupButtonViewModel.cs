namespace WSOA.Shared.ViewModel
{
    public class PopupButtonViewModel
    {
        public PopupButtonViewModel() { }

        public string Label { get; set; }

        public Action<int?> Action { get; set; }
    }
}
