using WSOA.Shared.Dtos;
using WSOA.Shared.Resources;

namespace WSOA.Shared.ViewModel
{
    public class SeasonMyDetailResultViewModel
    {
        public SeasonMyDetailResultViewModel() { }

        public SeasonMyDetailResultViewModel(SeasonMyDetailResultDto dto)
        {
            ResultTypeLabel = dto.RankResultType.GetLabel();
            Score = $"{dto.Score} {dto.RankResultType.GetUnity(dto.Score)}";
        }

        public string ResultTypeLabel { get; set; }

        public string Score { get; set; }
    }
}
