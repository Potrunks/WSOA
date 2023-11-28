using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class PlayerPlayingDto
    {
        public PlayerPlayingDto()
        {

        }

        public PlayerPlayingDto
        (
            PlayerDto playerDto,
            IDictionary<string, BonusTournament> allBonusByCode,
            IEnumerable<BonusTournamentEarned> bonusTournamentEarneds,
            User? lastWinner,
            User? firstRanked
        )
        {
            Id = playerDto.Player.Id;
            FirstName = playerDto.User.FirstName;
            LastName = playerDto.User.LastName;
            TotalRebuy = playerDto.Player.TotalReBuy;
            TotalAddOn = playerDto.Player.TotalAddOn;
            BonusTournamentEarnedsByBonusTournamentCode = bonusTournamentEarneds.ToDictionary(bonus => bonus.BonusTournamentCode, bonus => new BonusTournamentEarnedDto(allBonusByCode[bonus.BonusTournamentCode], bonus));
            IsEliminated = playerDto.Player.CurrentTournamentPosition != null;
            HasWinLastTournament = lastWinner != null && lastWinner.Id == playerDto.User.Id;
            IsActualFirstSeasonRank = firstRanked != null && firstRanked.Id == playerDto.User.Id;
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? TotalRebuy { get; set; }

        public int? TotalAddOn { get; set; }

        public IDictionary<string, BonusTournamentEarnedDto> BonusTournamentEarnedsByBonusTournamentCode { get; set; }

        public bool IsEliminated { get; set; }

        public bool HasWinLastTournament { get; set; }

        public bool IsActualFirstSeasonRank { get; set; }
    }
}
