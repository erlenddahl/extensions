using System;
using System.Collections.Generic;
using System.Linq;
using Extensions.DictionaryExtensions;
using Extensions.ListExtensions;

namespace Extensions.IEnumerableExtensions
{
    public static class MajorityExtensions
    {
        /// <summary>
        /// Given a list of Ts, this function will return the most common element.
        /// For example: [1,2,3,3,3,4,4,5].Majority() will return 3.
        /// If T is an object, reference equality will be used. Null objects will be counted separately, and the function may return null if null objects are the most common.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T Majority<T>(this IEnumerable<T> list)
        {
            var nulls = 0;
            var counts = new Dictionary<T, int>();
            foreach (var item in list)
            {
                if (item == null) nulls++;
                else counts.Increment(item);
            }

            var max = counts.MaxBy(p => p.Value);
            if (nulls > max.Value) return default;

            return max.Key;
        }

        /// <summary>
        /// Calculate the median of a given value in a range around the given point.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ienumerable">The list containing the values.</param>
        /// <param name="index">The index of the list for which to calculate the median around.</param>
        /// <param name="radius">The radius of the area around the index to use for the median calculations.</param>
        /// <param name="extractor">And extractor function, extracting the value to calculate the majority of.</param>
        /// <returns></returns>
        public static TV MajorityAround<TK, TV>(this IEnumerable<TK> ienumerable, int index, int radius, Func<TK, TV> extractor)
        {
            var list = ienumerable as IList<TK> ?? ienumerable.ToArray();
            var counter = new Dictionary<TV, int>();
            for (var i = index - radius; i <= index + radius; i++)
            {
                if (i < 0 || i >= list.Count) continue;
                counter.Increment(extractor(list[i]));
            }

            return counter.MaxBy(p => p.Value).Key;
        }

        /// <inheritdoc cref="MajorityAround{TK,TV}"/>
        public static int MajorityAround(this IEnumerable<int> ienumerable, int index, int radius)
        {
            var list = ienumerable as IList<int> ?? ienumerable.ToArray();
            var counter = new Dictionary<int, int>();
            for (var i = index - radius; i <= index + radius; i++)
            {
                if (i < 0 || i >= list.Count) continue;
                counter.Increment(list[i]);
            }

            return counter.MaxBy(p => p.Value).Key;
        }

        /// <summary>
        /// For every element in the given list, picks the majority element of the surrounding elements. Returns an array of the same size as the input list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ienumerable">The list containing the values.</param>
        /// <param name="radius">The radius of the area around the index to use for the median calculations.</param>
        /// <param name="extractor">And extractor function, extracting the value to calculate the majority of.</param>
        /// <returns></returns>
        public static double[] MajoritySmooth<T>(this IEnumerable<T> ienumerable, Func<T, double> extractor, int radius = 5)
        {
            if (ienumerable == null) return null;
            var list = ienumerable as IList<T> ?? ienumerable.ToArray();
            if (!list.Any()) return Array.Empty<double>();

            return list.Select((p, i) => list.MajorityAround(i, radius, extractor)).ToArray();
        }

        /// <inheritdoc cref="MajoritySmooth{T}"/>>
        public static int[] MajoritySmooth(this IEnumerable<int> ienumerable, int radius = 5)
        {
            if (ienumerable == null) return null;
            var list = ienumerable as IList<int> ?? ienumerable.ToArray();
            if (!list.Any()) return Array.Empty<int>();

            return list.Select((p, i) => list.MajorityAround(i, radius)).ToArray();
        }
    }
}