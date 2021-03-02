using System;
using System.Collections.Generic;

namespace Extensions.IEnumerableExtensions
{
    public static class IndexOfExtensions
    {
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
    }
}