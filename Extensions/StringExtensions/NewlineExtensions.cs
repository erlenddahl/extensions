using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace net.erlenddahl.Extensions.StringExtensions
{
    public static class NewlineExtensions
    {

        /// <summary>
        /// Will append the given number of line breaks to the string.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static string Br(this string s, int times = 1)
        {
            return s + Environment.NewLine.Repeat(times);
        }

        /// <summary>
        /// Will remove any empty lines from the given string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveEmptyLines(this string text)
        {
            var lines = Regex.Split(text.Replace("\r", "\n"), "\n");
            return String.Join(Environment.NewLine, lines.Where(p => !String.IsNullOrWhiteSpace(p)));
        }
    }
}