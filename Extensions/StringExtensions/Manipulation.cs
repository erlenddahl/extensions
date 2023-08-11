using System.Collections.Generic;

namespace Extensions.StringExtensions
{
    public static class Manipulation
    {

        /// <summary>
        /// Shortens the given string if it is longer than the given limit,
        /// and optionally inserts a suffix to indicate this.
        /// Example: "I am a string".MaxLength(4, "[...]") => "I am [...]"
        /// </summary>
        /// <param name="str"></param>
        /// <param name="limit"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string MaxLength(this string str, int limit, string suffix="")
        {
            if (str.Length <= limit) return str;
            return str.Substring(0, limit) + suffix;
        }
    }
}