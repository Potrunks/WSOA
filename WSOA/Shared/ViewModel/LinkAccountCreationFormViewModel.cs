using System.ComponentModel.DataAnnotations;

namespace WSOA.Shared.ViewModel
{
    public class LinkAccountCreationFormViewModel
    {
        public LinkAccountCreationFormViewModel()
        {
            RecipientMail = null;
            ProfileCodeSelected = null;
            SubSectionIdConcerned = 0;
        }

        [Required(ErrorMessage = "Mail manquant")]
        public string RecipientMail { get; set; }

        [Required(ErrorMessage = "Profile manquant")]
        public string ProfileCodeSelected { get; set; }

        public int SubSectionIdConcerned { get; set; }
    }
}
