using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class RangeExtensions
    {
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
