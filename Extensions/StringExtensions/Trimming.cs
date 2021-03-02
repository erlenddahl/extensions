namespace Extensions.StringExtensions
{
    public static class Trimming
    {
        public static string RemoveTrailingZeroes(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "0";
            while (s.EndsWith("0"))
                s = s.Substring(0, s.Length - 1);
            if (s.EndsWith(".") || s.EndsWith(",")) s = s.Substring(0, s.Length - 1);
            if (string.IsNullOrEmpty(s)) return "0";
            return s;
        }
    }
}
