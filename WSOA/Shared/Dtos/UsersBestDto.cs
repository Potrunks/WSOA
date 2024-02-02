using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class UsersBestDto
    {
        public UsersBestDto() { }

        public User? FirstRanked { get; set; }

        public User? WinnerPreviousTournament { get; set; }

        public int TotalSeasonTournamentPlayed { get; set; }
    }
}
