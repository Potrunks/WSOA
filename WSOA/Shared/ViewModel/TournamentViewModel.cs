using WSOA.Shared.Dtos;

namespace WSOA.Shared.ViewModel
{
    public class TournamentViewModel
    {
        public TournamentViewModel()
        {

        }

        public TournamentViewModel(TournamentDto tournamentDto, int currentUserId)
        {
            TournamentId = tournamentDto.Tournament.Id;
            Season = tournamentDto.Tournament.Season;
            StartDate = tournamentDto.Tournament.StartDate;
            BuyIn = tournamentDto.Tournament.BuyIn;
            Address = tournamentDto.Address.Content;
            PlayerDatasVM = tournamentDto.Players.Select(p => new PlayerViewModel(p.User, p.Player)).ToList();
            CurrentUserPresenceStateCode = tournamentDto.Players.SingleOrDefault(p => p.User.Id == currentUserId)?.Player.PresenceStateCode;
        }

        public int TournamentId { get; set; }

        public string Season { get; set; }

        public DateTime StartDate { get; set; }

        public int BuyIn { get; set; }

        public string Address { get; set; }

        public List<PlayerViewModel> PlayerDatasVM { get; set; }

        public string? CurrentUserPresenceStateCode { get; set; }
    }
}
