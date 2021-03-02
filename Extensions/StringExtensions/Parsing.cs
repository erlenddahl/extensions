using System.Globalization;
using System.IO;

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
    }
}