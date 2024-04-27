using Microsoft.AspNetCore.Components;
using WSOA.Shared.Dtos;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.Components.SeasonMyDetailResult
{
    public class SeasonMyDetailResultComponent : ComponentBase
    {
        [EditorRequired]
        [Parameter]
        public SeasonMyDetailResultDto Dto { get; set; }

        public SeasonMyDetailResultViewModel ViewModel { get; set; }

        protected override void OnInitialized()
        {
            ViewModel = new SeasonMyDetailResultViewModel(Dto);
        }
    }
}
