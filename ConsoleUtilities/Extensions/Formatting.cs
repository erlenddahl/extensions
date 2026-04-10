using System.Text;

namespace net.erlenddahl.ConsoleUtilities.Extensions
{
    public static class Formatting
    {
        /// <summary>
        /// Returns the TimeSpan formatted as a short, but readable string, for example:
        /// 45 s, 01:35 m, 14:45:00, 1d 03:15:11
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public static string ToShortPrettyFormat(this TimeSpan span)
        {
            if (span.TotalSeconds < 60) return span.TotalSeconds.ToString("n0") + " s";

            var sb = new StringBuilder();
            if (span.Days > 0)
                sb.AppendFormat("{0}d ", span.Days);
            if (span.Days > 0 || span.Hours > 0)
                sb.AppendFormat("{0:00}:", span.Hours);
            sb.AppendFormat("{0:00}:{1:00}", span.Minutes, span.Seconds);

            if (span.Days <= 0 && span.Hours <= 0)
                sb.Append(" m");

            return sb.ToString();
        }
    }
}
