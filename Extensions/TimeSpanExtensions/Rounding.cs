using System;

namespace net.erlenddahl.Extensions.TimeSpanExtensions
{
    public static class Rounding
    {
        /// <summary>
        /// Will round the given timespan to the nearest product of the interval. For example, giving an
        /// interval of 15 minutes will round to the nearest 15 minutes.
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static TimeSpan Round(this TimeSpan ts, TimeSpan interval)
        {
            var t = ts.Ticks;
            var i = interval.Ticks;

            var diff = t % i;

            if (diff < i / 2)
                return new TimeSpan(t - diff);
            return new TimeSpan(t + (i - diff));
        }

        /// <summary>
        /// Will round the given timespan to the nearest product of the interval. For example, giving an
        /// interval of 15 minutes will round to the nearest 15 minutes.
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static TimeSpan RoundMinutes(this TimeSpan ts, int interval)
        {
            return ts.Round(new TimeSpan(0, 0, interval, 0));
        }
    }
}
