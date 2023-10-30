using WSOA.Shared.Dtos;

namespace WSOA.Shared.ViewModel
{
    public class PlayerPlayingViewModel
    {
        public PlayerPlayingViewModel()
        {

        }

        public PlayerPlayingViewModel(PlayerDto playerDto)
        {
            Id = playerDto.Player.Id;
            FirstName = playerDto.User.FirstName;
            LastName = playerDto.User.LastName;
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
