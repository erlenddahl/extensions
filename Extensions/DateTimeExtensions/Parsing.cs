using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.DateTimeExtensions
{
    public static class Parsing
    {
        public static DateTime DateTimeFromUnix(double unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTimeStamp);
        }

        public static DateTime MakeUtc(this DateTime dt)
        {
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
        }
    }
}
