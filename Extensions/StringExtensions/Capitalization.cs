namespace Extensions.StringExtensions
{
    public static class Capitalization
    {

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
        /// Will return a camelcase representation of the given text. Anything that is not an alphanumeric symbol will be removed (as well as digits at the start of the string).
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
                if (camel == "" && char.IsDigit(c)) continue;

                if (!char.IsLetterOrDigit(c))
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
    }
}