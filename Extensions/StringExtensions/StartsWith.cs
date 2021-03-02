namespace Extensions.StringExtensions
{
    public static class StartsWith
    {

        /// <summary>
        /// Returns true if the given string starts with any of the given needle strings.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="needles"></param>
        /// <returns></returns>
        public static bool StartsWithAny(this string str, params string[] needles)
        {
            foreach(var n in needles)
                if (str.StartsWith(n))
                    return true;
            return false;
        }
    }
}