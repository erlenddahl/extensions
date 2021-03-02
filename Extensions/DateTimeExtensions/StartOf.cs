using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.DateTimeExtensions
{
    public static class StartOf
    {
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
    }
}
