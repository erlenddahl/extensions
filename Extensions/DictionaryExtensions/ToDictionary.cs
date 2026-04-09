using System;
using System.Collections.Generic;

namespace net.erlenddahl.Extensions.DictionaryExtensions
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
        /// <param name="duplicateHandling"></param>
        /// <returns></returns>
        public static Dictionary<string, TV> ToDictionarySafe<TI, TV>(this IEnumerable<TI> list, Func<TI, string> keyFunc, Func<TI, TV> valueFunc, DictionaryDuplicateKeyHandling duplicateHandling = DictionaryDuplicateKeyHandling.AppendNumbers)
        {
            var dict = new Dictionary<string, TV>();

            if (duplicateHandling == DictionaryDuplicateKeyHandling.UseFirstValue)
            {
                foreach (var element in list)
                {
                    var key = keyFunc(element);
                    if (dict.ContainsKey(key)) continue;
                    dict.Add(key, valueFunc(element));
                }

                return dict;
            }

            var numberDict = new Dictionary<string, int>();
            foreach (var element in list)
            {
                var key = keyFunc(element);
                var value = valueFunc(element);

                if (!numberDict.TryGetValue(key, out var safeNumber))
                {
                    safeNumber = 1;
                    numberDict.Add(key, safeNumber + 1);
                }
                else
                    numberDict[key]++;

                var safeKey = safeNumber == 1 ? key : key + " (" + safeNumber++ + ")";

                dict.Add(safeKey, value);
            }

            return dict;
        }

        /// <summary>
        /// Creates a dictionary from the given enumerable, using the key function to extract keys, and the value function to extract values.
        /// Works like the normal ToDictionary, but if there are duplicate keys, this version will keep the first item.
        /// </summary>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="list"></param>
        /// <param name="keyFunc"></param>
        /// <param name="valueFunc"></param>
        /// <returns></returns>
        public static Dictionary<TK, TV> ToDictionarySafe<TI, TK, TV>(this IEnumerable<TI> list, Func<TI, TK> keyFunc, Func<TI, TV> valueFunc)
        {
            var dict = new Dictionary<TK, TV>();
            foreach (var element in list)
            {
                var key = keyFunc(element);
                if (dict.ContainsKey(key)) continue;
                dict.Add(key, valueFunc(element));
            }

            return dict;
        }
    }

    public enum DictionaryDuplicateKeyHandling
    {
        AppendNumbers,
        UseFirstValue
    }
}