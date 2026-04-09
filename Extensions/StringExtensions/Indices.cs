using System;
using System.Collections.Generic;
using System.Linq;

namespace net.erlenddahl.Extensions.StringExtensions
{
    public static class Indices
    {
        /// <summary>
        /// Will return a list of all indices of the given needle.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="needle"></param>
        /// <returns></returns>
        public static IEnumerable<int> IndicesOf(this string text, string needle)
        {
            var i = text.IndexOf(needle, StringComparison.Ordinal);
            while (i >= 0)
            {
                yield return i;
                i = text.IndexOf(needle, i + 1, StringComparison.Ordinal);
            }
        }

        /// <summary>
        /// Will return the last indexOf the given needle BEFORE the given index.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="needle"></param>
        /// <param name="beforeIndex"></param>
        /// <returns></returns>
        public static int IndexOfBefore(this string text, string needle, int beforeIndex)
        {
            var items = text.IndicesOf(needle).TakeWhile(p => p < beforeIndex);
            if (items.Any())
                return items.Last();
            return -1;
        }

        /// <summary>
        /// Will return the last indexOf the given needle AFTER the given index.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="needle"></param>
        /// <param name="afterIndex"></param>
        /// <returns></returns>
        public static int IndexOfAfter(this string text, string needle, int afterIndex)
        {
            var items = text.IndicesOf(needle).SkipWhile(p => p <= afterIndex);
            if (items.Any())
                return items.First();
            return -1;
        }
    }
}