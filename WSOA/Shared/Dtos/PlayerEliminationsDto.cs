using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class PlayerEliminationsDto
    {
        public PlayerEliminationsDto() { }

        public Player EliminatedPlayer { get; set; }

        public User EliminatedUser { get; set; }

        public IEnumerable<Elimination> EliminatedPlayerEliminations { get; set; }

        public IDictionary<int, Player> EliminatorPlayersById { get; set; }

        public Tournament Tournament { get; set; }
    }
}
