namespace WSOA.Shared.Dtos
{
    public class TournamentPreparedDto
    {
        public TournamentPreparedDto()
        {

        }

        public int TournamentId { get; set; }

        public IEnumerable<int> SelectedUserIds { get; set; }
    }
}
