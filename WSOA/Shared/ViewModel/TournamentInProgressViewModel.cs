using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;

namespace WSOA.Shared.ViewModel
{
    public class TournamentInProgressViewModel
    {
        public TournamentInProgressViewModel() { }

        public TournamentInProgressViewModel(TournamentInProgressDto tournamentInProgressDto)
        {
            Id = tournamentInProgressDto.Id;
            StartDate = tournamentInProgressDto.StartDate.ToString("dd MMMM yyyy");
            TotalJackpot = $"{tournamentInProgressDto.CalculateTotalJackpot()} euros";
            WinnableMoneys = tournamentInProgressDto.WinnableMoneyByPosition.Select(win =>
            {
                return win.Key == 1 ? $"{win.Key}er : {win.Value} euros" : $"{win.Key}eme : {win.Value} euros";
            });
            TournamentNumber = tournamentInProgressDto.TournamentNumber;
            Season = tournamentInProgressDto.Season;
            User? winnerLastTournament = tournamentInProgressDto.LastWinner;
            WinnerLastTournamentFullName = winnerLastTournament != null ? $"{winnerLastTournament.FirstName} {winnerLastTournament.LastName}" : "Personne";
            User? firstRanked = tournamentInProgressDto.FirstSeasonRanked;
            FirstRankedFullName = firstRanked != null ? $"{firstRanked.FirstName} {firstRanked.LastName}" : "Personne";
            Step = tournamentInProgressDto.IsFinalTable ? "Table finale" :
                    tournamentInProgressDto.IsAddOn ? "Add-On" :
                    "Normal";
        }

        public int Id { get; set; }

        public string StartDate { get; set; }

        public string TotalJackpot { get; set; }

        public IEnumerable<string> WinnableMoneys { get; set; }

        public int TournamentNumber { get; set; }

        public string Season { get; set; }

        public string WinnerLastTournamentFullName { get; set; }

        public string FirstRankedFullName { get; set; }

        public string Step { get; set; }
    }
}
