namespace Extensions.StringExtensions
{
    public static class Trimming
    {
        public static string RemoveTrailingZeroes(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "0";
            var keep = s.Length;
            for (var i = s.Length - 1; i >= 0; i--)
            {
                if (s[i] != '0')
                {
                    keep = i + 1;
                    if (s[i] == '.' || s[i] == ',')
                    {
                        keep = i;
                    }
                    break;
                }
            }

            if (keep == s.Length) return s;

            if (keep <= 0) return "0";
            return s.Substring(0, keep);
        }
    }
}
