using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public bool Overlaps(DateRange v)
        {
            return Overlaps(v.Start, v.End);
        }

        public bool Overlaps(DateTime start, DateTime end)
        {
            if (end > Start && end <= End) return true;
            if (start > Start && start < End) return true;
            if (start <= Start && end >= End) return true;
            return false;
        }
    }
}
