using System;
using System.Collections.Generic;

namespace Extensions.IEnumerableExtensions
{
    public static class GroupAdjacentByExtensions
    {
        public static IEnumerable<List<T>> GroupAdjacentBy<T>(this IEnumerable<T> source, Func<T, T, bool> predicate)
        {
            using (var e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    var list = new List<T> { e.Current };
                    var pred = e.Current;
                    while (e.MoveNext())
                    {
                        if (predicate(pred, e.Current))
                        {
                            list.Add(e.Current);
                        }
                        else
                        {
                            yield return list;
                            list = new List<T> { e.Current };
                        }
                        pred = e.Current;
                    }
                    yield return list;
                }
            }
        }

        public static IEnumerable<TOut> GroupAdjacentBy<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, TIn, bool> predicate, Func<List<TIn>, TOut> mergeFunc)
        {
            using (var e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    var list = new List<TIn> { e.Current };
                    var pred = e.Current;
                    while (e.MoveNext())
                    {
                        if (predicate(pred, e.Current))
                        {
                            list.Add(e.Current);
                        }
                        else
                        {
                            yield return mergeFunc(list);
                            list.Clear();
                            list.Add(e.Current);
                        }
                        pred = e.Current;
                    }
                    yield return mergeFunc(list);
                }
            }
        }

        public static IEnumerable<TOut> GroupAdjacentBy<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, object> comparer, Func<List<TIn>, TOut> mergeFunc)
        {
            using (var e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    var list = new List<TIn> { e.Current };
                    var pred = e.Current;
                    while (e.MoveNext())
                    {
                        if (comparer(pred).Equals(comparer(e.Current)))
                        {
                            list.Add(e.Current);
                        }
                        else
                        {
                            yield return mergeFunc(list);
                            list.Clear();
                            list.Add(e.Current);
                        }
                        pred = e.Current;
                    }
                    yield return mergeFunc(list);
                }
            }
        }
    }
}