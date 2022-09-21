using System;
using System.Collections.Generic;

namespace Extensions.IListExtensions
{
    public static partial class IndexOfSequenceExtensions
    {
        /// <summary>
        /// Return the starting index of the last sequence that matches the
        /// given predicate and is of (at least) the given length.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <param name="minimumLength"></param>
        /// <returns></returns>
        public static (int Start, int End) LastIndexOfSequence<T>(this IList<T> list, Func<T, bool> predicate, int minimumLength)
        {
            var startIndex = -1;
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (!predicate(list[i]) || i == 0)
                {
                    if (startIndex >= 0 && startIndex - i >= minimumLength) return (i + (i == 0 ? 0 : 1), startIndex);

                    startIndex = -1;
                    continue;
                }

                if (startIndex < 0) startIndex = i;
            }

            return (-1, -1);
        }
    }
}