using System.Collections.Generic;

namespace net.erlenddahl.Extensions.StringExtensions
{
    public static class Substrings
    {

        /// <summary>
        /// Extracts and returns the string chopped into parts of the given lengths.
        /// Example: "I am a string".SubstringsOfLength(4, 6, 1) => ["I am", " a str", "i"]
        /// </summary>
        /// <param name="str"></param>
        /// <param name="lengths"></param>
        /// <returns></returns>
        public static IEnumerable<string> SubstringsOfLength(this string str, params int[] lengths)
        {
            foreach (var l in lengths)
            {
                yield return str.Substring(0, l);
                str = str.Substring(l);
            }
        }
    }
}