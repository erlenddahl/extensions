using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.IEnumerableExtensions
{
    public static class SafeAverageExtensions
    {
        /// <summary>
        /// Does exactly the same as the Average() function, but returns a default value if the source collection is null or empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double SafeAverage(this IEnumerable<double> source, double defaultValue = double.MinValue)
        {
            if (source == null || !source.Any()) return defaultValue;
            return source.Average();
        }

        /// <summary>
        /// Does exactly the same as the Average() function, but returns a default value if the source collection is null or empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double SafeAverage(this IEnumerable<long> source, double defaultValue = double.MinValue)
        {
            if (source == null || !source.Any()) return defaultValue;
            return source.Average();
        }

        /// <summary>
        /// Does exactly the same as the Average() function, but returns a default value if the source collection is null or empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double SafeAverage(this IEnumerable<int> source, double defaultValue = double.MinValue)
        {
            if (source == null || !source.Any()) return defaultValue;
            return source.Average();
        }

        /// <summary>
        /// Does exactly the same as the Average() function, but returns a default value if the source collection is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double SafeAverage<T>(this IEnumerable<T> source, Func<T, double> target, double defaultValue = double.MinValue)
        {
            if (source == null || !source.Any()) return defaultValue;
            return source.Average(target);
        }
    }
}