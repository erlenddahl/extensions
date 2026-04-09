namespace net.erlenddahl.Extensions.StringExtensions
{
    public static class Repetetion
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
    }
}