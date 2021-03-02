using System;
using System.Collections.Generic;

namespace Extensions.IntExtensions
{
    public static class Range
    {

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
        /// Returns an enumerable of the integers from start to end, with the given increment.
        /// </summary>
        /// <param name="start">Will always be returned as the first number in the enumerable.</param>
        /// <param name="end">Will be returned as the last number in the enumerable if it is start + increment * n.</param>
        /// <param name="increment">How much the number should be incremented for each step.</param>
        /// <returns></returns>
        public static IEnumerable<uint> To(this uint start, uint end, uint increment = 1)
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
        /// Returns an enumerable of the integers from start to end, with the given increment.
        /// </summary>
        /// <param name="start">Will always be returned as the first number in the enumerable.</param>
        /// <param name="end">Will be returned as the last number in the enumerable if it is start + increment * n.</param>
        /// <param name="increment">How much the number should be incremented for each step.</param>
        /// <returns></returns>
        public static IEnumerable<uint> To(this int start, uint end, uint increment = 1)
        {
            return ((uint) start).To(end, increment);
        }
    }
}