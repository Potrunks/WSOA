namespace WSOA.Shared.Dtos
{
    public class EliminationEditionDto
    {
        public EliminationEditionDto() { }

        public int EliminatedPlayerId { get; set; }

        public bool IsAddOn { get; set; }

        public bool IsFinalTable { get; set; }

        public int TournamentId { get; set; }

        public int BuyIn { get; set; }
    }
}
