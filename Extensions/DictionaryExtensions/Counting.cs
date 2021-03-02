using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Extensions.Utilities;

namespace Extensions.DictionaryExtensions
{
    public static class Counting
    {
        /// <summary>
        /// Increments the value of the given key, or sets it to 0 if it didn't exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="increment"></param>
        public static void Increment<T>(this Dictionary<T, double> dictionary, T key, double increment = 1)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, increment);
            else
                dictionary[key] += increment;
        }

        /// <summary>
        /// Increments the value of the given key, or sets it to 0 if it didn't exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="increment"></param>
        public static void Increment<T>(this Dictionary<T, Wrapper<double>> dictionary, T key, double increment = 1)
        {
            if (dictionary.TryGetValue(key, out var dw)) dw.Value += increment;
            else dictionary.Add(key, new Wrapper<double>(increment));
        }

        /// <summary>
        /// Increments the value of the given key, or sets it to the default value (increment) if it didn't exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="increment"></param>
        public static void Increment<T>(this Dictionary<T, int> dictionary, T key, int increment = 1)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, increment);
            else
                dictionary[key] += increment;
        }
    }
}
