using System.Collections.Generic;

namespace Extensions.IEnumerableExtensions
{
    public static class Converting
    {
        /// <summary>
        /// Returns a new Queue based on the source list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        {
            return new Queue<T>(source);
        }
    }
}