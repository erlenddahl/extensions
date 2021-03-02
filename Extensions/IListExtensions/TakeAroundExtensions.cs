using System;
using System.Collections.Generic;

namespace Extensions.IListExtensions
{
    public static class TakeAroundExtensions
    {

        /// <summary>
        /// Returns a list of all elements around the given start index that fulfills the given conditions.
        /// For example, [0,1,2,3,4,5,6,7,8,9].TakeAround(5, p =&gt; p &gt; 2, p =&gt; p &lt; 8) will return [3,4,5,6,7].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="startIndex"></param>
        /// <param name="takeBefore"></param>
        /// <param name="takeAfter"></param>
        /// <returns></returns>
        public static IEnumerable<T> TakeAround<T>(this IList<T> list, int startIndex, Func<T, bool> takeBefore, Func<T, bool> takeAfter = null)
        {
            if (takeAfter == null) takeAfter = takeBefore;

            //Identify and return the suitable objects before the start index
            var initialIndex = -1;
            for (var i = startIndex - 1; i >= 0; i--)
            {
                if (i < 0) break;
                if (!takeBefore(list[i])) break;
                initialIndex = i;
            }

            if(initialIndex >= 0)
                for (var i = initialIndex; i < startIndex; i++)
                    yield return list[i];

            //Return the start index
            yield return list[startIndex];

            //Identify and return the suitable objects after the start index
            for (var i = startIndex + 1; i < list.Count; i++)
            {
                if (!takeAfter(list[i])) break;
                yield return list[i];
            }
        }
    }
}