namespace WSOA.Shared.Dtos
{
    public class EliminationCreationResultDto
    {
        public EliminationCreationResultDto()
        {
            EliminatorPlayerWonBonusCodes = new List<string>();
        }

        public int? EliminatedPlayerTotalReBuy { get; set; }

        public List<string> EliminatorPlayerWonBonusCodes { get; set; }

        public bool IsTournamentOver { get; set; }
    }
}
