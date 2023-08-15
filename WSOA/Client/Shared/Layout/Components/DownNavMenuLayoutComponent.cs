using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.Layout.Components
{
    public class DownNavMenuLayoutComponent : LayoutComponentBase
    {
        [Inject]
        public IMenuService MenuService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public List<MainNavSectionViewModel> _mainNavSectionVMs = new List<MainNavSectionViewModel>();

        protected override async Task OnInitializedAsync()
        {
            MainNavMenuResult result = await MenuService.LoadMainMenu();
            if (!result.Success)
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
                return;
            }

            _mainNavSectionVMs = result.MainNavSectionVMs.OrderBy(sec => sec.Order).ToList();
        }
    }
}
