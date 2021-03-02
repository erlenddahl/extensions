using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.IEnumerableExtensions
{
    public static class NotUniqueExtensions
    {
        public static bool NotUnique<T>(this IEnumerable<T> source, Func<T, IComparable> target)
        {
            return source.Select(target).Distinct().Count() > 1;
        }
    }
}