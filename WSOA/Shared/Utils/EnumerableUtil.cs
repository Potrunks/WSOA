namespace WSOA.Shared.Utils
{
    public static class EnumerableUtil
    {
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable, Random random)
        {
            return enumerable.ElementAt(random.Next(enumerable.Count()));
        }
    }
}
