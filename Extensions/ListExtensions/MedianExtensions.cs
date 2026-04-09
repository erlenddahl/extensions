using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace net.erlenddahl.Extensions.ListExtensions
{
    public static class MedianExtensions
    {

        /// <summary>
        /// Returns the median of the given list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static double Median<T>(this IEnumerable<T> list, Func<T, double> selector)
        {
            var c = list.Select(selector).OrderBy(p => p).ToArray();
            if (c.Length < 1) throw new InvalidDataException("Empty sequence");
            if (c.Length % 2 == 0)
                return (c[c.Length / 2] + c[c.Length / 2 - 1]) / 2d;
            return c[c.Length / 2];
        }

        /// <summary>
        /// Returns the median of the given list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static decimal Median(this IEnumerable<decimal> list)
        {
            var c = list.OrderBy(p => p).ToArray();
            if (c.Length < 1) throw new InvalidDataException("Empty sequence");
            if (c.Length % 2 == 0)
                return (c[c.Length / 2] + c[c.Length / 2 - 1]) / 2m;
            return c[c.Length / 2];
        }

        /// <summary>
        /// Returns the median of the given list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static double Median(this IEnumerable<double> list)
        {
            return (double)list.Select(p => (decimal)p).Median();
        }

        /// <summary>
        /// Returns the median of the given list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static double Median(this IEnumerable<float> list)
        {
            return (double)list.Select(p => (decimal)p).Median();
        }

        /// <summary>
        /// Returns the median of the given list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static double Median(this IEnumerable<int> list)
        {
            return (double)list.Select(p => (decimal)p).Median();
        }

        /// <summary>
        /// Calculate the median of a given value in a range around the given point.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list containing the values.</param>
        /// <param name="index">The index of the list for which to calculate the median around.</param>
        /// <param name="radius">The radius of the area around the index to use for the median calculations.</param>
        /// <param name="extractor">And extractor function, extracting the value to calculate the median of.</param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double MedianAround<T>(this IList<T> list, int index, int radius, Func<T, double> extractor, double defaultValue = -1)
        {
            var neighbours = new List<double>();
            for (var i = index - radius; i <= index + radius; i++)
            {
                if (i < 0 || i >= list.Count) continue;
                neighbours.Add(extractor(list[i]));
            }

            if (!neighbours.Any()) return defaultValue;

            neighbours = neighbours.OrderBy(p => p).ToList();

            if (neighbours.Count % 2 == 0)
                return (neighbours[neighbours.Count / 2] + neighbours[neighbours.Count / 2 - 1]) / 2;

            return neighbours[(neighbours.Count - 1) / 2];
        }

        /// <summary>
        /// Will smooth the given property on the given list using a median filter with the given radius.
        /// Note: uses reflection, may be slower than doing it manually.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void MedianSmooth<T>(this IList<T> list, string propertyName, int radius = 5)
        {
            if (list == null || !list.Any()) return;
            var type = list.First().GetType();
            var prop = type.GetProperty(propertyName);
            if (prop == null) throw new NullReferenceException("No property named '" + propertyName + "'");

            var smoothedValues = list.Select((p, i) => list.MedianAround(i, radius, c => (double)prop.GetValue(c, null))).ToList();
            for (var i = 0; i < list.Count; i++)
                prop.SetValue(list[i], smoothedValues[i]);
        }

        /// <summary>
        /// Will smooth the given property on the given list using a median filter with the given radius.
        /// Note: uses reflection, may be slower than doing it manually.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void MedianSmooth<T>(this IList<T> list, Func<T, double> getValue, Action<T, double> setValue, int radius = 5)
        {
            if (list == null || !list.Any()) return;

            var smoothedValues = list.Select((p, i) => list.MedianAround(i, radius, getValue)).ToList();
            for (var i = 0; i < list.Count; i++)
                setValue(list[i], smoothedValues[i]);
        }

        /// <summary>
        /// Will smooth the given list using a median filter with the given radius.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<double> MedianSmooth(this IEnumerable<long> list, int radius = 5)
        {
            return list.Select(p => (double)p).ToList().MedianSmooth(radius);
        }

        /// <summary>
        /// Will smooth the given list using a median filter with the given radius.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<double> MedianSmooth(this IEnumerable<decimal> list, int radius = 5)
        {
            return list.Select(p => (double)p).ToList().MedianSmooth(radius);
        }

        /// <summary>
        /// Will smooth the given list using a median filter with the given radius.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<double> MedianSmooth(this IEnumerable<float> list, int radius = 5)
        {
            return list.Select(p => (double)p).ToList().MedianSmooth(radius);
        }

        /// <summary>
        /// Will smooth the given list using a median filter with the given radius.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<double> MedianSmooth(this IEnumerable<int> list, int radius = 5)
        {
            return list.Select(p => (double)p).ToList().MedianSmooth(radius);
        }

        /// <summary>
        /// Will smooth the given list using a median filter with the given radius.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<double> MedianSmooth(this IEnumerable<double> list, int radius = 5)
        {
            return list.ToList().MedianSmooth(radius);
        }

        /// <summary>
        /// Will smooth the given list using a median filter with the given radius.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<double> MedianSmooth(this IList<double> list, int radius = 5)
        {
            if (list == null || !list.Any()) return list;
            return list.Select((p, i) => list.MedianAround(i, radius, c => c));
        }
    }
}
