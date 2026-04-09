using System;
using System.Collections.Generic;
using System.Linq;

namespace net.erlenddahl.Extensions.IEnumerableExtensions
{
    public static class SafeMaxExtensions
    {
        /// <summary>
        /// Does exactly the same as the Max() function, but returns a default value if the source collection is null or empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double SafeMax(this IEnumerable<double> source, double defaultValue = double.MinValue)
        {
            if (source == null || !source.Any()) return defaultValue;
            return source.Max();
        }

        /// <summary>
        /// Does exactly the same as the Max() function, but returns a default value if the source collection is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double SafeMax<T>(this IEnumerable<T> source, Func<T, double> target, double defaultValue = double.MinValue)
        {
            if (source == null || !source.Any()) return defaultValue;
            return source.Max(target);
        }

        /// <summary>
        /// Does exactly the same as the Max() function, but returns a default value if the source collection is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime SafeMax<T>(this IEnumerable<T> source, Func<T, DateTime> target, DateTime? defaultValue = null)
        {
            if (source == null || !source.Any()) return defaultValue ?? DateTime.MinValue;
            return source.Max(target);
        }
    }
}