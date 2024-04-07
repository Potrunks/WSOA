namespace WSOA.Shared.Resources
{
    public enum RankResultType
    {
        POINTS = 0,
        ELIMINATOR = 1,
        VICTIM = 2,
        PROFITABILITY = 3,
        BONUS = 4
    }

    public static class RankResultTypeUtil
    {
        private static IDictionary<RankResultType, string> _rankResultTypeLabelsAccessor = new Dictionary<RankResultType, string>
        {
            { RankResultType.POINTS, "Classement" },
            { RankResultType.ELIMINATOR, "Elimination" },
            { RankResultType.VICTIM, "Victimisation" },
            { RankResultType.PROFITABILITY, "Rentabilité" },
            { RankResultType.BONUS, "Bonus" }
        };

        private static IDictionary<RankResultType, string> _rankResultTypeUnitiesAccessor = new Dictionary<RankResultType, string>
        {
            { RankResultType.POINTS, "pts" },
            { RankResultType.PROFITABILITY, "euros" }
        };

        public static string GetLabel(this RankResultType type)
        {
            if (_rankResultTypeLabelsAccessor.TryGetValue(type, out string? label))
            {
                return label;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetUnity(this RankResultType type)
        {
            if (_rankResultTypeUnitiesAccessor.TryGetValue(type, out string? label))
            {
                return label;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
