namespace WSOA.Shared.Dtos
{
    public class TournamentPlayedDto
    {
        public DateTime StartDate { get; set; }

        public int BuyIn { get; set; }

        public IEnumerable<PlayerResultDto> PlayerResults { get; set; }
    }
}
