using System;

namespace net.erlenddahl.Extensions.DateTimeExtensions
{
    public static class EndOf
    {
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
    }
}