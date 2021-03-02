using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.Tests.IEnumerableExtensions
{
    public static class PercentageDifferenceExtensions
    {

        /// <summary>
        /// Calculates the percentage difference between two numbers using the following formula:
        /// diff = 100 * |a-b| / ( (a+b)/2 )
        /// </summary>
        /// <param name="number"></param>
        /// <param name="otherNumber"></param>
        /// <returns></returns>
        public static double PercentageDifference(this double number, double otherNumber)
        {
            return 100d * (Math.Abs(number - otherNumber) / ((number + otherNumber) / 2d));
        }
    }
}