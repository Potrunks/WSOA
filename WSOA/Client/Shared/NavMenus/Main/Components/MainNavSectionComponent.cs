using Microsoft.AspNetCore.Components;
using WSOA.Client.Shared.EventHandlers;
using WSOA.Client.Shared.Resources;
using WSOA.Shared.Resources;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.NavMenus.Main.Components
{
    public class MainNavSectionComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public MainNavSectionViewModel ViewModel { get; set; }

        [CascadingParameter]
        public IDictionary<int, string> SelectedStateBySectionOrder { get; set; }

        [Inject]
        public MainNavSectionEventHandlerContainer EventHandlerContainer { get; set; }

        public string _selectedStateCssClassName = CssClassNameResources.EMPTY_CLASS;

        public string _openSubSectionsCssClassName = CssClassNameResources.EMPTY_CLASS;

        protected override void OnInitialized()
        {
            if (ViewModel.Order == MainNavSectionResources.HOME_ORDER)
            {
                _selectedStateCssClassName = CssClassNameResources.SELECTED;
            }

            SelectedStateBySectionOrder[ViewModel.Order] = _selectedStateCssClassName;

            EventHandlerContainer._onSelectSectionChanged += (obj, currentMainNavSection) =>
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
            if (ViewModel.Order != MainNavSectionResources.HOME_ORDER && ViewModel.MainNavSubSectionVMs.Any())
            {
                _openSubSectionsCssClassName = CssClassNameResources.OPEN;
            }
            SelectedStateBySectionOrder[ViewModel.Order] = _selectedStateCssClassName;
            EventHandlerContainer._currentMainNavSection.Order = ViewModel.Order;
            EventHandlerContainer.Invoke();
        }

        private void UnselectSection()
        {
            _selectedStateCssClassName = CssClassNameResources.EMPTY_CLASS;
            _openSubSectionsCssClassName = CssClassNameResources.EMPTY_CLASS;
            SelectedStateBySectionOrder[ViewModel.Order] = _selectedStateCssClassName;
            StateHasChanged();
        }
    }
}
