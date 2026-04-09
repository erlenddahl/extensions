using System;

namespace net.erlenddahl.Extensions.IntExtensions
{
    public static class Rounding
    {
        /// <summary>
        /// Rounds the given number to the nearest bucket value.
        /// Examples:
        ///     12.RoundToNearest(10) => 10
        ///     16.RoundToNearest(10) => 20
        ///     12.RoundToNearest(100) => 0
        /// </summary>
        /// <param name="num"></param>
        /// <param name="bucket"></param>
        /// <param name="rounding"></param>
        /// <returns></returns>
        public static int RoundToNearest(this int num, int bucket, MidpointRounding rounding = MidpointRounding.ToEven)
        {
            var rounded = (int) Math.Round(num / (double) bucket, rounding);
            return rounded * bucket;
        }
    }
}
