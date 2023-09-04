using WSOA.Shared.Entity;
using WSOA.Shared.Resources;

namespace WSOA.Shared.ViewModel
{
    public class TournamentCreationDataViewModel
    {
        public TournamentCreationDataViewModel()
        {

        }

        public TournamentCreationDataViewModel(IEnumerable<Address> addresses)
        {
            SelectableSeasons = GenerateSelectableSeasons();
            SelectableAddresses = addresses.Select(adr => new AddressDataViewModel(adr));
        }

        public IEnumerable<string> SelectableSeasons { get; set; }

        public IEnumerable<AddressDataViewModel> SelectableAddresses { get; set; }

        private IEnumerable<string> GenerateSelectableSeasons()
        {
            int currentYear = DateTime.UtcNow.Year;

            return new List<string>
            {
                (currentYear--).ToString(),
                currentYear.ToString(),
                SeasonResources.OUT_OF_SEASON
            };
        }
    }
}
