using WSOA.Shared.Dtos;

namespace WSOA.Shared.ViewModel
{
    public class SeasonMySubDetailResultViewModel
    {
        public SeasonMySubDetailResultViewModel() { }

        public SeasonMySubDetailResultViewModel(SeasonMySubDetailResultDto dto)
        {
            Score = $"{dto.Score} {dto.GetUnity()}";
        }

        public string Score { get; set; }
    }
}
