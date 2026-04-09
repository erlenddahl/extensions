using System;
using System.IO;

namespace net.erlenddahl.Extensions.StringExtensions
{
    public static class Keeping
    {

        /// <summary>
        /// Will cut away everything after the given needle, including the needle itself.
        /// </summary>
        /// <param name="s">The string to cut</param>
        /// <param name="needle">A substring describing where to cut</param>
        /// <returns>The string, cut at the needle</returns>
        public static string KeepBefore(this string s, string needle)
        {
            var i = s.IndexOf(needle, StringComparison.Ordinal);
            if (i >= 0)
                return s.Substring(0, i);
            return s;
        }

        /// <summary>
        /// Will cut away everything before the given needle, excluding the needle itself.
        /// </summary>
        /// <param name="s">The string to cut</param>
        /// <param name="needle">A substring describing where to cut</param>
        /// <param name="keepNeedle">If the needle should be kept or cut</param>
        /// <returns>The string, cut at the needle</returns>
        public static string KeepAfter(this string s, string needle, bool keepNeedle = true)
        {
            var i = s.IndexOf(needle, StringComparison.Ordinal);
            if (i >= 0)
                return s.Substring(i + (keepNeedle ? 0 : needle.Length));
            return s;
        }

        /// <summary>
        /// Will extract a string between the two needles in the given input string. Note: any occurences of the end needle
        /// before the start needle will be ignored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="removeNeedles"></param>
        /// <returns></returns>
        public static string KeepBetween(this string text, string start, string end, bool removeNeedles = true)
        {
            if (!text.Contains(start) || !text.Contains(end)) throw new InvalidDataException("The given string does not contain one or more of the needles.");

            var startIndex = text.IndexOf(start);
            text = text.Substring(removeNeedles ? startIndex + start.Length : startIndex);

            if (!text.Contains(end)) throw new InvalidDataException("The given string does not contain one or more of the needles.");

            var endIndex = text.IndexOf(end);

            endIndex += removeNeedles ? 0 : end.Length;
            return text.Substring(0, endIndex);
        }
    }
}