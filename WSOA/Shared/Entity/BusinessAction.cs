using System.ComponentModel.DataAnnotations;

namespace WSOA.Shared.Entity
{
    public class BusinessAction
    {
        public BusinessAction()
        {

        }

        [Key]
        [Required]
        public string Code { get; set; }

        [Required]
        public string Label { get; set; }
    }
}
