using Microsoft.AspNetCore.Components;
using WSOA.Shared.Dtos;
using WSOA.Shared.Resources;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.Components.SeasonDataContainer.Components
{
    public class SeasonDataContainerComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public IDictionary<RankResultType, List<RankResultDto>> RankResults { get; set; }

        public SeasonDataContainerViewModel ViewModel { get; set; }

        private IDictionary<int, RankResultType> _availableRankResultType = new Dictionary<int, RankResultType>();

        protected override void OnInitialized()
        {
            List<RankResultType> rankResultTypes = RankResults.Keys.ToList();
            for (int i = 0; i < RankResults.Keys.Count; i++)
            {
                _availableRankResultType.Add(i, rankResultTypes[i]);
            }

            ViewModel = RankResults.Any() ? new SeasonDataContainerViewModel(RankResults.First(), 0) : new SeasonDataContainerViewModel();
        }

        public string GetScoreEvolutionClass(RankResultViewModel rankResult)
        {
            if (rankResult.Evolution == "-")
            {
                return "no-evol";
            }

            if (rankResult.Evolution.StartsWith('+'))
            {
                return "evol-up";
            }

            if (rankResult.Evolution.StartsWith('-'))
            {
                return "evol-down";
            }

            return string.Empty;
        }

        public string GetColorRankClass(RankResultViewModel rankResult)
        {
            if (rankResult.Rank == 1)
            {
                return "first";
            }

            if (rankResult.Rank == 2)
            {
                return "second";
            }

            if (rankResult.Rank == 3)
            {
                return "third";
            }

            return string.Empty;
        }

        public EventCallback<bool> SwitchRankResultDisplay => EventCallback.Factory.Create(this, (bool isNext) =>
        {
            if (ViewModel.Id == null)
            {
                return;
            }

            int currentId = ViewModel.Id.Value;
            int maxId = _availableRankResultType.Count - 1;

            if (isNext)
            {
                int nextId = currentId + 1;

                if (nextId <= maxId)
                {
                    ViewModel = new SeasonDataContainerViewModel(RankResults.Single(rr => rr.Key == _availableRankResultType[nextId]), nextId);
                }
                else
                {
                    ViewModel = new SeasonDataContainerViewModel(RankResults.First(), 0);
                }
            }
            else
            {
                int nextId = currentId - 1;

                if (nextId >= 0)
                {
                    ViewModel = new SeasonDataContainerViewModel(RankResults.Single(rr => rr.Key == _availableRankResultType[nextId]), nextId);
                }
                else
                {
                    ViewModel = new SeasonDataContainerViewModel(RankResults.Last(), maxId);
                }
            }
        });
    }
}
