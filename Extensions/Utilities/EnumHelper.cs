using System;
using System.Collections.Generic;

namespace net.erlenddahl.Extensions.Utilities
{
    public static class EnumHelper
    {
        /// <summary>
        /// Gets all items for an enum type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetValues<T>() where T : Enum
        {
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                yield return (T)item;
            }
        }

        /// <summary>
        /// Attempts to parse the given string value to an enum value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T Parse<T>(string value, bool ignoreCase = true)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
    }
}
