using WSOA.Shared.Dtos;

namespace WSOA.Shared.ViewModel
{
    public class SeasonResultViewModel
    {
        public SeasonResultViewModel() { }

        public SeasonResultViewModel(SeasonResultDto seasonResultDto)
        {
            Season = seasonResultDto.Tournaments.First().Season;
            NbTournamentPlayed = seasonResultDto.Tournaments.Count();
            PlayerPointList = seasonResultDto.Users.Select(usr => new PlayerPointViewModel(usr, seasonResultDto.Players.Where(pla => pla.UserId == usr.Id)));
            //PlayerProfitabilityList = seasonResultDto.Users.Select(usr => new PlayerProfitabilityViewModel
            //(
            //    usr,
            //    seasonResultDto.Players.Where(pla => pla.UserId == usr.Id),
            //    seasonResultDto.Tournaments.Where(tou => seasonResultDto.Players
            //                                                            .Where(pla => pla.UserId == usr.Id)
            //                                                            .Select(pla => pla.PlayedTournamentId)
            //                                                            .ToList()
            //                                                            .Contains(tou.Id))
            //));
            //PlayerEliminationList = seasonResultDto.Users.Select(usr => new PlayerEliminationViewModel
            //(
            //    usr,
            //    seasonResultDto.Eliminations,
            //    seasonResultDto.Players.Where(pla => pla.UserId == usr.Id)
            //));
        }

        public SeasonResultViewModel(int season)
        {
            Season = season.ToString();
            NbTournamentPlayed = 0;
            PlayerPointList = new List<PlayerPointViewModel>();
        }

        public string Season { get; set; }

        public int NbTournamentPlayed { get; set; }

        public IEnumerable<PlayerPointViewModel> PlayerPointList { get; set; }

        public IEnumerable<PlayerProfitabilityViewModel> PlayerProfitabilityList { get; set; }

        public IEnumerable<PlayerEliminationViewModel> PlayerEliminationList { get; set; }
    }
}
