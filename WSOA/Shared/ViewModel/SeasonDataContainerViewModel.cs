using WSOA.Shared.Dtos;
using WSOA.Shared.Resources;

namespace WSOA.Shared.ViewModel
{
    public class SeasonDataContainerViewModel
    {
        public SeasonDataContainerViewModel()
        {
            RankResultTypeLabel = "Pas de résultats";
            RankResults = new List<RankResultViewModel>();
            Id = null;
        }

        public SeasonDataContainerViewModel(KeyValuePair<RankResultType, List<RankResultDto>> rankResults, int index)
        {
            RankResultTypeLabel = rankResults.Key.GetLabel();
            RankResults = rankResults.Value.Select(rr => new RankResultViewModel(rankResults.Key, rr));
            Id = index;
        }

        public string RankResultTypeLabel { get; set; }

        public int? Id { get; set; }

        public IEnumerable<RankResultViewModel> RankResults { get; set; }
    }
}
