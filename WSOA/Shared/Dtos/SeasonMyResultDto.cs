using WSOA.Shared.Resources;

namespace WSOA.Shared.Dtos
{
    public class SeasonMyResultDto
    {
        public SeasonMyResultDto()
        {
            Details = new List<SeasonMyDetailResultDto>();
        }

        public SeasonMyResultDto(string season, IEnumerable<RankResultType> rankResultTypes, IEnumerable<TournamentPlayedDto> tournamentPlayeds, int currentUsrId)
        {
            Season = season;

            Details = new List<SeasonMyDetailResultDto>();
            foreach (RankResultType rankResultType in rankResultTypes)
            {
                Details.Add(new SeasonMyDetailResultDto(rankResultType, currentUsrId, tournamentPlayeds));
            }
        }

        public string Season { get; set; }

        public List<SeasonMyDetailResultDto> Details { get; set; }
    }
}
