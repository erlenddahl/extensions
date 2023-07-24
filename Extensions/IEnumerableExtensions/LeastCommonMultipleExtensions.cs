using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions.IEnumerableExtensions
{
    public static class LeastCommonMultipleExtensions
    {
        public static int ComputeLeastCommonMultiple<T>(this IEnumerable<T> list, Func<T, int> valueGetter)
        {
            if (list == null || !list.Any())
                throw new ArgumentException("The list cannot be null or empty.");

            return InternalComputeLeastCommonMultiple(list.Select(valueGetter));
        }

        public static int ComputeLeastCommonMultiple(this IEnumerable<int> list)
        {
            return InternalComputeLeastCommonMultiple(list);
        }

        // Helper function to compute the least common multiple of an array of integers
        private static int InternalComputeLeastCommonMultiple(IEnumerable<int> numbers)
        {
            var lcm = -1;
            foreach (var n in numbers)
            {
                if (lcm == -1)
                {
                    lcm = n;
                    continue;
                }
                lcm = LeastCommonMultiple(lcm, n);
            }
            return lcm;
        }

        // Helper function to compute the least common multiple of two integers using the greatest common divisor (GCD)
        private static int LeastCommonMultiple(int a, int b)
        {
            return Math.Abs(a * b) / GreatestCommonDivisor(a, b);
        }

        // Helper function to compute the greatest common divisor (GCD) of two integers using Euclid's algorithm
        private static int GreatestCommonDivisor(int a, int b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}
