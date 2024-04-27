namespace WSOA.Shared.Dtos
{
    public class TournamentPlayedDto
    {
        public DateTime StartDate { get; set; }

        public int BuyIn { get; set; }

        public List<PlayerResultDto> PlayerResults { get; set; }
    }
}
