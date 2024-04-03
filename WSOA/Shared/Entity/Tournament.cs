using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WSOA.Shared.ViewModel;

namespace WSOA.Shared.Entity
{
    public class Tournament
    {
        public Tournament()
        {

        }

        public Tournament(TournamentCreationFormViewModel form)
        {
            Season = form.Season;
            StartDate = form.StartDate;
            BuyIn = form.BuyIn;
            AddressId = form.AddressId;
        }

        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Season { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsInProgress { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsOver { get; set; }

        [Required]
        public int BuyIn { get; set; }

        [Required]
        [ForeignKey(nameof(Address))]
        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}
