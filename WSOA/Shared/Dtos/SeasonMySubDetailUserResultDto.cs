using WSOA.Shared.Resources;

namespace WSOA.Shared.Dtos
{
    public class SeasonMySubDetailUserResultDto : SeasonMySubDetailResultDto
    {
        public SeasonMySubDetailUserResultDto()
        {

        }

        public static List<SeasonMySubDetailUserResultDto> CreateSeasonMySubDetailUserResultDtos(IEnumerable<TournamentPlayedDto> tournamentPlayeds, SubRankResultType subRankResultType, int currentUserId)
        {
            List<SeasonMySubDetailUserResultDto> result = new List<SeasonMySubDetailUserResultDto>();

            if (_usrDtosCalculatorAccessor.TryGetValue(subRankResultType, out Func<IEnumerable<TournamentPlayedDto>, int, IEnumerable<UserDto>>? calculator))
            {
                foreach (UserDto userDto in calculator.Invoke(tournamentPlayeds, currentUserId))
                {
                    SeasonMySubDetailUserResultDto seasonMySubDetailUserResultDto = new SeasonMySubDetailUserResultDto
                    {
                        FirstName = userDto.FirstName,
                        LastName = userDto.LastName,
                        Score = userDto.Score,
                        SubRankResultType = subRankResultType
                    };

                    result.Add(seasonMySubDetailUserResultDto);
                }
            }

            return result;
        }

        private static IDictionary<SubRankResultType, Func<IEnumerable<TournamentPlayedDto>, int, IEnumerable<UserDto>>> _usrDtosCalculatorAccessor = new Dictionary<SubRankResultType, Func<IEnumerable<TournamentPlayedDto>, int, IEnumerable<UserDto>>>
        {
            { SubRankResultType.ALL_VICTIM, GetAllVictim() },
            { SubRankResultType.ALL_ELIMINATOR, GetAllEliminator() }
        };

        private static Func<IEnumerable<TournamentPlayedDto>, int, IEnumerable<UserDto>> GetAllVictim()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
                                        .Where(pla => pla.UserId == currentUserId)
                                        .SelectMany(pla => pla.Eliminations)
                                        .GroupBy(
                                            eli => eli.UserVictimId,
                                            eli => new
                                            {
                                                eli.FirstNameVictim,
                                                eli.LastNameVictim
                                            }
                                        )
                                        .Select(gr => new UserDto
                                        {
                                            Id = gr.Key,
                                            FirstName = gr.FirstOrDefault()!.FirstNameVictim,
                                            LastName = gr.FirstOrDefault()!.LastNameVictim,
                                            Score = gr.Count()
                                        })
                                        .OrderByDescending(sel => sel.Score)
                                        .ThenBy(sel => sel.LastName);
            };
        }

        private static Func<IEnumerable<TournamentPlayedDto>, int, IEnumerable<UserDto>> GetAllEliminator()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
                                        .Where(pla => pla.UserId == currentUserId)
                                        .SelectMany(pla => pla.Victimisations)
                                        .GroupBy(
                                            eli => eli.UserEliminatorId,
                                            eli => new
                                            {
                                                eli.FirstNameEliminator,
                                                eli.LastNameEliminator
                                            }
                                        )
                                        .Select(gr => new UserDto
                                        {
                                            Id = gr.Key,
                                            FirstName = gr.FirstOrDefault()!.FirstNameEliminator,
                                            LastName = gr.FirstOrDefault()!.LastNameEliminator,
                                            Score = gr.Count()
                                        })
                                        .OrderByDescending(sel => sel.Score)
                                        .ThenBy(sel => sel.LastName);
            };
        }
    }
}
