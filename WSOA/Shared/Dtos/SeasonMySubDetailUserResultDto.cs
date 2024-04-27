using WSOA.Shared.Resources;

namespace WSOA.Shared.Dtos
{
    public class SeasonMySubDetailUserResultDto : SeasonMySubDetailResultDto
    {
        public SeasonMySubDetailUserResultDto()
        {

        }

        public SeasonMySubDetailUserResultDto(IEnumerable<TournamentPlayedDto> tournamentPlayeds, SubRankResultType subRankResultType, int currentUserId)
        {
            SubRankResultType = subRankResultType;

            if (_usrDtoCalculatorAccessor.TryGetValue(subRankResultType, out Func<IEnumerable<TournamentPlayedDto>, int, UserDto?>? calculator))
            {
                UserDto? result = calculator.Invoke(tournamentPlayeds, currentUserId);

                if (result != null)
                {
                    FirstName = result.FirstName;
                    LastName = result.LastName;
                    Score = result.Score;
                }
            }
        }

        private IDictionary<SubRankResultType, Func<IEnumerable<TournamentPlayedDto>, int, UserDto?>> _usrDtoCalculatorAccessor = new Dictionary<SubRankResultType, Func<IEnumerable<TournamentPlayedDto>, int, UserDto?>>
        {
            { SubRankResultType.FIRST_MOST_VICTIM, GetMostVictim(1) },
            { SubRankResultType.SECOND_MOST_VICTIM, GetMostVictim(2) },
            { SubRankResultType.THIRD_MOST_VICTIM, GetMostVictim(3) },
            { SubRankResultType.FIRST_MOST_ELIMINATOR, GetMostEliminator(1) },
            { SubRankResultType.SECOND_MOST_ELIMINATOR, GetMostEliminator(2) },
            { SubRankResultType.THIRD_MOST_ELIMINATOR, GetMostEliminator(3) }
        };

        private static Func<IEnumerable<TournamentPlayedDto>, int, UserDto?> GetMostVictim(int rankWanted)
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                IEnumerable<UserDto> rawResults = tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
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
                                                                   });

                if (rankWanted <= rawResults.Count())
                {
                    return rawResults.OrderByDescending(sel => sel.Score)
                                     .ThenBy(sel => sel.LastName)
                                     .Take(rankWanted)
                                     .LastOrDefault();
                }

                return null;
            };
        }

        private static Func<IEnumerable<TournamentPlayedDto>, int, UserDto?> GetMostEliminator(int rankWanted)
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                IEnumerable<UserDto> rawResults = tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
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
                                                                   });

                if (rankWanted <= rawResults.Count())
                {
                    return rawResults.OrderByDescending(sel => sel.Score)
                                     .ThenBy(sel => sel.LastName)
                                     .Take(rankWanted)
                                     .LastOrDefault();
                }

                return null;
            };
        }
    }
}
