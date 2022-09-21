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

        public static IEnumerable<Tuple<TSource, TSource>> Pairwise<TSource>(this IEnumerable<TSource> list)
        {
            return list.Pairwise(Tuple.Create);
        }
    }
}
