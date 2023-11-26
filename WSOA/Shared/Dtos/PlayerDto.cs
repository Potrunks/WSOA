using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class PlayerDto
    {
        public PlayerDto()
        {

        }

        public Player Player { get; set; }

        public User User { get; set; }
    }
}
