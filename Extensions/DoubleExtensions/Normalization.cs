using System;

namespace net.erlenddahl.Extensions.DoubleExtensions
{
    public static class Normalization
    {
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
            if (min == max) return minBoundary;
            return (num - min) / (max - min) * (maxBoundary - minBoundary) + minBoundary;
        }
    }
}