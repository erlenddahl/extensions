using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.EnumExtensions
{
    public static class EnumItems
    {
        /// <summary>
        /// Gets all items for an enum type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAllItems<T>() where T : Enum
        {
            foreach (object item in Enum.GetValues(typeof(T)))
            {
                yield return (T) item;
            }
        }
    }
}
