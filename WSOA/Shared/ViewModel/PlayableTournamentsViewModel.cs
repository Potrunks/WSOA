using WSOA.Shared.Dtos;

namespace WSOA.Shared.ViewModel
{
    public class PlayableTournamentsViewModel : SubSectionViewModel
    {
        public PlayableTournamentsViewModel()
        {

        }

        public PlayableTournamentsViewModel(IEnumerable<TournamentDto> tournamentDtos, string subSectionDescription) : base(subSectionDescription)
        {
            TournamentsVM = tournamentDtos.Select(tou => new TournamentViewModel(tou));
        }

        public IEnumerable<TournamentViewModel> TournamentsVM { get; set; }
    }
}
