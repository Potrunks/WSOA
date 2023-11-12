using WSOA.Shared.Dtos;

namespace WSOA.Shared.ViewModel
{
    public class PlayerPlayingViewModel
    {
        public PlayerPlayingViewModel()
        {

        }

        public PlayerPlayingViewModel(PlayerPlayingDto playerPlayingDto)
        {
            Id = playerPlayingDto.Id;
            IsEliminated = playerPlayingDto.IsEliminated;
            FirstName = $"{playerPlayingDto.FirstName.Substring(0, 1).ToUpper()}{playerPlayingDto.FirstName.Substring(1).ToLower()}";
            LastName = playerPlayingDto.LastName.ToUpper();
            TotalAddOn = playerPlayingDto.TotalAddOn == null ? 0 : playerPlayingDto.TotalAddOn.Value;
            TotalRebuy = playerPlayingDto.TotalRebuy == null ? 0 : playerPlayingDto.TotalRebuy.Value;
            EarnedBonusLogoPathsWithOccurrences = playerPlayingDto.EarnedBonusLogoPathsWithOccurrences;
            HasBonusEarned = EarnedBonusLogoPathsWithOccurrences.Any();
        }

        public int Id { get; set; }

        public bool IsEliminated { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int TotalAddOn { get; set; }

        public int TotalRebuy { get; set; }

        public IDictionary<string, int> EarnedBonusLogoPathsWithOccurrences { get; set; }

        public bool HasBonusEarned { get; set; }
    }
}
