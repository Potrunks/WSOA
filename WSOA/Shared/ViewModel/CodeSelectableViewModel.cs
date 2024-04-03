using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;

namespace WSOA.Shared.ViewModel
{
    public class CodeSelectableViewModel
    {
        public CodeSelectableViewModel()
        {

        }

        public CodeSelectableViewModel(BonusTournament bonusTournament)
        {
            Value = bonusTournament.Code;
            Label = bonusTournament.Label;
        }

        public CodeSelectableViewModel(BonusTournamentEarnedDto bonusTournamentEarnedDto)
        {
            Value = bonusTournamentEarnedDto.Code;
            Label = bonusTournamentEarnedDto.Label;
        }

        public string Value { get; set; }

        public string Label { get; set; }

        public bool IsSelected { get; set; }
    }
}
