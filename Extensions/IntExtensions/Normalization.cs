using Extensions.DoubleExtensions;

namespace Extensions.IntExtensions
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
        public static double Normalize(this int num, double min, double max, double minBoundary = 0, double maxBoundary = 1)
        {
            return ((double)num).Normalize(min, max, minBoundary, maxBoundary);
        }
    }
}