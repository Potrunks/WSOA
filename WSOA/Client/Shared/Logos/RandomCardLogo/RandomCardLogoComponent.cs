using Microsoft.AspNetCore.Components;
using WSOA.Shared.RenderObject;

namespace WSOA.Client.Shared.Logos.RandomCardLogo
{
    public class RandomCardLogoComponent : ComponentBase
    {
        [Parameter]
        public float? Height { get; set; }

        public CardRenderObject Card { get; set; } = new CardRenderObject();

        protected override void OnInitialized()
        {
            Height = Height ?? 15;
        }
    }
}
