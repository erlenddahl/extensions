using System;
using System.Collections.Generic;

namespace Extensions.DictionaryExtensions
{
    public static class ValueExtraction {
        /// <summary>
        /// Returns the value of the given key if it exists, or the default value otherwise.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <param name="useDefaultOnEmptyString"></param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default, bool useDefaultOnEmptyString = true)
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value)) return defaultValue;
            if (useDefaultOnEmptyString && value is string s && string.IsNullOrWhiteSpace(s)) return defaultValue;

            return value;
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
            return (byte[]) dict[key];
        }
    }
}