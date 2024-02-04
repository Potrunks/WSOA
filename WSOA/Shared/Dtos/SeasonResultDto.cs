using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class SeasonResultDto
    {
        public SeasonResultDto() { }

        public IEnumerable<Tournament> Tournaments { get; set; }

        public IEnumerable<Player> Players { get; set; }

        public IEnumerable<Elimination> Eliminations { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
