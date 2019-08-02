using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions.IEnumerable;

namespace Extensions.Lists
{
    public static class BucketizeExtensions
    {
        public static int[] Bucketize(this IList<double> list, int bucketCount)
        {
            var mm = list.MinMax();

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

        public static int[] Bucketize(this Dictionary<int, int> dict, int bucketCount)
        {
            return dict.ToDictionary(k => (int) k.Key, v => v.Value).Bucketize(bucketCount);
        }

        public static int[] Bucketize(this Dictionary<double, int> dict, int bucketCount)
        {
            var mm = dict.Keys.MinMax();

            var buckets = new int[bucketCount];
            var bucketSize = (mm.Max - mm.Min) / bucketCount;
            foreach (var key in dict.Keys)
            {
                var ix = (int)((key - mm.Min) / bucketSize);
                if (ix == bucketCount) ix--;
                buckets[ix] += dict[key];
            }

            return buckets;
        }
    }
}
