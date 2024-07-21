using WSOA.Shared.Resources;
using WSOA.Shared.Utils;

namespace WSOA.Shared.Dtos
{
    public class SeasonMySubDetailResultDto
    {
        public SeasonMySubDetailResultDto()
        {

        }

        public SeasonMySubDetailResultDto(IEnumerable<TournamentPlayedDto> tournamentPlayeds, SubRankResultType subRankResultType, int currentUserId)
        {
            SubRankResultType = subRankResultType;

            if (_scoreCalculatorAccessor.TryGetValue(subRankResultType, out Func<IEnumerable<TournamentPlayedDto>, int, int>? calculator))
            {
                Score = calculator.Invoke(tournamentPlayeds, currentUserId);
            }
        }

        public SubRankResultType SubRankResultType { get; set; }

        public int Score { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        private IDictionary<SubRankResultType, Func<IEnumerable<TournamentPlayedDto>, int, int>> _scoreCalculatorAccessor = new Dictionary<SubRankResultType, Func<IEnumerable<TournamentPlayedDto>, int, int>>
        {
            { SubRankResultType.TOURNAMENT_WIN, GetNbTournamentWin() },
            { SubRankResultType.PAID_PLACE, GetNbPaidPlace() },
            { SubRankResultType.FINAL_TABLE, GetNbFinalTable() },
            { SubRankResultType.TOURNAMENT_PRESENT, GetNbParticipation() },
            { SubRankResultType.MONEY_SPENT, GetNbMoneySpent() },
            { SubRankResultType.MONEY_WIN, GetNbMoneyWin() },
            { SubRankResultType.TOTAL_REBUY, GetNbRebuy() },
            { SubRankResultType.TOTAL_ADDON, GetNbAddon() },
            { SubRankResultType.FIRST_RANKED_KILLED, GetNbBonusTournamentEarned(BonusTournamentResources.FIRST_RANKED_KILLED) },
            { SubRankResultType.PREVIOUS_WINNER_KILLED, GetNbBonusTournamentEarned(BonusTournamentResources.PREVIOUS_WINNER_KILLED) },
            { SubRankResultType.FOUR_OF_A_KIND, GetNbBonusTournamentEarned(BonusTournamentResources.FOUR_OF_A_KIND) },
            { SubRankResultType.STRAIGHT_FLUSH, GetNbBonusTournamentEarned(BonusTournamentResources.STRAIGHT_FLUSH) },
            { SubRankResultType.ROYAL_STRAIGHT_FLUSH, GetNbBonusTournamentEarned(BonusTournamentResources.ROYAL_STRAIGHT_FLUSH) }
        };

        private static Func<IEnumerable<TournamentPlayedDto>, int, int> GetNbTournamentWin()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
                                        .Where(pla => pla.Position == 1 && pla.UserId == currentUserId)
                                        .Count();
            };
        }

        private static Func<IEnumerable<TournamentPlayedDto>, int, int> GetNbPaidPlace()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
                                        .Where(pla => pla.TotalWinningAmount > 0 && pla.UserId == currentUserId)
                                        .Count();
            };
        }

        private static Func<IEnumerable<TournamentPlayedDto>, int, int> GetNbFinalTable()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
                                        .Where(pla => pla.WasFinalTable && pla.UserId == currentUserId)
                                        .Count();
            };
        }

        private static Func<IEnumerable<TournamentPlayedDto>, int, int> GetNbParticipation()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
                                        .Where(pla => pla.PresenceStateCode == PresenceStateResources.PRESENT_CODE && pla.UserId == currentUserId)
                                        .Count();
            };
        }

        private static Func<IEnumerable<TournamentPlayedDto>, int, int> GetNbMoneySpent()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.Where(tou => tou.PlayerResults.Select(pla => pla.UserId).Contains(currentUserId))
                                        .Select(tou =>
                                        {
                                            PlayerResultDto playerResult = tou.PlayerResults.Single(pla => pla.UserId == currentUserId);
                                            return tou.BuyIn * (1 + playerResult.TotalRebuy + playerResult.TotalAddon);
                                        })
                                        .Sum();
            };
        }

        private static Func<IEnumerable<TournamentPlayedDto>, int, int> GetNbMoneyWin()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
                                        .Where(pla => pla.UserId == currentUserId)
                                        .Sum(pla => pla.TotalWinningAmount);
            };
        }

        private static Func<IEnumerable<TournamentPlayedDto>, int, int> GetNbRebuy()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
                                        .Where(pla => pla.UserId == currentUserId)
                                        .Sum(pla => pla.TotalRebuy);
            };
        }

        private static Func<IEnumerable<TournamentPlayedDto>, int, int> GetNbAddon()
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
                                        .Where(pla => pla.UserId == currentUserId)
                                        .Sum(pla => pla.TotalAddon);
            };
        }

        private static Func<IEnumerable<TournamentPlayedDto>, int, int> GetNbBonusTournamentEarned(string bonusTournamentWanted)
        {
            return (tournamentPlayeds, currentUserId) =>
            {
                return tournamentPlayeds.SelectMany(tou => tou.PlayerResults)
                                        .Where(pla => pla.UserId == currentUserId)
                                        .SelectMany(pla => pla.BonusTournamentEarneds)
                                        .Where(bon => bon.Code == bonusTournamentWanted)
                                        .Sum(bon => bon.Occurence);
            };
        }

        public string GetUnity()
        {
            switch (SubRankResultType)
            {
                case SubRankResultType.TOURNAMENT_WIN:
                case SubRankResultType.PAID_PLACE:
                case SubRankResultType.FINAL_TABLE:
                case SubRankResultType.TOURNAMENT_PRESENT:
                case SubRankResultType.MONEY_SPENT:
                case SubRankResultType.MONEY_WIN:
                case SubRankResultType.TOTAL_REBUY:
                case SubRankResultType.TOTAL_ADDON:
                case SubRankResultType.FIRST_RANKED_KILLED:
                case SubRankResultType.PREVIOUS_WINNER_KILLED:
                case SubRankResultType.FOUR_OF_A_KIND:
                case SubRankResultType.STRAIGHT_FLUSH:
                case SubRankResultType.ROYAL_STRAIGHT_FLUSH:
                    return SubRankResultType.GetUnity();
                case SubRankResultType.ALL_VICTIM:
                case SubRankResultType.ALL_ELIMINATOR:
                    return $"fois {StringFormatUtil.ToFormatFullName(FirstName, LastName)}";
                default:
                    return string.Empty;
            }
        }

        public bool IsNoResult(bool isZeroScoreAccepted)
        {
            return !isZeroScoreAccepted && Score == 0;
        }
    }
}
