using System.ComponentModel.DataAnnotations;
using WSOA.Shared.Resources;

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

        [Required(ErrorMessage = DataValidationResources.MAIL_MISSING)]
        [RegularExpression(RegexResources.MAIL, ErrorMessage = DataValidationResources.MAIL_FORMAT_NO_VALID)]
        public string RecipientMail { get; set; }

        [Required(ErrorMessage = DataValidationResources.PROFILE_MISSING)]
        public string ProfileCodeSelected { get; set; }

        public int SubSectionIdConcerned { get; set; }
    }
}
