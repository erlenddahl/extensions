using System;
using System.Globalization;

namespace net.erlenddahl.Extensions.DateTimeExtensions
{
    public static class WeekNumber
    {
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
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}