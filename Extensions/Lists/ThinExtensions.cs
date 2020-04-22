using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Lists
{
    public static class ThinExtensions
    {
        /// <summary>
        /// Thins the given list by returning every Nth element so that the returned count is equal
        /// to remainingCount. The first and last elements will always be returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="remainingCount"></param>
        /// <returns></returns>
        public static IEnumerable<T> Thin<T>(this IList<T> list, int remainingCount)
        {
            if (remainingCount < 2) throw new ArgumentException("RemainingCount must be at least 2.");
            if (remainingCount >= list.Count)
            {
                foreach (var i in list)
                    yield return i;
                yield break;
            }
            var returnEvery = (list.Count - 1) / (double)(remainingCount - 1);
            for (var i = 0d; i < list.Count; i += returnEvery)
                yield return list[(int)Math.Round(i, 0, MidpointRounding.AwayFromZero)];
        }

        /// <summary>
        /// Thins the given list by returning every Nth element so that the returned count is equal
        /// to remainingCount. The first and last elements will always be returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="remainingCount"></param>
        /// <returns></returns>
        public static IEnumerable<T> Thin<T>(this IEnumerable<T> list, int remainingCount)
        {
            return list.ToArray().Thin(remainingCount);
        }
    }
}
