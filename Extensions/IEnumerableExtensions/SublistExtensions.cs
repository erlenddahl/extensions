using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.IEnumerableExtensions
{
    public static class SublistExtensions
    {

        /// <summary>
        /// Returns sub sets with the given size. For example, use [1,2,3,4].Sublists(2) to get [1,2] and [3,4].
        /// Or, with overlap, use [1,2,3,4,5,6].Sublists(3, 1) to get [1,2,3], [3,4,5] and [5,6].
        /// The final sub set is not guaranteed to be of the given size.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="size"></param>
        /// <param name="overlap"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> Sublists<T>(this IEnumerable<T> list, int size, int overlap = 0)
        {
            if (overlap >= size) throw new ArgumentException("Overlap cannot be larger than or equal to sublist size; then sublist generation would never finish.");

            var subList = new List<T>();
            foreach (var item in list)
            {
                subList.Add(item);

                if (subList.Count >= size)
                {
                    yield return subList;

                    if (overlap > 0)
                    {
                        var n = new List<T>(size);
                        n.AddRange(subList.Skip(size - overlap));
                        subList = n;
                    }else
                        subList = new List<T>(size);
                }
            }

            if (subList.Count > overlap)
                yield return subList;
        }
    }
}
