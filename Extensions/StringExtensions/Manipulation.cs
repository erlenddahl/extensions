namespace net.erlenddahl.Extensions.StringExtensions
{
    public static class Manipulation
    {

        /// <summary>
        /// Shortens the given string if it is longer than the given limit,
        /// and optionally inserts a suffix to indicate this.
        /// Example: "I am a string".MaxLength(4, "[...]") => "I am [...]"
        /// </summary>
        /// <param name="str"></param>
        /// <param name="limit"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string MaxLength(this string str, int limit, string suffix="")
        {
            if (str.Length <= limit) return str;
            return str.Substring(0, limit) + suffix;
        }

        /// <summary>
        /// Returns the replacement string if the given string is null
        /// or white space (string.IsNullOrWhiteSpace(str)).
        /// If it's not null or white space, the original string is
        /// returned unchanged.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string Or(this string str, string replacement)
        {
            if (!string.IsNullOrWhiteSpace(str)) return str;
            return replacement;
        }
    }
}