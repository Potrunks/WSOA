using Microsoft.AspNetCore.Components;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.NavMenus.Main.Components
{
    public class MainNavSubSectionComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public MainNavSubSectionViewModel ViewModel { get; set; }
    }
}
