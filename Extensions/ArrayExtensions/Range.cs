using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.ArrayExtensions
{
    public static class Range
    {
        public static T[] GetRange<T>(this T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static T[] GetRangeByIndex<T>(this T[] array, int fromIndex, int toIndex)
        {
            if (fromIndex == toIndex) return Array.Empty<T>();
            var length = toIndex - fromIndex + 1;
            return array.GetRange(fromIndex, length);
        }
    }
}
