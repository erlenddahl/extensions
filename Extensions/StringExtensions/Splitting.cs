using System;
using System.Text.RegularExpressions;

namespace net.erlenddahl.Extensions.StringExtensions
{
    public static class Splitting
    {
        /// <summary>
        /// Will use Regex to split the string with the given separator.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] Split(this string s, string separator)
        {
            return Regex.Split(s, separator);
        }

        /// <summary>
        /// Will split the given string into lines.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string[] GetLines(this string s)
        {
            return Regex.Split(s, Environment.NewLine);
        }
    }
}