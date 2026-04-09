using System;
using net.erlenddahl.Extensions.DateTimeExtensions;

namespace net.erlenddahl.Extensions.DoubleExtensions
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

        /// <inheritdoc cref="RoundToNearest(double,int,MidpointRounding)"/>
        public static long RoundToNearest(this double num, long bucket, MidpointRounding rounding = MidpointRounding.ToEven)
        {
            var rounded = (long)Math.Round(num / (double)bucket, rounding);
            return rounded * bucket;
        }

        /// <inheritdoc cref="RoundToNearest(double,int,MidpointRounding)"/>
        public static int Round(this double num, int bucket, RoundingDirection direction = RoundingDirection.Nearest, MidpointRounding rounding = MidpointRounding.ToEven)
        {
            if (direction == RoundingDirection.Nearest)
                return num.RoundToNearest(bucket, rounding);

            var number = (int)Math.Round(num, 0, rounding);
            var modulo = number % bucket;

            if (direction == RoundingDirection.TowardsZero) return number - modulo;
            if (direction == RoundingDirection.AwayFromZero)
            {
                if(num<0)
                    return number - (bucket + modulo);
                return number + (bucket - modulo);
            }
            if (direction == RoundingDirection.Down)
            {
                if(num < 0)
                    return number - (bucket + modulo);
                return number - modulo;
            }
            
            //RoundingDirection.Up

            if (num < 0)
                return number - modulo;
            return number + (bucket - Math.Abs(modulo));
        }

        /// <inheritdoc cref="RoundToNearest(double,int,MidpointRounding)"/>
        public static long Round(this double num, long bucket, RoundingDirection direction = RoundingDirection.Nearest, MidpointRounding rounding = MidpointRounding.ToEven)
        {
            if (direction == RoundingDirection.Nearest)
                return num.RoundToNearest(bucket, rounding);

            var number = (int)Math.Round(num, 0, rounding);
            var modulo = number % bucket;

            if (direction == RoundingDirection.TowardsZero) return number - modulo;
            if (direction == RoundingDirection.AwayFromZero)
            {
                if (num < 0)
                    return number - (bucket + modulo);
                return number + (bucket - modulo);
            }
            if (direction == RoundingDirection.Down)
            {
                if (num < 0)
                    return number - (bucket + modulo);
                return number - modulo;
            }

            //RoundingDirection.Up

            if (num < 0)
                return number - modulo;
            return number + (bucket - Math.Abs(modulo));
        }
    }
}
