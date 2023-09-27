using WSOA.Shared.Dtos;

namespace WSOA.Shared.ViewModel
{
    public class FutureTournamentsViewModel : SubSectionDataViewModel
    {
        public FutureTournamentsViewModel()
        {
            FutureTournamentsVM = new List<FutureTournamentDataViewModel>();
        }

        public FutureTournamentsViewModel(IEnumerable<TournamentDto> tournamentDtos, int currentUserId, string description)
        {
            FutureTournamentsVM = tournamentDtos.Select(dto => new FutureTournamentDataViewModel(dto, currentUserId)).ToList();
            Description = description;
        }

        public List<FutureTournamentDataViewModel> FutureTournamentsVM { get; set; }
    }
}
