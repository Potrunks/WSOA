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
            BaseUri = null;
        }

        [Required(ErrorMessage = DataValidationResources.MAIL_MISSING)]
        [RegularExpression(RegexResources.MAIL, ErrorMessage = DataValidationResources.MAIL_FORMAT_NO_VALID)]
        public string RecipientMail
        {
            get
            {
                return _recipientMail;
            }
            set
            {
                _recipientMail = value?.Trim();
            }
        }
        private string _recipientMail;

        [Required(ErrorMessage = DataValidationResources.PROFILE_MISSING)]
        public string ProfileCodeSelected { get; set; }

        public int SubSectionIdConcerned { get; set; }

        public string BaseUri { get; set; }
    }
}
