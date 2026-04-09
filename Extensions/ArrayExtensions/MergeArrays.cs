using System;

namespace net.erlenddahl.Extensions.ArrayExtensions
{
    public static class MergeArrays
    {
        public static T[] Merge<T>(this T[] a, T[] b)
        {
            var result = new T[a.Length + b.Length];
            Array.Copy(a, 0, result, 0, a.Length);
            Array.Copy(b, 0, result, a.Length, b.Length);
            return result;
        }

        public static T[] Merge<T>(this T[] a, T[] b, T[] c)
        {
            var result = new T[a.Length + b.Length + c.Length];
            Array.Copy(a, 0, result, 0, a.Length);
            Array.Copy(b, 0, result, a.Length, b.Length);
            Array.Copy(c, 0, result, a.Length + b.Length, c.Length);
            return result;
        }

        public static T[] Merge<T>(this T[] a, T[] b, T[] c, T[] d)
        {
            var result = new T[a.Length + b.Length + c.Length + d.Length];
            Array.Copy(a, 0, result, 0, a.Length);
            Array.Copy(b, 0, result, a.Length, b.Length);
            Array.Copy(c, 0, result, a.Length + b.Length, c.Length);
            Array.Copy(d, 0, result, a.Length + b.Length + c.Length, d.Length);
            return result;
        }

        public static T[] Merge<T>(this T[] a, T[] b, T[] c, T[] d, T[] e)
        {
            var result = new T[a.Length + b.Length + c.Length + d.Length + e.Length];
            Array.Copy(a, 0, result, 0, a.Length);
            Array.Copy(b, 0, result, a.Length, b.Length);
            Array.Copy(c, 0, result, a.Length + b.Length, c.Length);
            Array.Copy(d, 0, result, a.Length + b.Length + c.Length, d.Length);
            Array.Copy(e, 0, result, a.Length + b.Length + c.Length + d.Length, e.Length);
            return result;
        }
    }
}