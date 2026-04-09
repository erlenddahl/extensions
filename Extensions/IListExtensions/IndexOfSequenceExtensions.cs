using System;
using System.Collections.Generic;

namespace net.erlenddahl.Extensions.IListExtensions
{
    public static partial class IndexOfSequenceExtensions
    {
        /// <summary>
        /// Return the starting index of the first sequence that matches the
        /// given predicate and is of (at least) the given length.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <param name="minimumLength"></param>
        /// <returns></returns>
        public static (int Start, int End) IndexOfSequence<T>(this IList<T> list, Func<T, bool> predicate, int minimumLength)
        {
            var startIndex = -1;
            for (var i = 0; i < list.Count; i++)
            {
                if (!predicate(list[i]) || i == list.Count - 1)
                {
                    if (startIndex >= 0 && i - startIndex + 1 >= minimumLength) return (startIndex, i - (i == list.Count - 1 ? 0 : 1));

                    startIndex = -1;
                    continue;
                }

                if (startIndex < 0) startIndex = i;
            }

            return (-1, -1);
        }
    }
}