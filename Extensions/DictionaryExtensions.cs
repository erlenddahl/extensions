using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Extensions
{
    public static class DictionaryExtensions
    {
        private static bool _debug = false;

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
        public static void Increment<T>(this Dictionary<T, DoubleWrapper> dictionary, T key, double increment = 1)
        {
            if (dictionary.TryGetValue(key, out var dw)) dw.Value += increment;
            else dictionary.Add(key, new DoubleWrapper(increment));
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
        /// Returns the value of the given key if it exists, or the default value otherwise.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
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

        /// <summary>
        /// Returns true if this dictionary exists, and has a non-null value for the given key.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Has(this Dictionary<string, object> dict, string key)
        {
            return dict != null && dict.ContainsKey(key) && dict[key] != null;
        }

        /// <summary>
        /// Returns true if this dictionary exists, and has a non-null value for the given key.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Has(this Dictionary<string, string> dict, string key)
        {
            return dict != null && dict.ContainsKey(key) && dict[key] != null;
        }

        /// <summary>
        /// Reads a bool value from the given dictionary. If the key doesn't exist, false is returned.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetBool(this Dictionary<string, object> dict, string key)
        {
            if (!dict.Has(key)) return false;
            return (bool) dict[key];
        }

        /// <summary>
        /// Reads an int value from the given dictionary. If the key doesn't exist, a null value is returned.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int? GetInt(this Dictionary<string, object> dict, string key)
        {
            if (!dict.Has(key)) return null;
            if (_debug) Debug.WriteLine(key + ": " + dict[key].GetType());
            return (int?) dict[key];
        }

        /// <summary>
        /// Reads a long value from the given dictionary. If the key doesn't exist, a null value is returned.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long? GetLong(this Dictionary<string, object> dict, string key)
        {
            if (!dict.Has(key)) return null;
            if (_debug) Debug.WriteLine(key + ": " + dict[key].GetType());
            if (dict[key] is int) return (int) dict[key];
            if (dict[key] is decimal) return (long) ((decimal) dict[key]);
            return (long?) dict[key];
        }

        /// <summary>
        /// Reads a short value from the given dictionary. If the key doesn't exist, a null value is returned.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static short? GetShort(this Dictionary<string, object> dict, string key)
        {
            if (!dict.Has(key)) return null;
            if (_debug) Debug.WriteLine(key + ": " + dict[key].GetType());
            return (short) dict[key];
        }

        /// <summary>
        /// Reads a single value from the given dictionary. If the key doesn't exist, a null value is returned.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Single? GetSingle(this Dictionary<string, object> dict, string key)
        {
            if (!dict.Has(key)) return null;
            if (_debug) Debug.WriteLine(key + ": " + dict[key].GetType());
            return (Single?) dict[key];
        }

        /// <summary>
        /// Reads an absolute single value from the given dictionary. If the key doesn't exist, a null value is returned.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Single? GetAbsolutedSingle(this Dictionary<string, object> dict, string key)
        {
            if (!dict.Has(key)) return null;
            if (_debug) Debug.WriteLine(key + ": " + dict[key].GetType());
            var value = (Single?) dict[key];
            if (value.HasValue && value.Value < 0)
                return Math.Abs(value.Value);
            return value;
        }

        /// <summary>
        /// Reads a double value from the given dictionary. If the key doesn't exist, a null value is returned.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Double? GetDouble(this Dictionary<string, object> dict, string key)
        {
            if (!dict.Has(key)) return null;
            if (_debug) Debug.WriteLine(key + ": " + dict[key].GetType());
            return (Double?) dict[key];
        }

        /// <summary>
        /// Reads a datetime value from the given dictionary. If the key doesn't exist, a null value is returned.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DateTime? GetDateTime(this Dictionary<string, object> dict, string key)
        {
            if (!dict.Has(key)) return null;
            if (_debug) Debug.WriteLine(key + ": " + dict[key].GetType());
            return (DateTime?) dict[key];
        }

        /// <summary>
        /// Reads a string value from the given dictionary. If the key doesn't exist, an empty string is returned.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(this Dictionary<string, object> dict, string key)
        {
            if (!dict.Has(key)) return "";
            if (_debug) Debug.WriteLine(key + ": " + dict[key].GetType());
            return (string) dict[key];
        }

        /// <summary>
        /// Reads a string value from the given dictionary. If the key doesn't exist, an empty string is returned.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(this Dictionary<string, string> dict, string key)
        {
            if (!dict.Has(key)) return null;
            if (_debug) Debug.WriteLine(key + ": " + dict[key].GetType());
            return (string) dict[key];
        }

        /// <summary>
        /// Reads a byte array value from the given dictionary. If the key doesn't exist, an empty byte array is returned.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] GetByteArray(this Dictionary<string, object> dict, string key)
        {
            if (!dict.Has(key)) return new byte[0];
            if (_debug) Debug.WriteLine(key + ": " + dict[key].GetType());
            return (byte[]) dict[key];
        }

        /// <summary>
        /// Creates a dictionary from the given enumerable, using the key function to extract keys, and the value function to extract values.
        /// Works like the normal ToDictionary, but if there are duplicate keys, this version will solve the issue by appending (1), (2), etc
        /// to the keys until they are unique.
        /// </summary>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="list"></param>
        /// <param name="keyFunc"></param>
        /// <param name="valueFunc"></param>
        /// <returns></returns>
        public static Dictionary<string, TV> ToDictionarySafe<TI, TV>(this IEnumerable<TI> list, Func<TI, string> keyFunc, Func<TI, TV> valueFunc)
        {
            var dict = new Dictionary<string, TV>();
            foreach (var element in list)
            {
                var key = keyFunc(element);
                var value = valueFunc(element);

                var safeKey = key;
                var safeNumber = 1;
                while (dict.ContainsKey(safeKey))
                    safeKey = key + " (" + safeNumber++ + ")";

                dict.Add(safeKey, value);
            }

            return dict;
        }
    }

    public class DoubleWrapper
    {
        public double Value { get; set; }

        public DoubleWrapper(double initialValue)
        {
            Value = initialValue;
        }
    }
}
