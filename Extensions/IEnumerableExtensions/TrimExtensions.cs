using System;
using System.Collections.Generic;
using System.Linq;

namespace net.erlenddahl.Extensions.IEnumerableExtensions
{
    public static class TrimExtensions
    {
        public static List<T> Trim<T>(this IEnumerable<T> originalList, int area,  Func<IEnumerable<T>, bool> func)
        {
            var wasInside = false;
            var list = new List<T>(originalList);

            for (var i = 0; i < list.Count; i++) //Start
            {
                if (list.Count - i < area) break;
                if (func(list.SkipTake(i, area)))
                {
                    wasInside = true;
                    list = list.Skip(i).ToList();
                    break;
                }
            }
            if (!wasInside)
                return new List<T>();

            list.Reverse();
            for (var i = 0; i < list.Count; i++) //End
            {
                if (list.Count - i < area) break;
                if (func(list.SkipTake(i, area)))
                {
                    list = list.Skip(i).ToList();
                    break;
                }
            }
            list.Reverse();

            return list;
        }
    }
}