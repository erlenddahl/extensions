using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.Utilities
{
    public class Range<T> where T : IComparable<T>
    {
        public T Start { get; set; }
        public T End { get; set; }

        public Range(T start, T end)
        {
            Start = start;
            End = end;
        }

        public IEnumerable<T> Enumerate(Func<T, T> incrementer)
        {
            var curr = Start;
            while (curr.CompareTo(End) <= 0)
            {
                yield return curr;
                curr = incrementer(curr);
            }
        }

        public bool Overlaps(Range<T> v)
        {
            return Overlaps(v.Start, v.End);
        }

        public bool Overlaps(T start, T end)
        {
            if (end.CompareTo(Start) > 0 && end.CompareTo(End) <= 0) return true;
            if (start.CompareTo(Start) > 0 && start.CompareTo(End) < 0) return true;
            if (start.CompareTo(Start) <= 0 && end.CompareTo(End) >= 0) return true;
            return false;
        }
    }
}
