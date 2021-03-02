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
    }
}
