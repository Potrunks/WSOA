using System.ComponentModel.DataAnnotations;
using WSOA.Shared.Resources;

namespace WSOA.Shared.ViewModel
{
    public class TournamentCreationFormViewModel
    {
        public TournamentCreationFormViewModel()
        {

        }

        public TournamentCreationFormViewModel(string baseUri, int subSectionId, TournamentCreationDataViewModel dataVm)
        {
            BaseUri = baseUri;
            SubSectionId = subSectionId;
            Season = dataVm.SelectableSeasons.First();
            StartDate = DateTime.UtcNow;
            BuyIn = 0;
            AddressId = dataVm.SelectableAddresses.First().Id;
        }

        [Required(ErrorMessage = DataValidationResources.SEASON_MISSING)]
        public string Season { get; set; }

        [Required(ErrorMessage = DataValidationResources.STARTDATE_MISSING)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = DataValidationResources.BUY_IN_MISSING)]
        [Range(1, int.MaxValue, ErrorMessage = DataValidationResources.BUY_IN_ERROR_RANGE)]
        public int BuyIn { get; set; }

        [Required]
        public int SubSectionId { get; set; }

        [Required(ErrorMessage = DataValidationResources.ADDRESS_MISSING)]
        public int AddressId { get; set; }

        [Required]
        public string BaseUri { get; set; }
    }
}
