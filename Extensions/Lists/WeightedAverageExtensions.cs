using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Lists
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
