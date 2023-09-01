using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WSOA.Shared.Entity
{
    public class Tournament
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Season { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsStarted { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsOver { get; set; }

        [Required]
        public int BuyIn { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
