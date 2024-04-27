using WSOA.Shared.Dtos;
using WSOA.Shared.Resources;
using WSOA.Shared.Utils;

namespace WSOA.Shared.ViewModel
{
    public class RankResultViewModel
    {
        public RankResultViewModel() { }

        public RankResultViewModel(RankResultType rankResultType, RankResultDto rankResult)
        {
            Rank = rankResult.Rank;
            FullName = StringFormatUtil.ToFormatFullName(rankResult.FirstName, rankResult.LastName);
            Score = $"{rankResult.Score} {rankResultType.GetUnity()}";
            Evolution = ToFormatEvolution(rankResult.Evolution);
        }

        public int Rank { get; set; }

        public string FullName { get; set; }

        public string Score { get; set; }

        public string Evolution { get; set; }

        public string ToFormatEvolution(int evolution)
        {
            return evolution > 0 ? $"+{evolution}" : evolution < 0 ? evolution.ToString() : "-";
        }
    }
}
