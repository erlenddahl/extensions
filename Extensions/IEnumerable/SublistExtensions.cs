using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.IEnumerable
{
    public static class SublistExtensions
    {
        /// <summary>
        /// Returns sub sets with the given size. For example, use [1,2,3,4].Sublists(2) to get [1,2] and [3,4].
        /// The final sub set is not guaranteed to be of the given size.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> Sublists<T>(this IEnumerable<T> list, int size)
        {
            var subList = new List<T>();
            foreach (var item in list)
            {
                subList.Add(item);

                if (subList.Count >= size)
                {
                    yield return subList;
                    subList = new List<T>();
                }
            }

            if (subList.Any())
                yield return subList;
        }
    }
}
