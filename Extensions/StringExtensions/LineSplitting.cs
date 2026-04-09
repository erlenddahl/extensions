using System.Collections.Generic;

namespace net.erlenddahl.Extensions.StringExtensions
{
    public static class LineSplitting
    {
        public static IEnumerable<string> Lines(this string str)
        {
            using (System.IO.StringReader reader = new System.IO.StringReader(str))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}