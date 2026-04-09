using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Utilities.MoreMathTests
{
    [TestClass]
    public class ToRadianOrDegreesTests
    {

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
    }
}