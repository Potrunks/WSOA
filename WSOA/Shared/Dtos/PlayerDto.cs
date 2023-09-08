using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class PlayerDto
    {
        public PlayerDto()
        {

        }

        public PlayerDto(Player player, User user)
        {
            Player = player;
            User = user;
        }

        public Player Player { get; set; }

        public User User { get; set; }
    }
}
