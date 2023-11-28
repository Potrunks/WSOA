using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class BonusTournamentEarnedDto
    {
        public BonusTournamentEarnedDto() { }

        public BonusTournamentEarnedDto(BonusTournament bonusTournament, BonusTournamentEarned bonusTournamentEarned)
        {
            Code = bonusTournament.Code;
            LogoPath = bonusTournament.LogoPath;
            Occurence = bonusTournamentEarned.Occurrence;
        }

        public BonusTournamentEarnedDto(BonusTournament bonusTournament)
        {
            Code = bonusTournament.Code;
            LogoPath = bonusTournament.LogoPath;
            Occurence = 1;
        }

        public string Code { get; set; }

        public string LogoPath { get; set; }

        public int Occurence { get; set; }
    }
}
