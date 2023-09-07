using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class TournamentDto
    {
        public TournamentDto()
        {
            Tournament = null;
            Players = new List<Player>();
        }

        public TournamentDto(Tournament tournament, IEnumerable<Player> players)
        {
            Tournament = tournament;
            Players = players;
        }

        public Tournament Tournament { get; set; }

        public IEnumerable<Player> Players { get; set; }
    }
}
