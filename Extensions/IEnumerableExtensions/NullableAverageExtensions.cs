using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.IEnumerableExtensions
{
    public static class NullableAverageExtensions
    {
        /// <summary>
        /// Does exactly the same as the Average() function, but returns a default value if the source collection is null or empty, or contains only null values.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double? NullableAverage(this IEnumerable<double?> source, double? defaultValue = null)
        {
            if (source == null || !source.Any()) return defaultValue;
            var nonNull = source.Where(p => p.HasValue).ToArray();
            if (!nonNull.Any()) return defaultValue;
            return nonNull.Average();
        }

        /// <summary>
        /// Does exactly the same as the Average() function, but returns a default value if the source collection is null or empty, or contains only null values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double? NullableAverage<T>(this IEnumerable<T> source, Func<T, double?> target, double? defaultValue = null)
        {
            if (source == null || !source.Any()) return defaultValue;
            var nonNull = source.Select(target).Where(p => p.HasValue).ToArray();
            if (!nonNull.Any()) return defaultValue;
            return nonNull.Average();
        }
    }
}