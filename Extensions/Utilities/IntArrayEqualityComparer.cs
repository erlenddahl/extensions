using System.Collections.Generic;

namespace Extensions.Utilities
{
    /// <summary>
    /// Non-generic variant of Jon Skeet's ArrayEqualityComparer for using arrays as Dictionary keys.
    /// https://stackoverflow.com/a/7244729/153336
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class IntArrayEqualityComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] first, int[] second)
        {
            if (first == second)
            {
                return true;
            }
            if (first == null || second == null)
            {
                return false;
            }
            if (first.Length != second.Length)
            {
                return false;
            }
            for (int i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(int[] array)
        {
            unchecked
            {
                if (array == null)
                {
                    return 0;
                }
                int hash = 17;
                foreach (var element in array)
                {
                    hash = hash * 31 + element;
                }
                return hash;
            }
        }
    }
}