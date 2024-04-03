namespace WSOA.Client.Shared.EventHandlers
{
    public class MainNavSubSectionEventHandler
    {
        public MainNavSubSectionEventArgs _currentMainNavSubSection = new MainNavSubSectionEventArgs();

        public event EventHandler<MainNavSubSectionEventArgs> _onSelectSectionChanged;

        public void Invoke()
        {
            if (_currentMainNavSubSection != null)
            {
                _onSelectSectionChanged?.Invoke(this, _currentMainNavSubSection);
            }
        }
    }
}
