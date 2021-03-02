using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extensions.IEnumerableExtensions;

namespace Extensions.DictionaryExtensions
{
    public static class BucketizeExtensions
    {

        public static int[] Bucketize(this Dictionary<int, int> dict, int bucketCount, DoubleMinMax minMax = null)
        {
            return dict.ToDictionary(k => (double) k.Key, v => v.Value).Bucketize(bucketCount, minMax);
        }

        public static int[] Bucketize(this Dictionary<double, int> dict, int bucketCount, DoubleMinMax minMax = null)
        {
            var mm = minMax ?? dict.Keys.MinMax();

            var buckets = new int[bucketCount];
            var bucketSize = (mm.Max - mm.Min) / bucketCount;
            foreach (var key in dict.Keys)
            {
                var ix = (int) ((key - mm.Min) / bucketSize);
                if (ix == bucketCount) ix--;
                buckets[ix] += dict[key];
            }

            return buckets;
        }
    }
}