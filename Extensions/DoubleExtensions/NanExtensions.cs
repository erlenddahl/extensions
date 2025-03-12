using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.DoubleExtensions
{
    public static class NanExtensions
    {
        /// <summary>
        /// Returns 0 if the given value is NaN, or the given value itself otherwise.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double NanZero(this double value)
        {
            if (double.IsNaN(value)) return 0;
            return value;
        }
    }
}
