using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Returns only the first date from each week (according to the ISO8601 week number) from this list.
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> UniqueWeeks(this IEnumerable<DateTime> dates)
        {
            return dates.GroupBy(p => new { p.Year, Week = p.GetWeekNumber() }).Select(p => p.First());
        }

        /// <summary>
        /// Returns only the first date from each month from this list.
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> UniqueMonths(this IEnumerable<DateTime> dates)
        {
            return dates.GroupBy(p => new { p.Year, p.Month }).Select(p => p.First());
        }

        /// <summary>
        /// Returns only the first date from each year from this list.
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> UniqueYears(this IEnumerable<DateTime> dates)
        {
            return dates.GroupBy(p => new { p.Year }).Select(p => p.First());
        }

        /// <summary>
        /// Returns a range of dates between (and including) this date and the given date.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> DatesTo(this DateTime start, DateTime end)
        {
            var curr = start;
            while (curr <= end)
            {
                yield return curr.Date;
                curr = curr.AddDays(1);
            }
        }

        /// <summary>
        /// Returns the ISO8601 week number for the given date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetWeekNumber(this DateTime date)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static Range<DateTime> To(this DateTime start, DateTime end)
        {
            return new Range<DateTime>(start, end);
        }

        public static DateTime DateTimeFromUnix(double unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTimeStamp);
        }

        public static DateTime MakeUtc(this DateTime dt)
        {
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
        }

        /// <summary>
        /// Will round the given datetime to the nearest product of the interval. For example, giving an
        /// interval of 15 minutes will round to the nearest 15 minutes.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="interval"></param>
        /// <param name="rounding"></param>
        /// <returns></returns>
        public static DateTime Round(this DateTime dt, TimeSpan interval, Rounding rounding = Rounding.Nearest)
        {
            var t = dt.Ticks;
            var i = interval.Ticks;

            var diff = t % i;

            if (diff == 0) return dt;

            if (rounding == Rounding.Nearest)
            {
                if (diff < i/2)
                    return new DateTime(t - diff);
                return new DateTime(t + (i - diff));
            }
            
            if (rounding == Rounding.Up)
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
        public static DateTime RoundMinutes(this DateTime dt, int interval, Rounding rounding = Rounding.Nearest)
        {
            return dt.Round(new TimeSpan(0, 0, interval, 0), rounding);
        }

        /// <summary>
        /// Returns the datetime of the first day in the current week.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime dt)
        {
            var start = new DateTime(dt.Ticks);
            while (start.GetWeekNumber() == dt.GetWeekNumber())
                start = start.AddDays(-1);
            start = start.AddDays(1);

            return start.Date;
        }

        /// <summary>
        /// Returns the datetime of the first day in the current month.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime StartOfMonth(this DateTime dt)
        {
            var start = new DateTime(dt.Ticks);
            return start.AddDays(-(start.Day - 1)).Date;
        }

        /// <summary>
        /// Returns the datetime of the first day in the current week.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EndOfWeek(this DateTime dt)
        {
            var start = new DateTime(dt.Ticks);
            while (start.GetWeekNumber() == dt.GetWeekNumber())
                start = start.AddDays(1);
            start = start.AddDays(-1);

            return start.Date;
        }

        /// <summary>
        /// Returns the datetime of the first day in the current month.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EndOfMonth(this DateTime dt)
        {
            var start = new DateTime(dt.Ticks);
            return start.AddMonths(1).StartOfMonth().AddDays(-1).Date;
        }

        public static List<DateTime> ToList(this Range<DateTime> range)
        {
            return range.Enumerate(p => p.AddDays(1)).ToList();
        }
    }
}
