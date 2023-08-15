using System.ComponentModel.DataAnnotations;

namespace WSOA.Shared.ViewModel
{
    public class LinkAccountCreationViewModel
    {
        public LinkAccountCreationViewModel()
        {
            RecipientMail = null;
            ProfileCodeSelected = null;
        }

        [Required(ErrorMessage = "Mail manquant")]
        public string RecipientMail { get; set; }

        [Required(ErrorMessage = "Profile manquant")]
        public string ProfileCodeSelected { get; set; }

        public int SubSectionIdConcerned { get; set; }
    }
}
