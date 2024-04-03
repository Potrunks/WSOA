namespace WSOA.Shared.Resources
{
    public static class TournamentPointsResources
    {
        public static IDictionary<int, int> TournamentPointAmountByPosition = new Dictionary<int, int>
        {
            { 1, 150 },
            { 2, 130 },
            { 3, 110 },
            { 4, 90 },
            { 5, 80 },
            { 6, 70 },
            { 7, 60 },
            { 8, 50 },
            { 9, 40 },
            { 10, 30 },
            { 11, 20 }
        };

        public const int MinimumPointAmount = 10;
    }
}
