using System.Collections.Generic;

namespace Extensions.IEnumerableExtensions
{
    public static class EveryExtensions
    {

        public static IEnumerable<T> Every<T>(this IEnumerable<T> list, int step)
        {
            var count = 0;
            foreach (var v in list)
            {
                if (count % step == 0)
                {
                    count = 0;
                    yield return v;
                }
                count++;
            }
        }
    }
}