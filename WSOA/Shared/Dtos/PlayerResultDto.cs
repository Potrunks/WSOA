namespace WSOA.Shared.Dtos
{
    public class PlayerResultDto
    {
        public int PlayerId { get; set; }

        public int UserId { get; set; }

        public int Position { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Points { get; set; }

        public int TotalRebuy { get; set; }

        public int TotalAddon { get; set; }

        public int BuyIn { get; set; }

        public int TotalWinningAmount { get; set; }

        public bool WasFinalTable { get; set; }

        public string PresenceStateCode { get; set; }

        public List<BonusTournamentEarnedResultDto> BonusTournamentEarneds { get; set; }

        public List<EliminationResultDto> Eliminations { get; set; }

        public List<EliminationResultDto> Victimisations { get; set; }
    }
}
