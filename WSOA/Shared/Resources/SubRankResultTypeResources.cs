using WSOA.Shared.Dtos;

namespace WSOA.Shared.Resources
{
    public enum SubRankResultType
    {
        TOURNAMENT_WIN = 0,
        PAID_PLACE = 1,
        FINAL_TABLE = 2,
        TOURNAMENT_PRESENT = 3,
        MONEY_SPENT = 4,
        MONEY_WIN = 5,
        TOTAL_REBUY = 6,
        TOTAL_ADDON = 7,
        FIRST_RANKED_KILLED = 8,
        PREVIOUS_WINNER_KILLED = 9,
        FOUR_OF_A_KIND = 10,
        STRAIGHT_FLUSH = 11,
        ROYAL_STRAIGHT_FLUSH = 12,
        ALL_VICTIM = 13,
        ALL_ELIMINATOR = 14
    }

    public static class SubRankResultTypeUtil
    {
        private static IDictionary<SubRankResultType, string> _unitiesAccessor = new Dictionary<SubRankResultType, string>
        {
            { SubRankResultType.TOURNAMENT_WIN, "tournoi gagné" },
            { SubRankResultType.PAID_PLACE, "place payé" },
            { SubRankResultType.FINAL_TABLE, "table finale" },
            { SubRankResultType.TOURNAMENT_PRESENT, "participation" },
            { SubRankResultType.MONEY_SPENT, "euros dépensés" },
            { SubRankResultType.MONEY_WIN, "euros gagnés" },
            { SubRankResultType.TOTAL_REBUY, "rebuy" },
            { SubRankResultType.TOTAL_ADDON, "addon" },
            { SubRankResultType.FIRST_RANKED_KILLED, "elimination du 1er" },
            { SubRankResultType.PREVIOUS_WINNER_KILLED, "elimination d'un précédent gagnant" },
            { SubRankResultType.FOUR_OF_A_KIND, "carré" },
            { SubRankResultType.STRAIGHT_FLUSH, "quinte flush" },
            { SubRankResultType.ROYAL_STRAIGHT_FLUSH, "quinte flush royal" }
        };

        public static string GetUnity(this SubRankResultType type)
        {
            if (_unitiesAccessor.TryGetValue(type, out string? label))
            {
                return label;
            }
            else
            {
                return string.Empty;
            }
        }

        public static List<SeasonMySubDetailResultDto> CreateSeasonMySubDetailResultDto(this SubRankResultType subRankResultType, IEnumerable<TournamentPlayedDto> tournamentPlayeds, int currentUserId)
        {
            switch (subRankResultType)
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
                    return new List<SeasonMySubDetailResultDto> { new SeasonMySubDetailResultDto(tournamentPlayeds, subRankResultType, currentUserId) };
                case SubRankResultType.ALL_VICTIM:
                case SubRankResultType.ALL_ELIMINATOR:
                    List<SeasonMySubDetailResultDto> result = new List<SeasonMySubDetailResultDto>();
                    result.AddRange(SeasonMySubDetailUserResultDto.CreateSeasonMySubDetailUserResultDtos(tournamentPlayeds, subRankResultType, currentUserId));
                    return result;
                default:
                    return new List<SeasonMySubDetailResultDto>();
            }
        }
    }
}
