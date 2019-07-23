using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class IEnumerableExtensions
    {
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        public static double Distance<T>(this IEnumerable<T> list, Func<T, double[]> selector)
        {
            GeoCoordinate last = null;
            var dist = 0d;
            foreach (var item in list)
            {
                var currPos = selector(item);
                var curr = new GeoCoordinate(currPos[0], currPos[1]);
                if (last != null)
                {
                    dist += last.GetDistanceTo(curr);
                }
                last = curr;
            }
            return dist;
        }
        
        public static T Random<T>(this IEnumerable<T> list)
        {
            return list.ElementAt(_random.Next(list.Count()));
        }

        public static T Random<T>(this IEnumerable<T> list, Func<T, bool> selector)
        {
            var l = list.Where(selector).ToList();
            return l.ElementAt(_random.Next(l.Count()));
        }

        public static IEnumerable<T> SkipTake<T>(this List<T> list, int start, int count)
        {
            for (var i = start; i < Math.Min(start + count, list.Count); i++)
                yield return list[i];
        } 

        public static List<T> Trim<T>(this IEnumerable<T> originalList, int area,  Func<IEnumerable<T>, bool> func)
        {
            var wasInside = false;
            var list = new List<T>(originalList);

            for (var i = 0; i < list.Count; i++) //Start
            {
                if (list.Count - i < area) break;
                if (func(list.SkipTake(i, area)))
                {
                    wasInside = true;
                    list = list.Skip(i).ToList();
                    break;
                }
            }
            if (!wasInside)
                return new List<T>();

            list.Reverse();
            for (var i = 0; i < list.Count; i++) //End
            {
                if (list.Count - i < area) break;
                if (func(list.SkipTake(i, area)))
                {
                    list = list.Skip(i).ToList();
                    break;
                }
            }
            list.Reverse();

            return list;
        }

        public static IEnumerable<T> Every<T>(this IEnumerable<T> list, int step)
        {
            var count = 0;
            foreach (var v in list)
            {
                if (count % step == 0)
                {
                    count = 0;
                    yield return v;
                }
                count++;
            }
        } 

        public static IEnumerable<IEnumerable<T>> GroupAdjacentBy<T>(this IEnumerable<T> source, Func<T, T, bool> predicate)
        {
            using (var e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    var list = new List<T> { e.Current };
                    var pred = e.Current;
                    while (e.MoveNext())
                    {
                        if (predicate(pred, e.Current))
                        {
                            list.Add(e.Current);
                        }
                        else
                        {
                            yield return list;
                            list = new List<T> { e.Current };
                        }
                        pred = e.Current;
                    }
                    yield return list;
                }
            }
        }

        /// <summary>
        /// Return the index of the element that returns true on the target function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> target)
        {
            var index = 0;
            var found = false;
            foreach(var v in source)
                if (!target(v))
                    index++;
                else
                {
                    found = true;
                    break;
                }

            return found ? index : -1;
        }

        /// <summary>
        /// Does exactly the same as the Min() function, but returns a default value if the source collection is null or empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double SafeMin(this IEnumerable<double> source, double defaultValue = double.MinValue)
        {
            if (source == null || !source.Any()) return defaultValue;
            return source.Min();
        }

        /// <summary>
        /// Does exactly the same as the Min() function, but returns a default value if the source collection is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double SafeMin<T>(this IEnumerable<T> source, Func<T, double> target, double defaultValue = double.MinValue)
        {
            if (source == null || !source.Any()) return defaultValue;
            return source.Min(target);
        }

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

        /// <summary>
        /// Returns the element at the given index, or the default value if the given index does not exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetOrDefault<T>(this IEnumerable<T> source, int index, T defaultValue = default(T))
        {
            if (index < 0) return defaultValue;
            var arr = source as T[] ?? source.ToArray();
            if (arr.Length > index) return arr[index];
            return defaultValue;
        }

        public static bool NotUnique<T>(this IEnumerable<T> source, Func<T, IComparable> target)
        {
            return source.Select(target).Distinct().Count() > 1;
        }

        /// <summary>
        /// Compresses an int list to a list of groups. For example, [1,2,3,4] will be compressed to "1 to 4", expressed as a list of Range objects.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<Range<int>> CompressToConsecutiveGroups(this IEnumerable<int> source)
        {
            var list = source.OrderBy(p => p).ToList();
            if(!list.Any()) yield break;
            Range<int> r = null;
            foreach (var item in list)
            {
                if (r == null) r = new Range<int>(item, item);

                if (item > r.End + 1)
                {
                    yield return r;
                    r = new Range<int>(item, item);
                }
                else
                    r.End = item;
            }

            yield return r;
        }

        /// <summary>
        /// Returns the sum of the given timespans.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TimeSpan DurationSum<T>(this IEnumerable<T> source, Func<T, TimeSpan> target)
        {
            return new TimeSpan(source.Sum(p => target(p).Ticks));
        }

        /// <summary>
        /// Returns a new Queue based on the source list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        {
            return new Queue<T>(source);
        }
    }
}
