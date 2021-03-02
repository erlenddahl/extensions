using System;
using System.Collections.Generic;

namespace Extensions.DictionaryExtensions
{
    public static class ToDictionary
    {
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
}