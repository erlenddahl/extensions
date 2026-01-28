using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using static Extensions.ListExtensions.NeighbourExtensions;

namespace Extensions.ListExtensions
{
    public static class InterpolationExtensions
    {
        public static int? WeightedAverage(MatchingNeighbour<int?> prev, MatchingNeighbour<int?> next)
        {
            return (int?)WeightedAverage(prev.Neighbour.Value, next.Neighbour.Value, prev.Distance, next.Distance);
        }

        public static double? WeightedAverage(MatchingNeighbour<double?> prev, MatchingNeighbour<double?> next)
        {
            return (double?)WeightedAverage(prev.Neighbour.Value, next.Neighbour.Value, prev.Distance, next.Distance);
        }

        public static int WeightedAverage(MatchingNeighbour<int> prev, MatchingNeighbour<int> next)
        {
            return (int)WeightedAverage(prev.Neighbour, next.Neighbour, prev.Distance, next.Distance);
        }

        public static double WeightedAverage(MatchingNeighbour<double> prev, MatchingNeighbour<double> next)
        {
            return WeightedAverage(prev.Neighbour, next.Neighbour, prev.Distance, next.Distance);
        }

        public static double WeightedAverage(double a, double b, double distA, double distB)
        {
            var diff = b - a;
            var dist = distB + distA;

            var slope = diff / dist;

            return a + slope * distA;
        }

        private static T Nearest<T>(MatchingNeighbour<T> prev, MatchingNeighbour<T> next)
        {
            return prev.Distance <= next.Distance ? prev.Neighbour : next.Neighbour;
        }

        public static IEnumerable<int?> Interpolate(this IEnumerable<int?> list)
        {
            return Interpolate(list, WeightedAverage, p => p == null);
        }

        public static IEnumerable<double?> Interpolate(this IEnumerable<double?> list)
        {
            return Interpolate(list, WeightedAverage, p => p == null);
        }

        public static IEnumerable<double?> Interpolate(this IEnumerable<double?> list, Func<double?, bool> needsToBeInterpolated)
        {
            return Interpolate(list, WeightedAverage, needsToBeInterpolated);
        }

        public static IEnumerable<double> Interpolate(this IEnumerable<double> list)
        {
            return Interpolate(list, WeightedAverage, p => p == null);
        }

        public static IEnumerable<string> Interpolate(this IEnumerable<string> list)
        {
            return Interpolate(list, Nearest, p => p == null);
        }

        public static IEnumerable<T> Interpolate<T>(this IEnumerable<T> list, Func<MatchingNeighbour<T>, MatchingNeighbour<T>, T> interpolation, Func<T, bool> needsToBeInterpolated = null)
        {
            if (needsToBeInterpolated == null) needsToBeInterpolated = p => p == null;

            var prevValue = default(T);
            var prevValueIx = -1;
            
            var i = -1;
            foreach(var item in list)
            {
                ++i;

                // If it does not need to be interpolated, we store this value
                // as the "previous value", then return it directly.
                if (!needsToBeInterpolated(item))
                {
                    // If the last valid item is older than the previous, we need to interpolate in between.
                    if (prevValueIx < i - 1)
                    {
                        for (var j = prevValueIx + 1; j < i; j++)
                        {
                            yield return prevValueIx == -1 ? item : interpolation(new MatchingNeighbour<T>(prevValue, j - prevValueIx, j), new MatchingNeighbour<T>(item, i - j, i));
                        }
                    }

                    prevValue = item;
                    prevValueIx = i;
                    yield return item;
                }
            }

            if (prevValueIx < i)
            {
                for (var j = prevValueIx + 1; j < i + 1; j++)
                {
                    yield return prevValue;
                }
            }
        }

        public static int Interpolate<T>(this IEnumerable<T> list, Func<T, double?> getter, Action<T, double?> setter)
        {
            return list.Interpolate(p => p == null, getter, setter);
        }

        public static int Interpolate<T>(this IEnumerable<T> list, Func<double?, bool> needsToBeInterpolated, Func<T, double?> getter, Action<T, double?> setter)
        {
            var interpolated = list
                .Select(getter)
                .Interpolate(WeightedAverage, needsToBeInterpolated);

            using (var enumerator1 = list.GetEnumerator())
            using (var enumerator2 = interpolated.GetEnumerator())
            {
                while (enumerator1.MoveNext() && enumerator2.MoveNext())
                {
                    setter(enumerator1.Current, enumerator2.Current);
                }
            }

            return 0;
        }
    }
}
