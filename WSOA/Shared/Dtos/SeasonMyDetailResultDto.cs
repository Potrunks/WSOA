using WSOA.Shared.Resources;

namespace WSOA.Shared.Dtos
{
    public class SeasonMyDetailResultDto
    {
        public SeasonMyDetailResultDto()
        {
            SubDetails = new List<SeasonMySubDetailResultDto>();
        }

        public SeasonMyDetailResultDto(RankResultType rankResultType, int currentUsrId, IEnumerable<TournamentPlayedDto> tournamentPlayeds)
        {
            RankResultType = rankResultType;

            if (_scoreCalculatorAccessor.TryGetValue(rankResultType, out Func<IEnumerable<TournamentPlayedDto>, int, int>? calculator))
            {
                Score = calculator.Invoke(tournamentPlayeds, currentUsrId);
            }

            SubDetails = new List<SeasonMySubDetailResultDto>();
            foreach (SubRankResultType subRankResultType in rankResultType.GetSubRankResultTypes())
            {
                List<SeasonMySubDetailResultDto> dtos = subRankResultType.CreateSeasonMySubDetailResultDto(tournamentPlayeds, currentUsrId);
                foreach (SeasonMySubDetailResultDto dto in dtos)
                {
                    if (!dto.IsNoResult(false))
                    {
                        SubDetails.Add(dto);
                    }
                }
            }
        }

        public RankResultType RankResultType { get; set; }

        public int Score { get; set; }

        public List<SeasonMySubDetailResultDto> SubDetails { get; set; }

        private IDictionary<RankResultType, Func<IEnumerable<TournamentPlayedDto>, int, int>> _scoreCalculatorAccessor = new Dictionary<RankResultType, Func<IEnumerable<TournamentPlayedDto>, int, int>>
        {
            { RankResultType.RANK, GetRank() },
            { RankResultType.PROFITABILITY, GetProfitability() },
            { RankResultType.ELIMINATOR, GetEliminationsGiven() },
            { RankResultType.VICTIM, GetEliminationsTaken() },
            { RankResultType.BONUS, GetNbBonusTournamentEarned() }
        };

        private static Func<IEnumerable<TournamentPlayedDto>, int, int> GetRank()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                Dictionary<int, int> rankByUserId = tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
                                                                     .GroupBy(pla => pla.UserId)
                                                                     .Select(grouped =>
                                                                     {
                                                                         return new
                                                                         {
                                                                             UserId = grouped.Key,
                                                                             Score = grouped.Sum(gr => gr.Points) + grouped.SelectMany(gr => gr.BonusTournamentEarneds).Sum(bon => bon.Occurence * bon.Points)
                                                                         };
                                                                     })
                                                                     .OrderByDescending(r => r.Score)
                                                                     .Select((r, id) =>
                                                                     {
                                                                         return new
                                                                         {
                                                                             r.UserId,
                                                                             Rank = id + 1
                                                                         };
                                                                     })
                                                                     .ToDictionary(r => r.UserId, r => r.Rank);

                if (rankByUserId.TryGetValue(currentUserId, out int rank))
                {
                    return rank;
                }
                else
                {
                    return 0;
                }
            };
        }

        private static Func<IEnumerable<TournamentPlayedDto>, int, int> GetProfitability()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.Where(tou => tou.PlayerResults.Select(pla => pla.UserId).Contains(currentUserId))
                                        .Select(tou =>
                                        {
                                            PlayerResultDto playerResult = tou.PlayerResults.Single(pla => pla.UserId == currentUserId);
                                            return playerResult.TotalWinningAmount - tou.BuyIn - (tou.BuyIn * playerResult.TotalRebuy) - (tou.BuyIn * playerResult.TotalAddon);
                                        })
                                        .Sum();
            };
        }

        private static Func<IEnumerable<TournamentPlayedDto>, int, int> GetEliminationsGiven()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
                                        .Where(pla => pla.UserId == currentUserId)
                                        .SelectMany(pla => pla.Eliminations)
                                        .Count();
            };
        }

        private static Func<IEnumerable<TournamentPlayedDto>, int, int> GetEliminationsTaken()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
                                        .Where(pla => pla.UserId == currentUserId)
                                        .SelectMany(pla => pla.Victimisations)
                                        .Count();
            };
        }

        private static Func<IEnumerable<TournamentPlayedDto>, int, int> GetNbBonusTournamentEarned()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
                                        .Where(pla => pla.UserId == currentUserId)
                                        .SelectMany(pla => pla.BonusTournamentEarneds)
                                        .Count();
            };
        }
    }
}
