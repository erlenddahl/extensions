using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Lists
{
    public static class SegmentExtensions
    {
        public static IEnumerable<TResult> Segment<TSource, TResult>(this IEnumerable<TSource> source, int segmentSize, Func<List<TSource>, TResult> aggregator)
        {
            var arr = new List<TSource>();
            foreach (var item in source)
            {
                arr.Add(item);
                if (arr.Count >= segmentSize)
                {
                    yield return aggregator(arr);
                    arr.Clear();
                }
            }
        }
    }
}
