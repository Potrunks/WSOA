using System.ComponentModel.DataAnnotations;

namespace WSOA.Shared.Forms
{
    public class MailForm
    {
        [Required]
        public string Mail
        {
            get
            {
                return _mail;
            }
            set
            {
                _mail = value?.ToLower().Trim();
            }
        }
        private string _mail;

        [Required]
        public string BaseURL { get; set; }
    }
}
