using WSOA.Shared.Dtos;

namespace WSOA.Shared.ViewModel
{
    public class FutureTournamentDataViewModel
    {
        public FutureTournamentDataViewModel()
        {

        }

        public FutureTournamentDataViewModel(TournamentDto tournamentDto, int currentUserId)
        {
            Season = tournamentDto.Tournament.Season;
            StartDate = tournamentDto.Tournament.StartDate;
            BuyIn = tournamentDto.Tournament.BuyIn;
            Address = tournamentDto.Address.Content;
            PlayersDataVM = tournamentDto.Players.Select(p => new PlayerDataViewModel(p.User, p.Player.PresenceStateCode));
            CurrentUserPresenceStateCode = tournamentDto.Players.SingleOrDefault(p => p.User.Id == currentUserId)?.Player.PresenceStateCode;
        }

        public string Season { get; set; }

        public DateTime StartDate { get; set; }

        public int BuyIn { get; set; }

        public string Address { get; set; }

        public IEnumerable<PlayerDataViewModel> PlayersDataVM { get; set; }

        public string? CurrentUserPresenceStateCode { get; set; }
    }
}
