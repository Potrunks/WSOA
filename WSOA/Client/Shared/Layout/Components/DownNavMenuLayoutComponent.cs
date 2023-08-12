using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Resources;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.Layout.Components
{
    public class DownNavMenuLayoutComponent : LayoutComponentBase
    {
        [Inject]
        public IMenuService MenuService { get; set; }

        public List<MainNavSectionViewModel> _mainNavSectionVMs = new List<MainNavSectionViewModel>();

        public IDictionary<int, string> SelectedStateBySectionOrder { get; set; }

        protected override async Task OnInitializedAsync()
        {
            MainNavMenuResult result = await MenuService.LoadMainMenu();
            if (!result.Success)
            {
                // Le menu ne s'est pas charger correctement, rediriger vers page de connexion avec message error ?
            }

            _mainNavSectionVMs = result.MainNavSectionVMs.OrderBy(sec => sec.Order).ToList();
            SelectedStateBySectionOrder = result.MainNavSectionVMs.ToDictionary(sec => sec.Order, sec => CssClassNameResources.EMPTY_CLASS);
        }
    }
}
