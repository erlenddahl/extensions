namespace net.erlenddahl.Extensions.DoubleExtensions
{
    public static class Restriction
    {
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
    }
}