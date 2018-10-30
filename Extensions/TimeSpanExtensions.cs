using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class TimeSpanExtensions
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

        public static Range<TimeSpan> To(this TimeSpan start, TimeSpan end)
        {
            return new Range<TimeSpan>(start, end);
        }

        public static string ToPrettyFormat(this TimeSpan span)
        {
            if (span.TotalSeconds < 60) return span.TotalSeconds.ToString("n0") + " seconds";

            var sb = new StringBuilder();
            if (span.Days > 0)
                sb.AppendFormat("{0} day{1} ", span.Days, span.Days > 1 ? "s" : string.Empty);
            if (span.Hours > 0)
                sb.AppendFormat("{0} hour{1} ", span.Hours, span.Hours > 1 ? "s" : string.Empty);
            if (span.Minutes > 0)
                sb.AppendFormat("{0} minute{1} ", span.Minutes, span.Minutes > 1 ? "s" : string.Empty);
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static string ToShortPrettyFormat(this TimeSpan span)
        {
            if (span.TotalSeconds < 60) return span.TotalSeconds.ToString("n0");

            var sb = new StringBuilder();
            if (span.Days > 0)
                sb.AppendFormat("{0}d ", span.Days);
            if (span.Days > 0 || span.Hours > 0)
                sb.AppendFormat("{0:00}:", span.Hours);
            sb.AppendFormat("{0:00}:{1:00}", span.Minutes, span.Seconds);

            return sb.ToString();
        }
    }
}
