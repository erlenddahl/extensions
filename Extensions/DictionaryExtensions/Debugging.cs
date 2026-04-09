using System;
using System.Collections.Generic;
using System.Linq;

namespace net.erlenddahl.Extensions.DictionaryExtensions
{
    public static class Debugging
    {

        /// <summary>
        /// Returns all keys and values as a pretty string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        public static string Print<T>(this Dictionary<T, int> dictionary)
        {
            var s = "";
            var longest = dictionary.Keys.Select(p => p.ToString()).Max(p => p.Length);
            foreach (var kvp in dictionary)
                s += (string.IsNullOrWhiteSpace(s) ? "" : Environment.NewLine) + kvp.Key.ToString().PadRight(longest) + ": " + kvp.Value.ToString("n0");
            return s;
        }

        /// <summary>
        /// Returns all keys and values as a pretty string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        public static string Print<T>(this Dictionary<T, double> dictionary)
        {
            var s = "";
            var longest = dictionary.Keys.Select(p => p.ToString()).Max(p => p.Length);
            foreach (var kvp in dictionary)
                s += (string.IsNullOrWhiteSpace(s) ? "" : Environment.NewLine) + kvp.Key.ToString().PadRight(longest) + ": " + kvp.Value.ToString("n3").Replace(",000", "");
            return s;
        }

        /// <summary>
        /// Prints the types of all objects in the result dictionaries.
        /// </summary>
        /// <param name="list"></param>
        public static void PrintTypes(this IEnumerable<Dictionary<string, object>> list)
        {
            var first = list.First();
            first.Keys.ToList().ForEach(p => Console.WriteLine("ID: {0}, Type: {1}", p, first[p].GetType()));
        }
    }
}