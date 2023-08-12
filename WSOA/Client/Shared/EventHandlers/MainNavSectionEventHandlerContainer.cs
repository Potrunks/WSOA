namespace WSOA.Client.Shared.EventHandlers
{
    public class MainNavSectionEventHandlerContainer
    {
        public MainNavSectionEventArgs _currentMainNavSection = new MainNavSectionEventArgs();

        public event EventHandler<MainNavSectionEventArgs> _onSelectSectionChanged;

        public void Invoke()
        {
            if (_currentMainNavSection != null && _currentMainNavSection.Order != 0)
            {
                _onSelectSectionChanged?.Invoke(this, _currentMainNavSection);
            }
        }
    }
}
