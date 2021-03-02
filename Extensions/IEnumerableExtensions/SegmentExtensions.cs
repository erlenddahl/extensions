using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.IEnumerableExtensions
{
    public static class SegmentExtensions
    {
        /// <summary>
        /// Runs through the given IEnumerable, and returns it in segments of the given size after running each segment through the aggregate function.
        /// </summary>
        /// <example>
        /// new [] { 1, 2, 3, 4, 5 }.Segment(2, p => string.Join("_", p)); // "1_2", "3_4", "5"
        /// </example>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="segmentSize"></param>
        /// <param name="aggregator"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> Segment<TSource, TResult>(this IEnumerable<TSource> source, int segmentSize, Func<TSource[], TResult> aggregator)
        {
            return source.Segment(segmentSize).Select(aggregator);
        }

        /// <summary>
        /// Runs through the given IEnumerable, and returns it in segments of the given size.
        /// </summary>
        /// <example>
        /// new [] { 1, 2, 3, 4, 5 }.Segment(2); // [1, 2], [3, 4], [5]
        /// </example>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="segmentSize"></param>
        /// <returns></returns>
        public static IEnumerable<TSource[]> Segment<TSource>(this IEnumerable<TSource> source, int segmentSize)
        {
            var arr = new TSource[segmentSize];
            var curr = 0;
            foreach (var item in source)
            {
                arr[curr++] = item;
                if (curr >= segmentSize)
                {
                    yield return arr;
                    arr = new TSource[segmentSize];
                    curr = 0;
                }
            }

            if (curr > 0) yield return arr.Take(curr).ToArray();
        }
    }
}
