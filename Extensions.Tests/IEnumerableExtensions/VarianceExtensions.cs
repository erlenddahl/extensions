using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions.Tests.IEnumerableExtensions
{
    public static class VarianceExtensions
    {

        /// <summary>
        /// Returns the variance of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the variance from</param>
        /// <param name="type">The type of variance to calculate</param>
        /// <returns></returns>
        public static double Variance(this IEnumerable<double> numbers, StandardDeviationType type = StandardDeviationType.Sample)
        {
            return Math.Pow(numbers.StandardDeviation(type), 2);
        }

        /// <summary>
        /// Returns the variance of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the variance from</param>
        /// <param name="type">The type of variance to calculate</param>
        /// <returns></returns>
        public static double Variance(this IEnumerable<decimal> numbers, StandardDeviationType type = StandardDeviationType.Sample)
        {
            return numbers.Select(p => (double) p).Variance(type);
        }

        /// <summary>
        /// Returns the variance of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the variance from</param>
        /// <param name="type">The type of variance to calculate</param>
        /// <returns></returns>
        public static double Variance(this IEnumerable<float> numbers, StandardDeviationType type = StandardDeviationType.Sample)
        {
            return numbers.Select(p => (double) p).Variance(type);
        }

        /// <summary>
        /// Returns the variance of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the variance from</param>
        /// <param name="type">The type of variance to calculate</param>
        /// <returns></returns>
        public static double Variance(this IEnumerable<int> numbers, StandardDeviationType type = StandardDeviationType.Sample)
        {
            return numbers.Select(p => (double) p).Variance(type);
        }
    }
}
