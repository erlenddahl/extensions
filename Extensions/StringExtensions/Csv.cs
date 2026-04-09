using System;
using System.Linq;

namespace net.erlenddahl.Extensions.StringExtensions
{
    public static class Csv
    {

        /// <summary>
        /// Replaces the separator of a CSV line, taking double quotes into consideration.
        /// WARNING: Does not consider single quotes.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="existingSeparator"></param>
        /// <param name="newSeparator"></param>
        /// <returns></returns>
        public static string ChangeSeparator(this string str, char existingSeparator, char newSeparator)
        {
            var result = str.Split('"')
                .Select((element, index) => index % 2 == 0 // If even index
                    ? element.Split(new[] {existingSeparator}, StringSplitOptions.RemoveEmptyEntries) // Split the item
                    : new string[] {element}) // Keep the entire item
                .SelectMany(element => element).ToList();
            return string.Join(newSeparator.ToString(), result);
        }
    }
}