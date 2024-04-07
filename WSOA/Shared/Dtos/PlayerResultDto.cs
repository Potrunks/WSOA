namespace WSOA.Shared.Dtos
{
    public class PlayerResultDto
    {
        public int UserId { get; set; }

        public int Position { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Points { get; set; }

        public int TotalRebuy { get; set; }

        public int TotalAddon { get; set; }

        public int BuyIn { get; set; }

        public int TotalWinningAmount { get; set; }

        public IEnumerable<BonusTournamentEarnedResultDto> BonusTournamentEarneds { get; set; }

        public IEnumerable<EliminationResultDto> Eliminations { get; set; }

        public IEnumerable<EliminationResultDto> Victims { get; set; }
    }
}
