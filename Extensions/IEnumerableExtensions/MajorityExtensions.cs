using System.Collections.Generic;
using Extensions.DictionaryExtensions;

namespace Extensions.IEnumerableExtensions
{
    public static class MajorityExtensions
    {
        public static T Majority<T>(this IEnumerable<T> list)
        {
            var counts = new Dictionary<T, int>();
            foreach (var item in list)
                counts.Increment(item);
            return counts.MaxBy(p => p.Value).Key;
        }
    }
}