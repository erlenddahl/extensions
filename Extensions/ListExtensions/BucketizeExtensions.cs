using System.Collections.Generic;
using System.Linq;
using Extensions.IEnumerableExtensions;

namespace Extensions.ListExtensions
{
    public static class BucketizeExtensions
    {
        public static int[] Bucketize(this IList<double> list, int bucketCount, DoubleMinMax minMax = null)
        {
            var mm = minMax ?? list.MinMax();

            var buckets = new int[bucketCount];
            var bucketSize = (mm.Max - mm.Min) / bucketCount;
            foreach (var item in list)
            {
                var ix = (int)((item - mm.Min) / bucketSize);
                if (ix == bucketCount) ix--;
                buckets[ix]++;
            }

            return buckets;
        }
    }
}
