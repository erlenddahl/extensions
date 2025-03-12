using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.IEnumerableExtensions
{
    public static class PairwiseExtensions
    {
        public static IEnumerable<TResult> Pairwise<TSource, TResult>(this IEnumerable<TSource> list, Func<TSource, TSource, TResult> aggregator)
        {
            var previous = default(TSource);
            var isFirst = true;
            foreach (var item in list)
            {
                if (isFirst)
                {
                    previous = item;
                    isFirst = false;
                    continue;
                }

                yield return aggregator(previous, item);
                previous = item;
            }
        }

        /// <summary>
        /// Returns the given list [A, B, C] as a set of pairs [A, B], [B, C], and [C, D].
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<(TSource A, TSource B)> Pairwise<TSource>(this IEnumerable<TSource> list)
        {
            return list.Pairwise((a, b) => (a, b));
        }
    }
}
