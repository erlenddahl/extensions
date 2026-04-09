using System.Collections.Generic;
using System.Linq;

namespace net.erlenddahl.Extensions.IEnumerableExtensions
{
    public static class GetOrDefaultExtensions
    {
        /// <summary>
        /// Returns the element at the given index, or the default value if the given index does not exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetOrDefault<T>(this IEnumerable<T> source, int index, T defaultValue = default(T))
        {
            if (index < 0) return defaultValue;
            var arr = source as T[] ?? source.ToArray();
            if (arr.Length > index) return arr[index];
            return defaultValue;
        }
    }
}