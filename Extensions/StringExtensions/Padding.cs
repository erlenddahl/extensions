namespace net.erlenddahl.Extensions.StringExtensions
{
    public static class Padding
    {

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
        /// Will pad a string in both directions with the given char until it reaches the given length.
        /// </summary>
        /// <param name="s">The string to pad</param>
        /// <param name="length">The target length of the string</param>
        /// <param name="padding">The char to pad with</param>
        /// <returns></returns>
        public static string PadCenter(this string s, int length, char padding = ' ')
        {
            var toPad = length - s.Length;
            var before = (int) (toPad / 2);
            return s.PadBefore(before + s.Length, padding).PadAfter(length, padding);
        }
    }
}