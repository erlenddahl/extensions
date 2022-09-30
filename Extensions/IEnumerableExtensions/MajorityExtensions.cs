using System.Collections.Generic;
using Extensions.DictionaryExtensions;

namespace Extensions.IEnumerableExtensions
{
    public static class MajorityExtensions
    {
        /// <summary>
        /// Given a list of Ts, this function will return the most common element.
        /// For example: [1,2,3,3,3,4,4,5].Majority() will return 3.
        /// If T is an object, reference equality will be used. Null objects will be counted separately, and the function may return null if null objects are the most common.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T Majority<T>(this IEnumerable<T> list)
        {
            var nulls = 0;
            var counts = new Dictionary<T, int>();
            foreach (var item in list)
            {
                if (item == null) nulls++;
                else counts.Increment(item);
            }

            var max = counts.MaxBy(p => p.Value);
            if (nulls > max.Value) return default;

            return max.Key;
        }
    }
}