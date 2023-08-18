using Microsoft.AspNetCore.Components;
using WSOA.Client.Shared.Resources;

namespace WSOA.Client.Shared.Buttons.Main.Components
{
    public class MainButtonComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public string Label { get; set; }

        [Parameter]
        public string Type { get; set; }

        [Parameter]
        public bool? IsPrimary { get; set; }

        public string? _primaryCssClassName;

        protected override void OnInitialized()
        {
            Type = Type == null ? ButtonTypeResources.BUTTON : Type.ToLower();
            _primaryCssClassName = IsPrimary != null && IsPrimary.Value ? CssClassNameResources.PRIMARY : CssClassNameResources.EMPTY_CLASS;
        }
    }
}
