using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Lists
{
    public static class PairwiseExtensions
    {
        public static IEnumerable<TResult> Pairwise<TSource, TResult>(this IList<TSource> list, Func<TSource, TSource, TResult> aggregator)
        {
            return list.Where((e, i) => i > 0).Select((e, i) => aggregator(list[i], e));
        }

        public static IEnumerable<Tuple<TSource, TSource>> Pairwise<TSource>(this IList<TSource> list)
        {
            return list.Pairwise(Tuple.Create);
        }
    }
}
