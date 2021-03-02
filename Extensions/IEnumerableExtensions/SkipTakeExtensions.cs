using System;
using System.Collections.Generic;

namespace Extensions.IEnumerableExtensions
{
    public static class SkipTakeExtensions { 

        public static IEnumerable<T> SkipTake<T>(this List<T> list, int start, int count)
        {
            for (var i = start; i < Math.Min(start + count, list.Count); i++)
                yield return list[i];
        }
    }
}