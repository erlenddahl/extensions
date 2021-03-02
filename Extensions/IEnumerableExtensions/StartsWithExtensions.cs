using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.IEnumerableExtensions
{
    public static class StartsWithExtensions
    {
        public static bool StartsWith<T>(this IEnumerable<T> list, IEnumerable<T> needle)
        {
            using (var listEnumerator = list.GetEnumerator())
            using (var needleEnumerator = needle.GetEnumerator())
            {
                while (needleEnumerator.MoveNext())
                {
                    // If the list has no more elements, it's a mismatch.
                    if (!listEnumerator.MoveNext()) return false;

                    // Or if one of the items are null, and the other isn't.
                    if (listEnumerator.Current == null && needleEnumerator.Current != null) return false;
                    if (needleEnumerator.Current == null && listEnumerator.Current != null) return false;

                    // Or if the current elements are not equal.
                    if (listEnumerator.Current != null && !listEnumerator.Current.Equals(needleEnumerator.Current)) return false;
                }
            }

            // Otherwise, it started with the given needle.
            return true;
        }

        public static bool StartsWith<T>(this IEnumerable<T> list, params T[] needle)
        {
            return list.StartsWith((IEnumerable<T>)needle);
        }
    }
}
