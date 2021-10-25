using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.IEnumerableExtensions
{
    public static class MinByExtensions
    {
        public static T MinBy<T>(this IEnumerable<T> list, Func<T, double> predicate)
        {
            var minValue = double.MaxValue;
            var minItem = default(T);
            foreach (var item in list)
            {
                var value = predicate(item);
                if (value < minValue)
                {
                    minValue = value;
                    minItem = item;
                }
            }

            return minItem;
        }
    }
}
