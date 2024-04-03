namespace WSOA.Shared.Utils
{
    public static class EnumerableUtil
    {
        /// <summary>
        /// Get a random value in a enumerable list.
        /// </summary>
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable, Random random)
        {
            return enumerable.ElementAt(random.Next(enumerable.Count()));
        }
    }
}
