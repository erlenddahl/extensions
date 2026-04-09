using System;
using System.Collections.Generic;

namespace net.erlenddahl.Extensions.IEnumerableExtensions
{
    public class IntegerMinMax
    {
        public int Min;
        public int Max;

        public IntegerMinMax(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }

    public class DoubleMinMax
    {
        public double Min;
        public double Max;

        public static DoubleMinMax Empty => new DoubleMinMax(double.MaxValue, double.MinValue);

        public double Size;

        public DoubleMinMax(double min, double max)
        {
            Min = min;
            Max = max;
            Size = max - min;
        }

        public void Extend(double min, double max)
        {
            if (min < Min) Min = min;
            if (max > Max) Max = max;
            Size = max - min;
        }

        public bool Contains(double value)
        {
            return value >= Min && value <= Max;
        }

        /// <summary>
        /// Scales the given value from this MinMax boundary to the given boundary.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public double ScaleTo(double value, double min, double max)
        {
            return ((value - Min) / (Max - Min)) * (max - min) + min;
        }

        public override string ToString()
        {
            return $"[{Min:n3}, {Max:n3}]";
        }
    }

    public static class MinMaxExtensions
    {
        public static DoubleMinMax MinMax<T>(this IEnumerable<T> enumerable, Func<T, double> selector)
        {
            var min = double.MaxValue;
            var max = double.MinValue;
            foreach (var item in enumerable)
            {
                var value = selector(item);
                if (value < min) min = value;
                if (value > max) max = value;
            }

            return new DoubleMinMax(min, max);
        }

        public static DoubleMinMax MinMax(this IEnumerable<double> enumerable)
        {
            var min = double.MaxValue;
            var max = double.MinValue;
            foreach (var value in enumerable)
            {
                if (value < min) min = value;
                if (value > max) max = value;
            }

            return new DoubleMinMax(min, max);
        }

        public static IntegerMinMax MinMax<T>(this IEnumerable<T> enumerable, Func<T, int> selector)
        {
            var min = int.MaxValue;
            var max = int.MinValue;
            foreach (var item in enumerable)
            {
                var value = selector(item);
                if (value < min) min = value;
                if (value > max) max = value;
            }

            return new IntegerMinMax(min, max);
        }

        public static IntegerMinMax MinMax(this IEnumerable<int> enumerable)
        {
            var min = int.MaxValue;
            var max = int.MinValue;
            foreach (var value in enumerable)
            {
                if (value < min) min = value;
                if (value > max) max = value;
            }

            return new IntegerMinMax(min, max);
        }
    }
}
