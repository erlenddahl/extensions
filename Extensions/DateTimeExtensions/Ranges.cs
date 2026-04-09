using System;
using net.erlenddahl.Extensions.Utilities;

namespace net.erlenddahl.Extensions.DateTimeExtensions
{
    public static class Ranges
    {
        public static Range<DateTime> To(this DateTime start, DateTime end)
        {
            return new Range<DateTime>(start, end);
        }
        public static TimeSpan Overlap(this Range<DateTime> a, Range<DateTime> b)
        {
            if (!a.Overlaps(b)) return new TimeSpan(0);

            if (b.Start >= a.Start && b.End <= a.End) return b.End - b.Start;
            if (b.Start <= a.Start && b.End >= a.End) return a.End - a.Start;
            if (b.Start <= a.Start && b.End <= a.End) return b.End - a.Start;
            return a.End - b.Start;
        }
    }
}
