using WSOA.Shared.Dtos;

namespace WSOA.Shared.ViewModel
{
    public class TournamentDataViewModel
    {
        public TournamentDataViewModel()
        {

        }

        public TournamentDataViewModel(TournamentDto tournamentDto)
        {
            TournamentId = tournamentDto.Tournament.Id;
            Season = tournamentDto.Tournament.Season;
            StartDate = tournamentDto.Tournament.StartDate;
            BuyIn = tournamentDto.Tournament.BuyIn;
            Address = tournamentDto.Address.Content;
            PlayerDatasVM = tournamentDto.Players.Select(p => new PlayerDataViewModel(p.User, p.Player.PresenceStateCode)).ToList();
        }

        public int TournamentId { get; set; }

        public string Season { get; set; }

        public DateTime StartDate { get; set; }

        public int BuyIn { get; set; }

        public string Address { get; set; }

        public List<PlayerDataViewModel> PlayerDatasVM { get; set; }
    }
}
