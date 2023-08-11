using Microsoft.AspNetCore.Components;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.NavMenus.Main.Components
{
    public class MainNavSectionComponent : ComponentBase
    {
        [Parameter]
        public MainNavSectionViewModel ViewModel { get; set; }
    }
}
