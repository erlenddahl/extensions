using System;
using System.Collections.Generic;

namespace net.erlenddahl.Extensions.ListExtensions
{
    public static class WeightedAverageExtensions
    {
        public static double WeightedAverage<T>(this IEnumerable<T> list, Func<T, double> value, Func<T, double> weight)
        {
            var sumValue = 0d;
            var sumWeight = 0d;
            foreach (var item in list)
            {
                var w = weight(item);
                sumValue += value(item) * w;
                sumWeight += w;
            }

            return sumValue / sumWeight;
        }
    }
}
