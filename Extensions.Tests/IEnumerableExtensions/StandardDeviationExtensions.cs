using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions.Tests.IEnumerableExtensions
{
    public enum StandardDeviationType
    {
        Sample,
        Population
    }

    public static class StandardDeviationExtensions
    {
        /// <summary>
        /// Returns the standard deviation of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the standard deviation from</param>
        /// <param name="type">The type of standard deviation to calculate</param>
        /// <returns></returns>
        public static double StandardDeviation(this IEnumerable<double> numbers, StandardDeviationType type = StandardDeviationType.Sample)
        {
            var count = numbers.Count();
            var n = type == StandardDeviationType.Population ? count : count - 1;
            if (n < 1) return double.NaN;
            var average = numbers.Average();
            return Math.Sqrt(numbers.Sum(p => Math.Pow(p - average, 2)) / n);
        }

        /// <summary>
        /// Returns the standard deviation of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the standard deviation from</param>
        /// <param name="type">The type of standard deviation to calculate</param>
        /// <returns></returns>
        public static double StandardDeviation(this IEnumerable<decimal> numbers, StandardDeviationType type = StandardDeviationType.Sample)
        {
            return numbers.Select(p => (double) p).StandardDeviation(type);
        }

        /// <summary>
        /// Returns the standard deviation of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the standard deviation from</param>
        /// <param name="type">The type of standard deviation to calculate</param>
        /// <returns></returns>
        public static double StandardDeviation(this IEnumerable<float> numbers, StandardDeviationType type = StandardDeviationType.Sample)
        {
            return numbers.Select(p => (double) p).StandardDeviation(type);
        }

        /// <summary>
        /// Returns the standard deviation of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the standard deviation from</param>
        /// <param name="type">The type of standard deviation to calculate</param>
        /// <returns></returns>
        public static double StandardDeviation(this IEnumerable<int> numbers, StandardDeviationType type = StandardDeviationType.Sample)
        {
            return numbers.Select(p => (double) p).StandardDeviation(type);
        }

        /// <summary>
        /// Returns the standard deviation of a property of the given collection of objects.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the standard deviation from</param>
        /// <param name="numberExtractorFunc">The function to extract the number from the object</param>
        /// <param name="type">The type of standard deviation to calculate</param>
        /// <returns></returns>
        public static double StandardDeviation<T>(this IEnumerable<T> numbers, Func<T, double> numberExtractorFunc, StandardDeviationType type = StandardDeviationType.Sample)
        {
            return numbers.Select(numberExtractorFunc).StandardDeviation(type);
        }

        /// <summary>
        /// Returns the standard deviation of a property of the given collection of objects.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the standard deviation from</param>
        /// <param name="numberExtractorFunc">The function to extract the number from the object</param>
        /// <param name="type">The type of standard deviation to calculate</param>
        /// <returns></returns>
        public static double StandardDeviation<T>(this IEnumerable<T> numbers, Func<T, decimal> numberExtractorFunc, StandardDeviationType type = StandardDeviationType.Sample)
        {
            return numbers.Select(p => (double) numberExtractorFunc(p)).StandardDeviation(type);
        }

        /// <summary>
        /// Returns the standard deviation of a property of the given collection of objects.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the standard deviation from</param>
        /// <param name="numberExtractorFunc">The function to extract the number from the object</param>
        /// <param name="type">The type of standard deviation to calculate</param>
        /// <returns></returns>
        public static double StandardDeviation<T>(this IEnumerable<T> numbers, Func<T, float> numberExtractorFunc, StandardDeviationType type = StandardDeviationType.Sample)
        {
            return numbers.Select(p => (double) numberExtractorFunc(p)).StandardDeviation(type);
        }

        /// <summary>
        /// Returns the standard deviation of a property of the given collection of objects.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the standard deviation from</param>
        /// <param name="numberExtractorFunc">The function to extract the number from the object</param>
        /// <param name="type">The type of standard deviation to calculate</param>
        /// <returns></returns>
        public static double StandardDeviation<T>(this IEnumerable<T> numbers, Func<T, int> numberExtractorFunc, StandardDeviationType type = StandardDeviationType.Sample)
        {
            return numbers.Select(p => (double) numberExtractorFunc(p)).StandardDeviation(type);
        }
    }
}