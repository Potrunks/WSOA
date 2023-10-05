using WSOA.Shared.Dtos;

namespace WSOA.Shared.ViewModel
{
    public class FutureTournamentsViewModel : SubSectionViewModel
    {
        public FutureTournamentsViewModel()
        {
            FutureTournamentsVM = new List<FutureTournamentDataViewModel>();
        }

        public FutureTournamentsViewModel(IEnumerable<TournamentDto> tournamentDtos, int currentUserId, string description) : base(description)
        {
            FutureTournamentsVM = tournamentDtos.Select(dto => new FutureTournamentDataViewModel(dto, currentUserId)).ToList();
        }

        public List<FutureTournamentDataViewModel> FutureTournamentsVM { get; set; }
    }
}
