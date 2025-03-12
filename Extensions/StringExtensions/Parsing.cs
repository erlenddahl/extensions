using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Extensions.StringExtensions
{
    public static class Parsing
    {

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
        /// Splits the given text using the given separator, and parses each part as a double using
        /// the given CultureInfo (or InvariantCulture if none is given).
        /// </summary>
        /// <param name="text"></param>
        /// <param name="separator"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        public static double[] Doubles(this string text, char separator, CultureInfo ci = null)
        {
            ci = ci ?? CultureInfo.InvariantCulture;
            return text.Split(separator).Select(p => double.Parse(p, ci)).ToArray();
        }

        /// <summary>
        /// Splits the given text into lines, the splits each line using the given separator, and parses each
        /// part as a double using the given CultureInfo (or InvariantCulture if none is given).
        /// Returns a 2D array where each row represents the lines in the input string, and the contents
        /// of each row is the parsed numbers of this row.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="separator"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        public static double[][] LinesWithDoubles(this string text, char separator, CultureInfo ci = null)
        {
            ci = ci ?? CultureInfo.InvariantCulture;
            return text.Lines().Select(p => p.Doubles(separator, ci)).ToArray();
        }

        /// <summary>
        /// Splits the given text using the given separator, and parses each part as an int.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="separator"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        public static int[] Ints(this string text, char separator)
        {
            return text.Split(separator).Select(int.Parse).ToArray();
        }
    }
}