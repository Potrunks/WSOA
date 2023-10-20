using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSOA.Shared.Entity
{
    public class BusinessActionByProfileCode
    {
        public BusinessActionByProfileCode()
        {

        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Profile))]
        public string ProfileCode { get; set; }
        public Profile Profile { get; set; }

        [Required]
        [ForeignKey(nameof(BusinessAction))]
        public string BusinessActionCode { get; set; }
        public BusinessAction BusinessAction { get; set; }
    }
}
