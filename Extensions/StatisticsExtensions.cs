using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Extensions
{
    public enum StdType
    {
        Sample,
        Population
    }

    public static class StatisticsExtensions
    {
        /// <summary>
        /// Returns the standard deviation of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the standard deviation from</param>
        /// <param name="type">The type of standard deviation to calculate</param>
        /// <returns></returns>
        public static double StandardDeviation(this IEnumerable<double> numbers, StdType type = StdType.Sample)
        {
            var count = numbers.Count();
            var n = type == StdType.Population ? count : count - 1;
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
        public static double StandardDeviation(this IEnumerable<decimal> numbers, StdType type = StdType.Sample)
        {
            return numbers.Select(p => (double)p).StandardDeviation(type);
        }

        /// <summary>
        /// Returns the standard deviation of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the standard deviation from</param>
        /// <param name="type">The type of standard deviation to calculate</param>
        /// <returns></returns>
        public static double StandardDeviation(this IEnumerable<float> numbers, StdType type = StdType.Sample)
        {
            return numbers.Select(p => (double)p).StandardDeviation(type);
        }

        /// <summary>
        /// Returns the standard deviation of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the standard deviation from</param>
        /// <param name="type">The type of standard deviation to calculate</param>
        /// <returns></returns>
        public static double StandardDeviation(this IEnumerable<int> numbers, StdType type = StdType.Sample)
        {
            return numbers.Select(p => (double)p).StandardDeviation(type);
        }

        /// <summary>
        /// Returns the standard deviation of a property of the given collection of objects.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the standard deviation from</param>
        /// <param name="numberExtractorFunc">The function to extract the number from the object</param>
        /// <param name="type">The type of standard deviation to calculate</param>
        /// <returns></returns>
        public static double StandardDeviation<T>(this IEnumerable<T> numbers, Func<T, double> numberExtractorFunc, StdType type = StdType.Sample)
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
        public static double StandardDeviation<T>(this IEnumerable<T> numbers, Func<T, decimal> numberExtractorFunc, StdType type = StdType.Sample)
        {
            return numbers.Select(p => (double)numberExtractorFunc(p)).StandardDeviation(type);
        }

        /// <summary>
        /// Returns the standard deviation of a property of the given collection of objects.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the standard deviation from</param>
        /// <param name="numberExtractorFunc">The function to extract the number from the object</param>
        /// <param name="type">The type of standard deviation to calculate</param>
        /// <returns></returns>
        public static double StandardDeviation<T>(this IEnumerable<T> numbers, Func<T, float> numberExtractorFunc, StdType type = StdType.Sample)
        {
            return numbers.Select(p => (double)numberExtractorFunc(p)).StandardDeviation(type);
        }

        /// <summary>
        /// Returns the standard deviation of a property of the given collection of objects.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the standard deviation from</param>
        /// <param name="numberExtractorFunc">The function to extract the number from the object</param>
        /// <param name="type">The type of standard deviation to calculate</param>
        /// <returns></returns>
        public static double StandardDeviation<T>(this IEnumerable<T> numbers, Func<T, int> numberExtractorFunc, StdType type = StdType.Sample)
        {
            return numbers.Select(p => (double)numberExtractorFunc(p)).StandardDeviation(type);
        }

        /// <summary>
        /// Returns the variance of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the variance from</param>
        /// <param name="type">The type of variance to calculate</param>
        /// <returns></returns>
        public static double Variance(this IEnumerable<double> numbers, StdType type = StdType.Sample)
        {
            return Math.Pow(numbers.StandardDeviation(type), 2);
        }

        /// <summary>
        /// Returns the variance of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the variance from</param>
        /// <param name="type">The type of variance to calculate</param>
        /// <returns></returns>
        public static double Variance(this IEnumerable<decimal> numbers, StdType type = StdType.Sample)
        {
            return numbers.Select(p => (double)p).Variance(type);
        }

        /// <summary>
        /// Returns the variance of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the variance from</param>
        /// <param name="type">The type of variance to calculate</param>
        /// <returns></returns>
        public static double Variance(this IEnumerable<float> numbers, StdType type = StdType.Sample)
        {
            return numbers.Select(p => (double)p).Variance(type);
        }

        /// <summary>
        /// Returns the variance of the given collection of numbers.
        /// </summary>
        /// <param name="numbers">The numbers to calculate the variance from</param>
        /// <param name="type">The type of variance to calculate</param>
        /// <returns></returns>
        public static double Variance(this IEnumerable<int> numbers, StdType type = StdType.Sample)
        {
            return numbers.Select(p => (double)p).Variance(type);
        }
    }
}