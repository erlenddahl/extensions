using System;
using System.Collections.Generic;
using System.Linq;
using static Extensions.ListExtensions.NeighbourExtensions;

namespace Extensions.ListExtensions
{
    public static class InterpolationExtensions
    {
        public static IEnumerable<int?> Interpolate(this IEnumerable<int?> list)
        {
            return ListExtensions.InterpolationExtensions.Interpolate(list.ToList(), p => (int)p.ToNonNullable().WeightedAverage(), p => p == null);
        }
        public static IEnumerable<double?> Interpolate(this IEnumerable<double?> list)
        {
            return ListExtensions.InterpolationExtensions.Interpolate(list.ToList(), p => p.ToNonNullable().WeightedAverage(), p => p == null);
        }

        public static IEnumerable<string> Interpolate(this IEnumerable<string> list)
        {
            return ListExtensions.InterpolationExtensions.Interpolate(list.ToList(), p => p.Nearest(), p => p == null);
        }

        public static IEnumerable<T> Interpolate<T>(this IEnumerable<T> list, Func<MatchingNeighbour<T>[], T> interpolation, Func<T, bool> needsToBeInterpolated = null)
        {
            return ListExtensions.InterpolationExtensions.Interpolate(list.ToList(), interpolation, needsToBeInterpolated);
        }

        public static IEnumerable<T> Interpolate<T>(this IList<T> list, Func<MatchingNeighbour<T>[], T> interpolation, Func<T, bool> needsToBeInterpolated = null)
        {
            if (needsToBeInterpolated == null) needsToBeInterpolated = p => p == null;

            for(var i = 0; i < list.Count; i++)
            {
                if (needsToBeInterpolated(list[i]))
                {
                    var neighbours = list.MatchingNeighbours(i, p => !needsToBeInterpolated(p)).ToArray();
                    yield return interpolation(neighbours);
                }
                else
                {
                    yield return list[i];
                }
            }
        }
    }
}
