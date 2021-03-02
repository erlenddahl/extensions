using System.Collections.Generic;
using System.Linq;
using Extensions.Utilities;

namespace Extensions.IEnumerableExtensions
{
    public static class Range
    {
        /// <summary>
        /// Compresses an int list to a list of groups. For example, [1,2,3,4] will be compressed to "1 to 4", expressed as a list of Range objects.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<Range<int>> CompressToConsecutiveGroups(this IEnumerable<int> source)
        {
            var list = source.OrderBy(p => p).ToList();
            if(!list.Any()) yield break;
            Range<int> r = null;
            foreach (var item in list)
            {
                if (r == null) r = new Range<int>(item, item);

                if (item > r.End + 1)
                {
                    yield return r;
                    r = new Range<int>(item, item);
                }
                else
                    r.End = item;
            }

            yield return r;
        }
    }
}