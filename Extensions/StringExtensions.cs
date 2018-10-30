using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Will repeat the string the given amount of times.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static string Repeat(this string s, int times)
        {
            var ns = "";
            for (var i = 0; i < times; i++)
                ns += s;
            return ns;
        }

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
        /// Will make sure all chars that are illegal in file names are removed from the given filename.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="replaceWith"></param>
        /// <returns></returns>
        public static string MakeSafeForFilename(this string str, string replaceWith = "")
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                str = str.Replace(c.ToString(), replaceWith);
            return str;
        }

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
                while (cleaned.StartsWith("0") && cleaned.Length > 1) cleaned = cleaned.Substring(1);
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
                while (cleaned.StartsWith("0") && cleaned.Length > 1) cleaned = cleaned.Substring(1);
            return sign + cleaned;
        }

        /// <summary>
        /// Will pad a string with the given char until it reaches the given length.
        /// </summary>
        /// <param name="s">The string to pad</param>
        /// <param name="length">The target length of the string</param>
        /// <param name="padding">The char to pad with</param>
        /// <returns></returns>
        public static string PadAfter(this string s, int length, char padding = ' ')
        {
            while (s.Length < length)
                s += padding;
            return s;
        }

        /// <summary>
        /// Will pad a string with the given char until it reaches the given length.
        /// </summary>
        /// <param name="s">The string to pad</param>
        /// <param name="length">The target length of the string</param>
        /// <param name="padding">The char to pad with</param>
        /// <returns></returns>
        public static string PadBefore(this string s, int length, char padding = ' ')
        {
            while (s.Length < length)
                s = padding + s;
            return s;
        }

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

        /// <summary>
        /// Replaces ONLY the first occurence of the needle.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="needle"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string ReplaceFirst(this string text, string needle, string replace)
        {
            var pos = text.IndexOf(needle);
            if (pos < 0)
                return text;
            return text.Substring(0, pos) + replace + text.Substring(pos + needle.Length);
        }

        /// <summary>
        /// Will capitalize the first letter in the given string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CapitalizeFirst(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text[0].ToString().ToUpper() + (text.Length > 1 ? text.Substring(1, text.Length - 1) : "");
        }

        /// <summary>
        /// Will attempt to convert the given string into a double value.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="decimalSeparator"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this string text, char decimalSeparator, double? defaultValue = null)
        {
            text = text.Replace(decimalSeparator, '.');
            double value;
            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                return value;
            if (defaultValue != null) return defaultValue.Value;
            throw new InvalidDataException("Couldn't parse the string '" + text + "' to a double, using the decimal separator '" + decimalSeparator + "'.");
        }

        /// <summary>
        /// Will attempt to convert the given string into a double value, attempting to guess the decimal separator
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this string text, double? defaultValue = null)
        {
            if (text.Contains(",") && !text.Contains(".")) return text.ToDouble(',');
            if (text.Contains(".") && !text.Contains(",")) return text.ToDouble('.');
            if (!text.Contains(".") && !text.Contains(",")) return text.ToDouble('.');
            if (defaultValue != null) return defaultValue.Value;
            throw new InvalidDataException("Couldn't parse the string '" + text + "' to a double (confused).");
        }

        /// <summary>
        /// Will attempt to convert the given string to an integer.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this string text, int? defaultValue = null)
        {
            int value;
            if (int.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                return value;
            if (defaultValue != null) return defaultValue.Value;
            throw new InvalidDataException("Couldn't parse the string '" + text + "' to an int.");
        }

        /// <summary>
        /// Will replace the file extension of the given filename with the new extension.
        /// </summary>
        /// <param name="oldFilename">Full path</param>
        /// <param name="newExtension">The new extension (for example ".ldb")</param>
        /// <returns></returns>
        public static string ChangeExtension(this string oldFilename, string newExtension)
        {
            if (string.IsNullOrEmpty(oldFilename)) return oldFilename;
            if (!oldFilename.Contains(".")) return oldFilename + newExtension;
            var parts = oldFilename.Split('.');
            return string.Join(".", parts.Take(parts.Length - 1)) + newExtension;
        }

        /// <summary>
        /// Will remove the file extension of the given filename.
        /// </summary>
        /// <param name="oldFilename">Full path</param>
        /// <returns></returns>
        public static string RemoveExtension(this string oldFilename)
        {
            if (string.IsNullOrWhiteSpace(oldFilename)) return oldFilename;
            return Path.Combine(Path.GetDirectoryName(oldFilename), Path.GetFileNameWithoutExtension(oldFilename));
        }

        /// <summary>
        /// Will return a list of all indices of the given needle.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="needle"></param>
        /// <returns></returns>
        public static IEnumerable<int> IndicesOf(this string text, string needle)
        {
            var i = text.IndexOf(needle, StringComparison.Ordinal);
            while (i >= 0)
            {
                yield return i;
                i = text.IndexOf(needle, i + 1, StringComparison.Ordinal);
            }
        }

        /// <summary>
        /// Will return the last indexOf the given needle BEFORE the given index.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="needle"></param>
        /// <param name="beforeIndex"></param>
        /// <returns></returns>
        public static int IndexOfBefore(this string text, string needle, int beforeIndex)
        {
            var items = text.IndicesOf(needle).TakeWhile(p => p < beforeIndex);
            if (items.Any())
                return items.Last();
            return -1;
        }

        /// <summary>
        /// Will return the last indexOf the given needle AFTER the given index.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="needle"></param>
        /// <param name="afterIndex"></param>
        /// <returns></returns>
        public static int IndexOfAfter(this string text, string needle, int afterIndex)
        {
            var items = text.IndicesOf(needle).SkipWhile(p => p <= afterIndex);
            if (items.Any())
                return items.First();
            return -1;
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

        /// <summary>
        /// Will remove any empty lines from the given string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveEmptyLines(this string text)
        {
            var lines = Regex.Split(text.Replace("\r", "\n"), "\n");
            return string.Join(Environment.NewLine, lines.Where(p => !string.IsNullOrWhiteSpace(p)));
        }

        /// <summary>
        /// Will return a camelcase representation of the given text. Everything that is not a letter will be removed.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string text)
        {
            var camel = "";
            var prevWasRemoved = false;
            var isFirst = true;
            foreach (var c in text)
            {
                if (!char.IsLetter(c))
                {
                    prevWasRemoved = true;
                    continue;
                }

                if (isFirst)
                {
                    camel += c.ToString().ToLower();
                    isFirst = prevWasRemoved = false;
                    continue;
                }

                if (prevWasRemoved)
                {
                    camel += c.ToString().ToUpper();
                    prevWasRemoved = false;
                    continue;
                }

                camel += c.ToString().ToLower();
            }
            return camel;
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

        public static string RemoveTrailingZeroes(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "0";
            while (s.EndsWith("0"))
                s = s.Substring(0, s.Length - 1);
            if (s.EndsWith(".") || s.EndsWith(",")) s = s.Substring(0, s.Length - 1);
            if (string.IsNullOrEmpty(s)) return "0";
            return s;
        }

        /// <summary>
        /// Will truncate a file path by replacing entire folder names with an ellipsis. For example: C:\folder\one\two\three\file.txt => C:\folder\...\three\file.txt
        /// </summary>
        /// <param name="path"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string TruncatePath(this string path, int length)
        {
            Func<string[], string> assemble = parts =>
            {
                var compressed = new List<string>();
                foreach (var part in parts)
                    if (part != null || compressed.Last() != null)
                        compressed.Add(part);

                return string.Join(Path.DirectorySeparatorChar.ToString(), compressed.Select(p => p ?? "...").ToArray());
            };

            var pathParts = path.Split(Path.DirectorySeparatorChar);
            while (path.Length > length)
            {
                if (pathParts.Count(p => p != null) <= 2) break;

                var middle = path.Length/2;
                var lengthSoFar = 0;
                var middleIndex = 0;
                foreach (var p in pathParts)
                {
                    if (p != null)
                        lengthSoFar += p.Length + 1;
                    if (lengthSoFar >= middle)
                        break;
                    middleIndex++;
                }

                while (middleIndex >= 0 && (middleIndex == pathParts.Length - 1 || pathParts[middleIndex] == null)) middleIndex--;
                if (middleIndex < 1) break;

                pathParts[middleIndex] = null;

                path = assemble(pathParts);

            }

            return path;
        }
    }
}
