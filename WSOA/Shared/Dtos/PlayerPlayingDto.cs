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
            IEnumerable<BonusTournamentEarned> bonusTournamentEarneds
        )
        {
            Id = playerDto.Player.Id;
            FirstName = playerDto.User.FirstName;
            LastName = playerDto.User.LastName;
            TotalRebuy = playerDto.Player.TotalReBuy;
            TotalAddOn = playerDto.Player.TotalAddOn;
            BonusTournamentEarnedsByBonusTournamentCode = bonusTournamentEarneds.ToDictionary(bonus => bonus.BonusTournamentCode, bonus => new BonusTournamentEarnedDto(allBonusByCode[bonus.BonusTournamentCode], bonus));
            IsEliminated = playerDto.Player.CurrentTournamentPosition != null;
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? TotalRebuy { get; set; }

        public int? TotalAddOn { get; set; }

        public IDictionary<string, BonusTournamentEarnedDto> BonusTournamentEarnedsByBonusTournamentCode { get; set; }

        public bool IsEliminated { get; set; }
    }
}
