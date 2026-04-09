using System;

namespace net.erlenddahl.Extensions.DateTimeExtensions
{
    public enum RoundingDirection
    {
        Nearest,
        Up,
        Down,
        TowardsZero,
        AwayFromZero
    }

    public static class Rounding
    {
        /// <summary>
        /// Will round the given datetime to the nearest product of the interval. For example, giving an
        /// interval of 15 minutes will round to the nearest 15 minutes.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="interval"></param>
        /// <param name="rounding"></param>
        /// <returns></returns>
        public static DateTime Round(this DateTime dt, TimeSpan interval, RoundingDirection rounding = RoundingDirection.Nearest)
        {
            var t = dt.Ticks;
            var i = interval.Ticks;

            var diff = t % i;

            if (diff == 0) return dt;

            if (rounding == RoundingDirection.Nearest)
            {
                if (diff < i / 2)
                    return new DateTime(t - diff);
                return new DateTime(t + (i - diff));
            }

            if (rounding == RoundingDirection.Up)
                return new DateTime(t + (i - diff));

            return new DateTime(t - diff);
        }

        /// <summary>
        /// Will round the given timespan to the nearest product of the interval. For example, giving an
        /// interval of 15 minutes will round to the nearest 15 minutes.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="interval"></param>
        /// <param name="rounding"></param>
        /// <returns></returns>
        public static DateTime RoundMinutes(this DateTime dt, int interval, RoundingDirection rounding = RoundingDirection.Nearest)
        {
            return dt.Round(new TimeSpan(0, 0, interval, 0), rounding);
        }
    }
}