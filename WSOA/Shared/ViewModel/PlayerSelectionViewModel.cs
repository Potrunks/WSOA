using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;

namespace WSOA.Shared.ViewModel
{
    public class PlayerSelectionViewModel
    {
        public PlayerSelectionViewModel()
        {

        }

        public PlayerSelectionViewModel(IEnumerable<PlayerDto> presentPlayers, IEnumerable<User> availableUsers)
        {
            PresentPlayers = presentPlayers.Select(pla => new PlayerViewModel(pla.User, pla.Player));
            AvailablePlayers = availableUsers.Select(usr => new PlayerViewModel(usr));
        }

        public IEnumerable<PlayerViewModel> PresentPlayers { get; set; }

        public IEnumerable<PlayerViewModel> AvailablePlayers { get; set; }
    }
}
