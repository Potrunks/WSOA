using Microsoft.AspNetCore.Components;
using WSOA.Shared.Dtos;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.Components.SeasonMySubDetailResult
{
    public class SeasonMySubDetailResultComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public SeasonMySubDetailResultDto Dto { get; set; }

        public SeasonMySubDetailResultViewModel ViewModel { get; set; }

        protected override void OnInitialized()
        {
            ViewModel = new SeasonMySubDetailResultViewModel(Dto);
        }
    }
}
