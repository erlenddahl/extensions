using System;
using System.Collections.Generic;
using System.Text;
using Extensions.DateTimeExtensions;

namespace Extensions.DoubleExtensions
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
        public static int RoundToNearest(this double num, int bucket, MidpointRounding rounding = MidpointRounding.ToEven)
        {
            var rounded = (int)Math.Round(num / (double)bucket, rounding);
            return rounded * bucket;
        }

        /// <summary>
        /// Rounds the given number to the nearest bucket value.
        /// Examples:
        ///     12.Round(10, RoundingDirection.Up) => 20
        ///     16.Round(10, RoundingDirection.Down) => 10
        ///     12.Round(10, RoundingDirection.Nearest) => 10
        /// </summary>
        /// <param name="num"></param>
        /// <param name="bucket"></param>
        /// <param name="direction"></param>
        /// <param name="rounding"></param>
        /// <returns></returns>
        public static int Round(this double num, int bucket, RoundingDirection direction = RoundingDirection.Nearest, MidpointRounding rounding = MidpointRounding.ToEven)
        {
            if (direction == RoundingDirection.Nearest)
                return num.RoundToNearest(bucket, rounding);

            var number = (int)Math.Round(num, 0, rounding);
            var modulo = number % bucket;

            if (direction == RoundingDirection.Down) return number - modulo;
            return number + (bucket - modulo);
        }
    }
}
