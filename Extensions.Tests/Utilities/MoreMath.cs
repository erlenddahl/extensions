using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Extensions.IEnumerableExtensions;
using Extensions.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Utilities
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
        public void ToRadians()
        {
            for (var i = -720; i < 720; i++)
                Assert.AreEqual(i / 360d * 2 * Math.PI, MoreMath.ToRadians(i), 0.000000001);
        }

        [TestMethod]
        public void ToDegrees()
        {
            for (var i = -4 * Math.PI; i < 4 * Math.PI; i += 1 / 360d)
                Assert.AreEqual(i / (2 * Math.PI) * 360d, MoreMath.ToDegrees(i), 0.000000001);
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

            var correct = new[]
            {
                0,
                244.358007082913,
                2619630.21946818,
                4778731.05975191,
                8374778.88147311,
                3593482.46450521,
                244.358007082913,
                0,
                2619385.88674566,
                4778842.35385624,
                8374895.43509213,
                3593726.50728744,
                2619630.21946818,
                2619385.88674566,
                0,
                6296973.14355838,
                9671121.53398988,
                6212022.53047232,
                4778731.05975191,
                4778842.35385624,
                6296973.14355838,
                0,
                3598375.80647798,
                4432772.30966672,
                8374778.88147311,
                8374895.43509213,
                9671121.53398988,
                3598375.80647798,
                0,
                7123543.70839201,
                3593482.46450521,
                3593726.50728744,
                6212022.53047232,
                4432772.30966672,
                7123543.70839201,
                0
            }.ToQueue();

            foreach (var a in coords)
            foreach (var b in coords)
            {
                var expected = correct.Dequeue();
                Assert.AreEqual(expected, MoreMath.HaversineDistance(a[0], a[1], b[0], b[1]), 0.0000005);
            }
        }
    }
}
