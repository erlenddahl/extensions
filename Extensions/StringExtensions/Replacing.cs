using System;

namespace net.erlenddahl.Extensions.StringExtensions
{
    public static class Replacing
    {

        /// <summary>
        /// Replaces ONLY the first occurence of the needle.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="needle"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string ReplaceFirst(this string text, string needle, string replace)
        {
            var pos = text.IndexOf(needle, StringComparison.InvariantCulture);
            if (pos < 0)
                return text;
            return text.Substring(0, pos) + replace + text.Substring(pos + needle.Length);
        }
        
        /// <summary>
        /// Finds the last occurrence of the "before" string that is before the index. Then finds the first occurrence of 
        /// the "after" string that is after the index. Then removes everything inbetween, and replaces it with "replaceWith".
        /// </summary>
        /// <param name="text"></param>
        /// <param name="index"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="replaceWith"></param>
        /// <returns></returns>
        public static string ReplaceSurrounding(this string text, int index, string before, string after, string replaceWith)
        {
            var start = text.IndexOfBefore(before, index);
            var end = text.IndexOfAfter(after, index);

            if (start < 0 || end < 0) return text;

            end += after.Length;

            var beforeContents = text.Substring(0, start);
            var afterContents = text.Substring(end);
            return beforeContents + replaceWith + afterContents;
        }
    }
}