using Microsoft.AspNetCore.Components;
using WSOA.Client.Shared.EventHandlers;
using WSOA.Client.Shared.Resources;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.NavMenus.Main.Components
{
    public class MainNavSectionComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public MainNavSectionViewModel ViewModel { get; set; }

        [Inject]
        public MainNavSectionEventHandler EventHandler { get; set; }

        public string _selectedStateCssClassName = CssClassNameResources.EMPTY_CLASS;

        public string _openSubSectionsCssClassName = CssClassNameResources.EMPTY_CLASS;

        protected override void OnInitialized()
        {
            EventHandler._onSelectSectionChanged += (obj, currentMainNavSection) =>
            {
                if (currentMainNavSection.Order != ViewModel.Order && _selectedStateCssClassName == CssClassNameResources.SELECTED)
                {
                    UnselectSection();
                }
            };
        }

        public void SelectSection()
        {
            _selectedStateCssClassName = CssClassNameResources.SELECTED;
            if (ViewModel.MainNavSubSectionVMs.Any())
            {
                _openSubSectionsCssClassName = _openSubSectionsCssClassName == CssClassNameResources.EMPTY_CLASS ? CssClassNameResources.OPEN : CssClassNameResources.EMPTY_CLASS;
            }
            EventHandler._currentMainNavSection.Order = ViewModel.Order;
            EventHandler.Invoke();
        }

        private void UnselectSection()
        {
            _selectedStateCssClassName = CssClassNameResources.EMPTY_CLASS;
            _openSubSectionsCssClassName = CssClassNameResources.EMPTY_CLASS;
            StateHasChanged();
        }
    }
}
