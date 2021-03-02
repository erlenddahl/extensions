using System;
using System.Collections.Generic;
using System.Text;
using Extensions.Utilities;

namespace Extensions.TimeSpanExtensions
{
    public static class Ranges
    {
        public static Range<TimeSpan> To(this TimeSpan start, TimeSpan end)
        {
            return new Range<TimeSpan>(start, end);
        }

    }
}