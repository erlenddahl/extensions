using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class MoreMath
    {
        /// <summary>
        /// Will calculate the angle C, given the sides a, b and c, using the law of cosines: c2 = a2 + b2 − 2ab cos(C)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns>The angle in degrees.</returns>
        public static double CalculateAngleC(double a, double b, double c)
        {
            return ToDegrees(Math.Acos((Math.Pow(c, 2) - Math.Pow(a, 2) - Math.Pow(b, 2)) / (-2 * a * b)));
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static double ToDegrees(double radians)
        {
            return radians / 0.0174532925199433;
        }

        /// <summary>
        /// Convert degrees to radians.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static double ToRadians(double degrees)
        {
            return degrees * 0.0174532925199433;
        }

        public static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double r = 6376500; // meters

            lat1 = ToRadians(lat1);
            lat2 = ToRadians(lat2);
            lon1 = ToRadians(lon1);
            lon2 = ToRadians(lon2);

            var sdlat = Math.Sin((lat2 - lat1) / 2);
            var sdlon = Math.Sin((lon2 - lon1) / 2);
            var q = sdlat * sdlat + Math.Cos(lat1) * Math.Cos(lat2) * sdlon * sdlon;
            var d = 2 * r * Math.Asin(Math.Sqrt(q));

            return d;
        }
    }
}
