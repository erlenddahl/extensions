using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions.TimeSpanExtensions
{
    public static class IEnumerables
    {
        /// <summary>
        /// Returns the sum of the given timespans.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static TimeSpan DurationSum<T>(this IEnumerable<T> source, Func<T, TimeSpan> target)
        {
            return new TimeSpan(source.Sum(p => target(p).Ticks));
        }
    }
}
