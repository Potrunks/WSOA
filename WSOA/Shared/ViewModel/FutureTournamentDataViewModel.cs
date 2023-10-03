using WSOA.Shared.Dtos;

namespace WSOA.Shared.ViewModel
{
    public class FutureTournamentDataViewModel : TournamentDataViewModel
    {
        public FutureTournamentDataViewModel()
        {

        }

        public FutureTournamentDataViewModel(TournamentDto tournamentDto, int currentUserId) : base(tournamentDto)
        {
            CurrentUserPresenceStateCode = tournamentDto.Players.SingleOrDefault(p => p.User.Id == currentUserId)?.Player.PresenceStateCode;
        }

        public string? CurrentUserPresenceStateCode { get; set; }
    }
}
