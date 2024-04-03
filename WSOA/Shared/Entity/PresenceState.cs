using System.ComponentModel.DataAnnotations;

namespace WSOA.Shared.Entity
{
    public class PresenceState
    {
        [Key]
        [Required]
        public string Code { get; set; }

        [Required]
        public string Label { get; set; }
    }
}
