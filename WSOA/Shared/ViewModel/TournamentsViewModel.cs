using WSOA.Shared.Dtos;

namespace WSOA.Shared.ViewModel
{
    public class TournamentsViewModel : SubSectionDataViewModel
    {
        public TournamentsViewModel()
        {

        }

        public TournamentsViewModel(IEnumerable<TournamentDto> tournamentDtos, string description) : base(description)
        {
            TournamentDatasVM = tournamentDtos.Select(tou => new TournamentDataViewModel(tou));
        }

        IEnumerable<TournamentDataViewModel> TournamentDatasVM { get; set; }
    }
}
