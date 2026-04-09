using System;
using net.erlenddahl.Extensions.Utilities;

namespace net.erlenddahl.Extensions.TimeSpanExtensions
{
    public static class Ranges
    {
        public static Range<TimeSpan> To(this TimeSpan start, TimeSpan end)
        {
            return new Range<TimeSpan>(start, end);
        }

    }
}