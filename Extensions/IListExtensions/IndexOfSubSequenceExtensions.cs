using System.Collections.Generic;

namespace net.erlenddahl.Extensions.IListExtensions
{
    public static class IndexOfSubSequenceExtensions
    {
        public static int IndexOf<T>(this IList<T> haystack, IList<T> needle, int startAt = 0)
        {
            var len = needle.Count;
            var limit = haystack.Count - len;
            for (var i = startAt; i <= limit; i++)
            {
                var k = 0;
                for (; k < len; k++)
                {
                    if (!needle[k].Equals(haystack[i + k])) break;
                }
                if (k == len) return i;
            }
            return -1;
        }
    }
}
