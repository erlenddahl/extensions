using System;
using System.Linq;

namespace Extensions.StringExtensions
{
    public static class Cleaning
    {

        /// <summary>
        /// Will remove any chars that are not digits or alphabetic characters.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CleanAlphaNumeric(this string s)
        {
            return new string(s.ToCharArray().Where(Char.IsLetterOrDigit).ToArray());
        }

        /// <summary>
        /// Will remove anything that is not a digit from a string. If removeStartingZeroes is set
        /// to true, any starting zeroes will be removed (i.e., 00115 => 115).
        /// </summary>
        /// <param name="s"></param>
        /// <param name="removeStartingZeroes"></param>
        /// <returns></returns>
        public static string CleanNumeric(this string s, bool removeStartingZeroes = true)
        {
            var sign = s.Trim().StartsWith("-") ? "-" : "";
            var cleaned = new string(s.ToCharArray().Where(Char.IsDigit).ToArray());
            if (removeStartingZeroes)
                while (cleaned.StartsWith("0") && cleaned.Length > 1)
                    cleaned = cleaned.Substring(1);
            return sign + cleaned;
        }

        /// <summary>
        /// Will remove anything that is not in the given string of legal characters.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="legalChars"></param>
        /// <param name="caseInsensitive"></param>
        /// <returns></returns>
        public static string Clean(this string s, string legalChars, bool caseInsensitive = false)
        {
            if (caseInsensitive)
            {
                var t = legalChars;
                legalChars = t.ToLower() + t.ToUpper();
            }

            var cleaned = new string(s.ToCharArray().Where(legalChars.Contains).ToArray());
            return cleaned;
        }

        /// <summary>
        /// Will remove anything that is not a digit or a punctuation from a string. If removeStartingZeroes is set
        /// to true, any starting zeroes will be removed (i.e., 00115 => 115).
        /// </summary>
        /// <param name="s"></param>
        /// <param name="removeStartingZeroes"></param>
        /// <returns></returns>
        public static string CleanNumericWithDecimals(this string s, bool removeStartingZeroes = true)
        {
            var sign = s.Trim().StartsWith("-") ? "-" : "";
            var cleaned = new string(s.ToCharArray().Where(p => char.IsDigit(p) || p == '.' || p == ',').ToArray());
            if (removeStartingZeroes)
                while (cleaned.StartsWith("0") && cleaned.Length > 1)
                    cleaned = cleaned.Substring(1);
            return sign + cleaned;
        }

        /// <summary>
        /// Will remove anything that is not a letter from a string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CleanLetters(this string s)
        {
            var cleaned = new string(s.ToCharArray().Where(Char.IsLetter).ToArray());
            return cleaned;
        }
    }
}