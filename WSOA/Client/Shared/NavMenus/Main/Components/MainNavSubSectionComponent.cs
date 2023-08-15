using Microsoft.AspNetCore.Components;
using WSOA.Client.Shared.EventHandlers;
using WSOA.Client.Shared.Resources;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.NavMenus.Main.Components
{
    public class MainNavSubSectionComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public MainNavSubSectionViewModel ViewModel { get; set; }

        [Parameter]
        public Action? OnSubSectionSelected { get; set; }

        [Inject]
        public MainNavSubSectionEventHandler EventHandler { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string _selectedStateCssClassName = CssClassNameResources.EMPTY_CLASS;

        protected override void OnInitialized()
        {
            EventHandler._onSelectSectionChanged += (obj, currentMainNavSection) =>
            {
                if (currentMainNavSection.Order != ViewModel.Order && _selectedStateCssClassName == CssClassNameResources.SELECTED)
                {
                    UnselectSubSection();
                }
            };
        }

        public void SelectSubSection()
        {
            _selectedStateCssClassName = CssClassNameResources.SELECTED;
            EventHandler._currentMainNavSubSection.Order = ViewModel.Order;
            EventHandler.Invoke();
            OnSubSectionSelected?.Invoke();
            NavigationManager.NavigateTo(ViewModel.Url);
        }

        private void UnselectSubSection()
        {
            _selectedStateCssClassName = CssClassNameResources.EMPTY_CLASS;
            StateHasChanged();
        }
    }
}
