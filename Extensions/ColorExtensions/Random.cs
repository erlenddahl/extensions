using System;
using System.Drawing;

namespace net.erlenddahl.Extensions.ColorExtensions
{
    public static class RandomColors
    {
        private static Random _rnd = new Random(DateTime.Now.Millisecond);

        public static Color Random()
        {
            return Color.FromArgb(_rnd.Next(256), _rnd.Next(256), _rnd.Next(256));
        }

        public static Color Random(this Color c)
        {
            return Color.FromArgb(_rnd.Next(256), _rnd.Next(256), _rnd.Next(256));
        }

        public static string ToHex(this Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
    }
}
