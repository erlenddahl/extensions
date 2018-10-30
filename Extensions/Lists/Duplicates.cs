using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Lists
{
   public static class Duplicates
    {
        /// <summary>
        /// Will return the list without any sequential duplicates.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="equalityComparer">A function that returns true if both parameters are equal. If null, it will be defined as a == b.</param>
        /// <returns></returns>
        public static List<T> RemoveSequentialDuplicates<T>(this IEnumerable<T> list, Func<T, T, bool> equalityComparer = null)
        {
            if (equalityComparer == null) equalityComparer = (a, b) => EqualityComparer<T>.Default.Equals(a, b);
            var arr = list.ToList();
            for (var i = 0; i < arr.Count - 1; i++)
                if (equalityComparer(arr[i], arr[i + 1]))
                {
                    arr.RemoveAt(i + 1);
                    i--;
                }
            return arr;
        }
    }
}
