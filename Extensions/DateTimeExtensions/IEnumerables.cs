using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions.DateTimeExtensions
{
    public static class IEnumerables
    {
        /// <summary>
        /// Returns only the first date from each week (according to the ISO8601 week number) from this list.
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> UniqueWeeks(this IEnumerable<DateTime> dates)
        {
            return dates.GroupBy(p => new {p.Year, Week = p.GetWeekNumber()}).Select(p => p.First());
        }

        /// <summary>
        /// Returns only the first date from each month from this list.
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> UniqueMonths(this IEnumerable<DateTime> dates)
        {
            return dates.GroupBy(p => new {p.Year, p.Month}).Select(p => p.First());
        }

        /// <summary>
        /// Returns only the first date from each year from this list.
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> UniqueYears(this IEnumerable<DateTime> dates)
        {
            return dates.GroupBy(p => new {p.Year}).Select(p => p.First());
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
    }
}
