using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests
{
    [TestClass]
    public class MoreMathTests
    {
        [TestMethod]
        public void CalculateAngleCTests()
        {
            Assert.AreEqual(37, MoreMath.CalculateAngleC(8, 11, 6.67), 0.1);
            Assert.AreEqual(60, MoreMath.CalculateAngleC(10, 10, 10), 0.1);
            Assert.AreEqual(62.2, MoreMath.CalculateAngleC(9, 5, 8), 0.1);
            Assert.AreEqual(180, MoreMath.CalculateAngleC(10, 10, 20), 0.1);
        }

        [TestMethod]
        public void HaversineDistanceTests()
        {
            var coords = new[]
            {
                new[] {12.214555153, 56.125551627},
                new[] {12.213555153, 56.123551627},
                new[] {1.2156721233, 35.1356712346},
                new[] {55.153647312, 56.125551627},
                new[] {87.125676, 84.1235676},
                new[] {23.124526374, 88.1234587}
            };
            foreach(var a in coords)
            foreach (var b in coords)
            {
                Debug.WriteLine(a[0] + ", " + a[1] + " => " + b[0] + ", " + b[1]);
                var expected = new GeoCoordinate(a[0], a[1]).GetDistanceTo(new GeoCoordinate(b[0], b[1]));
                Debug.WriteLine(expected);
                Assert.AreEqual(expected, MoreMath.HaversineDistance(a[0], a[1], b[0], b[1]), 0.0000005);
            }
        }
    }
}
