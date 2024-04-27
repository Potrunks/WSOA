using WSOA.Shared.Resources;

namespace WSOA.Shared.Dtos
{
    public class SeasonResultDto
    {
        public SeasonResultDto()
        {
            RankResults = new Dictionary<RankResultType, List<RankResultDto>>();
        }

        public SeasonResultDto(string season, IEnumerable<TournamentPlayedDto> tournamentPlayeds, IEnumerable<RankResultType> rankResultTypes)
        {
            Season = season;
            NbTournamentPlayed = tournamentPlayeds.Count();

            IEnumerable<PlayerResultDto> allPlayerResults = tournamentPlayeds.SelectMany(tou => tou.PlayerResults);
            IEnumerable<PlayerResultDto> previousPlayerResults = tournamentPlayeds.OrderBy(tou => tou.StartDate)
                                                                                  .Take(tournamentPlayeds.Count() - 1)
                                                                                  .SelectMany(tou => tou.PlayerResults);

            RankResults = new Dictionary<RankResultType, List<RankResultDto>>();
            foreach (RankResultType rankResultType in rankResultTypes)
            {
                if (_rankResultsAccessor.TryGetValue(rankResultType, out Func<IEnumerable<PlayerResultDto>, IEnumerable<PlayerResultDto>, List<RankResultDto>>? function))
                {
                    RankResults.Add(rankResultType, function.Invoke(allPlayerResults, previousPlayerResults));
                }
                else
                {
                    RankResults.Add(rankResultType, new List<RankResultDto>());
                }
            }
        }

        public string Season { get; set; }

        public int NbTournamentPlayed { get; set; }

        public IDictionary<RankResultType, List<RankResultDto>> RankResults { get; set; }

        private IDictionary<RankResultType, Func<IEnumerable<PlayerResultDto>, IEnumerable<PlayerResultDto>, List<RankResultDto>>> _rankResultsAccessor = new Dictionary<RankResultType, Func<IEnumerable<PlayerResultDto>, IEnumerable<PlayerResultDto>, List<RankResultDto>>>
        {
            { RankResultType.POINTS, GetPointsRankResultDtos() },
            { RankResultType.BONUS, GetBonusRankResultDtos() },
            { RankResultType.ELIMINATOR, GetEliminationRankResultDtos() },
            { RankResultType.VICTIM, GetVictimRankResultDtos() },
            { RankResultType.PROFITABILITY, GetProfitabilityRankResultDtos() }
        };

        private static Func<IEnumerable<PlayerResultDto>, IEnumerable<PlayerResultDto>, List<RankResultDto>> GetPointsRankResultDtos()
        {
            return (allPlayerResults, previousPlayerResults) =>
            {
                List<RankResultDto> currentRankResults = (
                        from pla in allPlayerResults
                        group pla by pla.UserId into grouped
                        select new RankResultDto
                        {
                            FirstName = grouped.First().FirstName,
                            LastName = grouped.First().LastName,
                            Score = grouped.Sum(gr => gr.Points) + grouped.SelectMany(gr => gr.BonusTournamentEarneds).Sum(bon => bon.Occurence * bon.Points),
                            UserId = grouped.First().UserId
                        }
                    )
                    .OrderByDescending(tou => tou.Score)
                    .ThenBy(tou => tou.LastName)
                    .Select((rnk, id) => new RankResultDto
                    {
                        FirstName = rnk.FirstName,
                        LastName = rnk.LastName,
                        Score = rnk.Score,
                        UserId = rnk.UserId,
                        Rank = id + 1
                    })
                    .ToList();

                IDictionary<int, RankResultDto> previousRankResults = (
                        from pla in previousPlayerResults
                        group pla by pla.UserId into grouped
                        select new RankResultDto
                        {
                            FirstName = grouped.First().FirstName,
                            LastName = grouped.First().LastName,
                            Score = grouped.Sum(gr => gr.Points) + grouped.SelectMany(gr => gr.BonusTournamentEarneds).Sum(bon => bon.Occurence * bon.Points),
                            UserId = grouped.First().UserId
                        }
                    )
                    .OrderByDescending(tou => tou.Score)
                    .ThenBy(tou => tou.LastName)
                    .Select((rnk, id) => new RankResultDto
                    {
                        FirstName = rnk.FirstName,
                        LastName = rnk.LastName,
                        Score = rnk.Score,
                        UserId = rnk.UserId,
                        Rank = id + 1
                    })
                    .ToDictionary(rnk => rnk.UserId);

                return CalculateEvolution(currentRankResults, previousRankResults);
            };
        }

        private static Func<IEnumerable<PlayerResultDto>, IEnumerable<PlayerResultDto>, List<RankResultDto>> GetBonusRankResultDtos()
        {
            return (allPlayerResults, previousPlayerResults) =>
            {
                List<RankResultDto> currentRankResults = (
                        from pla in allPlayerResults
                        group pla by pla.UserId into grouped
                        select new RankResultDto
                        {
                            FirstName = grouped.First().FirstName,
                            LastName = grouped.First().LastName,
                            Score = grouped.SelectMany(gr => gr.BonusTournamentEarneds).Count(),
                            UserId = grouped.First().UserId
                        }
                    )
                    .OrderByDescending(tou => tou.Score)
                    .ThenBy(tou => tou.LastName)
                    .Select((rnk, id) => new RankResultDto
                    {
                        FirstName = rnk.FirstName,
                        LastName = rnk.LastName,
                        Score = rnk.Score,
                        UserId = rnk.UserId,
                        Rank = id + 1
                    })
                    .ToList();

                IDictionary<int, RankResultDto> previousRankResults = (
                        from pla in previousPlayerResults
                        group pla by pla.UserId into grouped
                        select new RankResultDto
                        {
                            FirstName = grouped.First().FirstName,
                            LastName = grouped.First().LastName,
                            Score = grouped.SelectMany(gr => gr.BonusTournamentEarneds).Count(),
                            UserId = grouped.First().UserId
                        }
                    )
                    .OrderByDescending(tou => tou.Score)
                    .ThenBy(tou => tou.LastName)
                    .Select((rnk, id) => new RankResultDto
                    {
                        FirstName = rnk.FirstName,
                        LastName = rnk.LastName,
                        Score = rnk.Score,
                        UserId = rnk.UserId,
                        Rank = id + 1
                    })
                    .ToDictionary(rnk => rnk.UserId);

                return CalculateEvolution(currentRankResults, previousRankResults);
            };
        }

        private static Func<IEnumerable<PlayerResultDto>, IEnumerable<PlayerResultDto>, List<RankResultDto>> GetEliminationRankResultDtos()
        {
            return (allPlayerResults, previousPlayerResults) =>
            {
                List<RankResultDto> currentRankResults = (
                        from pla in allPlayerResults
                        group pla by pla.UserId into grouped
                        select new RankResultDto
                        {
                            FirstName = grouped.First().FirstName,
                            LastName = grouped.First().LastName,
                            Score = grouped.SelectMany(gr => gr.Eliminations).Count(),
                            UserId = grouped.First().UserId
                        }
                    )
                    .OrderByDescending(tou => tou.Score)
                    .ThenBy(tou => tou.LastName)
                    .Select((rnk, id) => new RankResultDto
                    {
                        FirstName = rnk.FirstName,
                        LastName = rnk.LastName,
                        Score = rnk.Score,
                        UserId = rnk.UserId,
                        Rank = id + 1
                    })
                    .ToList();

                IDictionary<int, RankResultDto> previousRankResults = (
                        from pla in previousPlayerResults
                        group pla by pla.UserId into grouped
                        select new RankResultDto
                        {
                            FirstName = grouped.First().FirstName,
                            LastName = grouped.First().LastName,
                            Score = grouped.SelectMany(gr => gr.Eliminations).Count(),
                            UserId = grouped.First().UserId
                        }
                    )
                    .OrderByDescending(tou => tou.Score)
                    .ThenBy(tou => tou.LastName)
                    .Select((rnk, id) => new RankResultDto
                    {
                        FirstName = rnk.FirstName,
                        LastName = rnk.LastName,
                        Score = rnk.Score,
                        UserId = rnk.UserId,
                        Rank = id + 1
                    })
                    .ToDictionary(rnk => rnk.UserId);

                return CalculateEvolution(currentRankResults, previousRankResults);
            };
        }

        private static Func<IEnumerable<PlayerResultDto>, IEnumerable<PlayerResultDto>, List<RankResultDto>> GetVictimRankResultDtos()
        {
            return (allPlayerResults, previousPlayerResults) =>
            {
                List<RankResultDto> currentRankResults = (
                        from pla in allPlayerResults
                        group pla by pla.UserId into grouped
                        select new RankResultDto
                        {
                            FirstName = grouped.First().FirstName,
                            LastName = grouped.First().LastName,
                            Score = grouped.SelectMany(gr => gr.Victimisations).Count(),
                            UserId = grouped.First().UserId
                        }
                    )
                    .OrderBy(tou => tou.Score)
                    .ThenBy(tou => tou.LastName)
                    .Select((rnk, id) => new RankResultDto
                    {
                        FirstName = rnk.FirstName,
                        LastName = rnk.LastName,
                        Score = rnk.Score,
                        UserId = rnk.UserId,
                        Rank = id + 1
                    })
                    .ToList();

                IDictionary<int, RankResultDto> previousRankResults = (
                        from pla in previousPlayerResults
                        group pla by pla.UserId into grouped
                        select new RankResultDto
                        {
                            FirstName = grouped.First().FirstName,
                            LastName = grouped.First().LastName,
                            Score = grouped.SelectMany(gr => gr.Victimisations).Count(),
                            UserId = grouped.First().UserId
                        }
                    )
                    .OrderBy(tou => tou.Score)
                    .ThenBy(tou => tou.LastName)
                    .Select((rnk, id) => new RankResultDto
                    {
                        FirstName = rnk.FirstName,
                        LastName = rnk.LastName,
                        Score = rnk.Score,
                        UserId = rnk.UserId,
                        Rank = id + 1
                    })
                    .ToDictionary(rnk => rnk.UserId);

                return CalculateEvolution(currentRankResults, previousRankResults);
            };
        }

        private static Func<IEnumerable<PlayerResultDto>, IEnumerable<PlayerResultDto>, List<RankResultDto>> GetProfitabilityRankResultDtos()
        {
            return (allPlayerResults, previousPlayerResults) =>
            {
                List<RankResultDto> currentRankResults = (
                        from pla in allPlayerResults
                        group pla by pla.UserId into grouped
                        select new RankResultDto
                        {
                            FirstName = grouped.First().FirstName,
                            LastName = grouped.First().LastName,
                            Score = grouped.Sum(gr => gr.TotalWinningAmount) - grouped.Sum(gr => gr.BuyIn + (gr.BuyIn * gr.TotalRebuy) + (gr.BuyIn * gr.TotalAddon)),
                            UserId = grouped.First().UserId
                        }
                    )
                    .OrderByDescending(tou => tou.Score)
                    .ThenBy(tou => tou.LastName)
                    .Select((rnk, id) => new RankResultDto
                    {
                        FirstName = rnk.FirstName,
                        LastName = rnk.LastName,
                        Score = rnk.Score,
                        UserId = rnk.UserId,
                        Rank = id + 1
                    })
                    .ToList();

                IDictionary<int, RankResultDto> previousRankResults = (
                        from pla in previousPlayerResults
                        group pla by pla.UserId into grouped
                        select new RankResultDto
                        {
                            FirstName = grouped.First().FirstName,
                            LastName = grouped.First().LastName,
                            Score = grouped.Sum(gr => gr.TotalWinningAmount) - grouped.Sum(gr => gr.BuyIn + (gr.BuyIn * gr.TotalRebuy) + (gr.BuyIn * gr.TotalAddon)),
                            UserId = grouped.First().UserId
                        }
                    )
                    .OrderByDescending(tou => tou.Score)
                    .ThenBy(tou => tou.LastName)
                    .Select((rnk, id) => new RankResultDto
                    {
                        FirstName = rnk.FirstName,
                        LastName = rnk.LastName,
                        Score = rnk.Score,
                        UserId = rnk.UserId,
                        Rank = id + 1
                    })
                    .ToDictionary(rnk => rnk.UserId);

                return CalculateEvolution(currentRankResults, previousRankResults);
            };
        }

        private static List<RankResultDto> CalculateEvolution(List<RankResultDto> currentRankResults, IDictionary<int, RankResultDto> previousRankResults)
        {
            foreach (RankResultDto rankResult in currentRankResults)
            {
                if (previousRankResults.TryGetValue(rankResult.UserId, out RankResultDto? previousRankResult))
                {
                    rankResult.Evolution = previousRankResult.Rank - rankResult.Rank;
                }
                else
                {
                    rankResult.Evolution = 0;
                }
            }

            return currentRankResults;
        }
    }
}
