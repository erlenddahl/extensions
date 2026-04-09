using System;
using System.Collections.Generic;
using System.Linq;

namespace net.erlenddahl.Extensions.IEnumerableExtensions
{
    public static class FillMissingValuesExtensions
    {
        private struct ValueAtIndex<T>
        {
            public T Value;
            public int Index;
        }

        public static int FillMissingValues<TSource, TValueType>(this IEnumerable<TSource> items, Func<TSource, bool> hasValue, Func<TSource, TValueType> getValue, Action<TSource, int, TValueType> setValue)
        {
            var arr = items.ToArray();

            var values = new List<ValueAtIndex<TValueType>>();
            for (var i = 0; i < arr.Length; i++)
            {
                if (hasValue(arr[i]))
                    values.Add(new ValueAtIndex<TValueType>() {Value = getValue(arr[i]), Index = i});
            }

            if (values.Count < 2) return 0;

            var count = 0;
            for (var i = 0; i < values.Count - 1; i++)
            {
                if(EqualityComparer<TValueType>.Default.Equals(values[i].Value, values[i+1].Value))
                    for (var j = values[i].Index + 1; j < values[i + 1].Index; j++)
                    {
                        setValue(arr[j], j, values[i].Value);
                        count++;
                    }
            }

            return count;
        }
    }
}
