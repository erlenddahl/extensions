using System.Collections.Generic;

namespace Extensions.IListExtensions
{
    public static class IsValidIndexExtensions
    {
        public static bool IsValidIndex<T>(this IList<T> list, int index)
        {
            if (index < 0) return false;
            if (index > list.Count - 1) return false;
            return true;
        }
    }
}
