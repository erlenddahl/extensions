using System;
using System.Collections.Generic;
using System.Linq;

namespace net.erlenddahl.Extensions.IEnumerableExtensions
{
    public static class RandomExtensions
    {
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        public static T Random<T>(this IEnumerable<T> list)
        {
            return list.ElementAt(_random.Next(list.Count()));
        }

        public static T Random<T>(this IEnumerable<T> list, Func<T, bool> selector)
        {
            var l = list.Where(selector).ToList();
            return l.ElementAt(_random.Next(l.Count()));
        }
    }
}
