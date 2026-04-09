using System;

namespace net.erlenddahl.Extensions.DoubleExtensions
{
    public static class PercentageDifferenceExtensions
    {

        /// <summary>
        /// Calculates the percentage difference between two numbers using the following formula:
        /// diff = 100 * |a-b| / ( (a+b)/2 )
        /// The resulting difference is a number between 0 and 200, where lower means more similar.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="otherNumber"></param>
        /// <returns></returns>
        public static double PercentageDifference(this double number, double otherNumber)
        {
            return 100d * (Math.Abs(number - otherNumber) / ((number + otherNumber) / 2d));
        }

        /// <summary>
        /// Returns true if the two numbers are similar. Similarity is measured using the <see cref="PercentageDifference"/>
        /// function, and they are considered similar if their calculated difference is less than 5% (or other values if given
        /// another similarityThreshold).
        /// </summary>
        /// <param name="number"></param>
        /// <param name="otherNumber"></param>
        /// <param name="similarityThreshold"></param>
        /// <returns></returns>
        public static bool IsSimilar(this double number, double otherNumber, double similarityThreshold = 5)
        {
            var percDiff = number.PercentageDifference(otherNumber);
            return Math.Abs(percDiff - 1) <= similarityThreshold;
        }
    }
}