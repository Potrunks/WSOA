namespace WSOA.Shared.Resources
{
    public enum RankResultType
    {
        POINTS = 0,
        ELIMINATOR = 1,
        VICTIM = 2,
        PROFITABILITY = 3,
        BONUS = 4,
        RANK = 5
    }

    public static class RankResultTypeUtil
    {
        private static IDictionary<RankResultType, string> _labelsAccessor = new Dictionary<RankResultType, string>
        {
            { RankResultType.POINTS, "Classement" },
            { RankResultType.ELIMINATOR, "Elimination effectuée" },
            { RankResultType.VICTIM, "Elimination subie" },
            { RankResultType.PROFITABILITY, "Rentabilité" },
            { RankResultType.BONUS, "Bonus" },
            { RankResultType.RANK, "Classement" }
        };

        private static IDictionary<RankResultType, string> _unitiesAccessor = new Dictionary<RankResultType, string>
        {
            { RankResultType.POINTS, "pts" },
            { RankResultType.PROFITABILITY, "euros" }
        };

        private static IDictionary<RankResultType, Func<int, string>> _unitiesByValueAccessor = new Dictionary<RankResultType, Func<int, string>>
        {
            { RankResultType.RANK, GetRankUnityByValue() }
        };

        private static IDictionary<RankResultType, List<SubRankResultType>> _subRankResultTypesAccessor = new Dictionary<RankResultType, List<SubRankResultType>>
        {
            { RankResultType.RANK, new List<SubRankResultType>
                {
                    SubRankResultType.TOURNAMENT_WIN,
                    SubRankResultType.PAID_PLACE,
                    SubRankResultType.FINAL_TABLE,
                    SubRankResultType.TOURNAMENT_PRESENT
                }
            },
            { RankResultType.PROFITABILITY, new List<SubRankResultType>
                {
                    SubRankResultType.MONEY_SPENT,
                    SubRankResultType.MONEY_WIN,
                    SubRankResultType.TOTAL_REBUY,
                    SubRankResultType.TOTAL_ADDON
                }
            },
            { RankResultType.ELIMINATOR, new List<SubRankResultType>
                {
                    SubRankResultType.ALL_VICTIM
                }
            },
            { RankResultType.VICTIM, new List<SubRankResultType>
                {
                    SubRankResultType.ALL_ELIMINATOR
                }
            },
            { RankResultType.BONUS, new List<SubRankResultType>
                {
                    SubRankResultType.FIRST_RANKED_KILLED,
                    SubRankResultType.PREVIOUS_WINNER_KILLED,
                    SubRankResultType.FOUR_OF_A_KIND,
                    SubRankResultType.STRAIGHT_FLUSH,
                    SubRankResultType.ROYAL_STRAIGHT_FLUSH
                }
            }
        };

        public static string GetLabel(this RankResultType type)
        {
            if (_labelsAccessor.TryGetValue(type, out string? label))
            {
                return label;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetUnity(this RankResultType type, int? value = null)
        {
            if (_unitiesByValueAccessor.TryGetValue(type, out Func<int, string>? func) && value != null)
            {
                return func.Invoke(value.Value);
            }

            if (_unitiesAccessor.TryGetValue(type, out string? label))
            {
                return label;
            }

            return string.Empty;
        }

        public static List<SubRankResultType> GetSubRankResultTypes(this RankResultType type)
        {
            if (_subRankResultTypesAccessor.TryGetValue(type, out List<SubRankResultType>? subRankResultTypes))
            {
                return subRankResultTypes;
            }
            else
            {
                return new List<SubRankResultType>();
            }
        }

        private static Func<int, string> GetRankUnityByValue()
        {
            return (value) =>
            {
                if (value == 1)
                {
                    return "er";
                }

                if (value > 1)
                {
                    return "eme";
                }

                return string.Empty;
            };
        }
    }
}
