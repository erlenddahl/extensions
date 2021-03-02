using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.ListExtensions
{
    public static class NeighbourExtensions
    {
        public class MatchingNeighbour<T>
        {
            public T Neighbour { get; set; }
            public int Distance { get; set; }
            public int Index { get; set; }

            public MatchingNeighbour(T neighbour, int distance, int index)
            {
                Neighbour = neighbour;
                Distance = distance;
                Index = index;
            }
        }

        private static double WeightedAverage(double a, double b, double distA, double distB)
        {
            var diff = b - a;
            var dist = distB + distA;

            var slope = diff / dist;

            return a + slope * distA;
        }

        public static double WeightedAverage(this IEnumerable<MatchingNeighbour<double>> list)
        {
            var a = list.First();
            var b = list.Last();
            return WeightedAverage(a.Neighbour, b.Neighbour, a.Distance, b.Distance);
        }

        public static double WeightedAverage(this IEnumerable<MatchingNeighbour<int>> list)
        {
            var a = list.First();
            var b = list.Last();
            return WeightedAverage(a.Neighbour, b.Neighbour, a.Distance, b.Distance);
        }

        public static IEnumerable<MatchingNeighbour<int>> ToNonNullable(this IEnumerable<MatchingNeighbour<int?>> list)
        {
            return list.Select(p => new MatchingNeighbour<int>(p.Neighbour.Value, p.Distance, p.Index));
        }

        public static IEnumerable<MatchingNeighbour<double>> ToNonNullable(this IEnumerable<MatchingNeighbour<double?>> list)
        {
            return list.Select(p => new MatchingNeighbour<double>(p.Neighbour.Value, p.Distance, p.Index));
        }

        public static T Nearest<T>(this IEnumerable<MatchingNeighbour<T>> list)
        {
            var a = list.First();
            var b = list.Last();
            return a.Distance <= b.Distance ? a.Neighbour : b.Neighbour;
        }

        public static IEnumerable<MatchingNeighbour<T>> MatchingNeighbours<T>(this IList<T> list, int index, Func<T, bool> condition)
        {
            if (index < 0 || index >= list.Count) yield break;

            var shouldReturnedAfter = true;
            var shouldReturnedBefore = true;

            for (var i = 1; i < list.Count; i++)
            {
                var afterIndex = index + i;
                var beforeIndex = index - i;

                if (afterIndex >= list.Count || afterIndex < 0) shouldReturnedAfter = false;
                if (beforeIndex >= list.Count || beforeIndex < 0) shouldReturnedBefore = false;

                if (shouldReturnedAfter && condition(list[afterIndex]))
                {
                    shouldReturnedAfter = false;
                    yield return new MatchingNeighbour<T>(list[afterIndex], i, afterIndex);
                }

                if (shouldReturnedBefore && condition(list[beforeIndex]))
                {
                    shouldReturnedBefore = false;
                    yield return new MatchingNeighbour<T>(list[beforeIndex], i, beforeIndex);
                }

                if (!shouldReturnedAfter && !shouldReturnedBefore) break;
            }
        }

        public static IEnumerable<T> GetNeighbours<T>(this IList<T> list, int index, int countBefore, int countAfter)
        {
            if (!list.Any()) yield break;

            for (var i = Math.Max(index - countBefore, 0); i < Math.Min(list.Count, index + countAfter + 1); i++)
                yield return list[i];
        }

        public static IEnumerable<T[]> GetNeighbours<T>(this IList<T> list, int countBefore, int countAfter)
        {
            return list.Select((p, i) => list.GetNeighbours(i, countBefore, countAfter).ToArray());
        }

        public static IEnumerable<TResult> GetNeighbours<TSource, TResult>(this IList<TSource> list, int countBefore, int countAfter, Func<int, IEnumerable<TSource>, TResult> selector)
        {
            return list.Select((p, i) => selector(i, list.GetNeighbours(i, countBefore, countAfter)));
        }
    }
}
