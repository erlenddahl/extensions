using System;
using System.Collections.Generic;

namespace Extensions.IEnumerableExtensions
{
    public static class MaxByExtensions
    {
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            comparer = comparer ?? Comparer<TKey>.Default;

            TSource accumulator;
            var enumerator = source.GetEnumerator();

            try
            {
                if (!enumerator.MoveNext())
                {
                    throw new InvalidOperationException("no elements to enumerate");
                }

                accumulator = enumerator.Current;

                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;

                    if (comparer.Compare(selector(accumulator), selector(current)) < 0)
                    {
                        accumulator = current;
                    }
                }
            }
            finally
            {
                enumerator.Dispose();
            }

            return accumulator;
        }
    }
}