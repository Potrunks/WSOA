using WSOA.Shared.Dtos;

namespace WSOA.Shared.ViewModel
{
    public class TournamentsViewModel : SubSectionViewModel
    {
        public TournamentsViewModel()
        {

        }

        public TournamentsViewModel(IEnumerable<TournamentDto> tournamentDtos, string subSectionDescription, int currentUserId) : base(subSectionDescription)
        {
            TournamentsVM = tournamentDtos.Select(tou => new TournamentViewModel(tou, currentUserId)).ToList();
        }

        public List<TournamentViewModel> TournamentsVM { get; set; }
    }
}
