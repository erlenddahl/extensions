using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class NumericExtensions
    {
        /// <summary>
        /// Will reverse the given number (return -N if it is positive, or +N if it is negative).
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static double Reverse(this double num)
        {
            return num * -1;
        }

        /// <summary>
        /// Will reverse the given number (return -N if it is positive, or +N if it is negative) IF the given expression is true.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static double ReverseIf(this double num, bool expression)
        {
            if (expression)
                return num * -1;
            return num;
        }
        /// <summary>
        /// Will reverse the given number (return -N if it is positive, or +N if it is negative).
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int Reverse(this int num)
        {
            return num * -1;
        }

        /// <summary>
        /// Will reverse the given number (return -N if it is positive, or +N if it is negative) IF the given expression is true.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static int ReverseIf(this int num, bool expression)
        {
            if (expression)
                return num * -1;
            return num;
        }

        /// <summary>
        /// Returns an enumerable of the integers from start to end, with the given increment.
        /// </summary>
        /// <param name="start">Will always be returned as the first number in the enumerable.</param>
        /// <param name="end">Will be returned as the last number in the enumerable if it is start + increment * n.</param>
        /// <param name="increment">How much the number should be incremented for each step.</param>
        /// <returns></returns>
        public static IEnumerable<int> To(this int start, int end, int increment = 1)
        {
            if (increment == 0) throw new Exception("Increment cannot be 0 (that would make an infinite loop)!");
            var increasing = end >= start;
            if (increasing && increment < 0) throw new Exception("Increment cannot be negative when end is larger than start.");
            if (!increasing && increment > 0) throw new Exception("Increment cannot be positive when end is smaller than start.");
            while (true)
            {
                yield return start;
                if ((increasing && start >= end) || (!increasing && start <= end)) break;
                start += increment;
            }
        }

        /// <summary>
        /// Will return the given number, unless it is outside the given boundaries.
        /// If the number is lower than the min value, this function returns the min value.
        /// If the number if higher than the max value, this function returns the max value.
        /// </summary>
        /// <param name="num">The number whose value to restrict.</param>
        /// <param name="min">The minimum boundary.</param>
        /// <param name="max">The maximum boundary.</param>
        /// <returns></returns>
        public static double Restrict(this double num, double min, double max)
        {
            if (num < min) return min;
            if (num > max) return max;
            return num;
        }

        /// <summary>
        /// Will return the given number, unless it is outside the given boundaries.
        /// If the number is lower than the min value, this function returns the min value.
        /// If the number if higher than the max value, this function returns the max value.
        /// </summary>
        /// <param name="num">The number whose value to restrict.</param>
        /// <param name="min">The minimum boundary.</param>
        /// <param name="max">The maximum boundary.</param>
        /// <returns></returns>
        public static int Restrict(this int num, int min, int max)
        {
            if (num < min) return min;
            if (num > max) return max;
            return num;
        }

        /// <summary>
        /// Will normalize the given number within the given boundaries.
        /// </summary>
        /// <param name="num">The number whose value to normalize.</param>
        /// <param name="min">The minimum value this number can have.</param>
        /// <param name="max">The maximum value this number can have.</param>
        /// <param name="minBoundary">The minimum boundary.</param>
        /// <param name="maxBoundary">The maximum boundary.</param>
        /// <returns></returns>
        public static double Normalize(this double num, double min, double max, double minBoundary = 0, double maxBoundary = 1)
        {
            if (num < min)
                throw new ArgumentOutOfRangeException(string.Format("The given number ({0:n2}) is outside of the normalization boundaries ({1:n2} - {2:n2}).", num, min, max));
            if (num > max)
                throw new ArgumentOutOfRangeException(string.Format("The given number ({0:n2}) is outside of the normalization boundaries ({1:n2} - {2:n2}).", num, min, max));
            if (min==max) return minBoundary;
            return (num - min) / (max - min) * (maxBoundary - minBoundary) + minBoundary;
        }

        /// <summary>
        /// Will normalize the given number within the given boundaries.
        /// </summary>
        /// <param name="num">The number whose value to normalize.</param>
        /// <param name="min">The minimum value this number can have.</param>
        /// <param name="max">The maximum value this number can have.</param>
        /// <param name="minBoundary">The minimum boundary.</param>
        /// <param name="maxBoundary">The maximum boundary.</param>
        /// <returns></returns>
        public static double Normalize(this int num, double min, double max, double minBoundary = 0, double maxBoundary = 1)
        {
            return ((double) num).Normalize(min, max, minBoundary, maxBoundary);
        }


        /// <summary>
        /// Returns the "Excel column" equivalent of the given number. 0 is A, 1 is B, etc. After Z, it will start with double characters, AA, AB, ..., BA, BB, etc.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ToExcelColumn(this int num)
        {
            if(num < 0) throw new InvalidDataException("Cannot generate Excel column for numbers less than zero (given: " + num + ").");

            //Source: https://stackoverflow.com/questions/181596/how-to-convert-a-column-number-eg-127-into-an-excel-column-eg-aa

            //Increment with one to make A = 0 instead of A = 1.
            num++;
            var columnName = string.Empty;

            while (num > 0)
            {
                var modulo = (num - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                num = (num - modulo) / 26;
            }

            return columnName;
        }

        /// <summary>
        /// Rounds the given number to the nearest bucket value.
        /// Examples:
        ///     12.RoundToNearest(10) => 10
        ///     16.RoundToNearest(10) => 20
        ///     12.RoundToNearest(100) => 0
        /// </summary>
        /// <param name="num"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public static int RoundToNearest(this int num, int bucket, MidpointRounding rounding = MidpointRounding.ToEven)
        {
            var rounded = (int)Math.Round(num / (double)bucket, rounding);
            return rounded * bucket;
        }

        /// <summary>
        /// Rounds the given number to the nearest bucket value.
        /// Examples:
        ///     12.RoundToNearest(10) => 10
        ///     16.RoundToNearest(10) => 20
        ///     12.RoundToNearest(100) => 0
        /// </summary>
        /// <param name="num"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public static int RoundToNearest(this double num, int bucket, MidpointRounding rounding = MidpointRounding.ToEven)
        {
            var rounded = (int)Math.Round(num / (double)bucket, rounding);
            return rounded * bucket;
        }
    }
}
