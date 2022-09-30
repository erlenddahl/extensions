using Extensions.DateTimeExtensions;
using Extensions.DoubleExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.DoubleExtensions
{
    [TestClass]
    public class Rounding
    {
        [TestMethod]
        public void RoundToNearestTests()
        {
            Assert.AreEqual(0, -4.0.RoundToNearest(10));
            Assert.AreEqual(0, -5.0.RoundToNearest(10));
            Assert.AreEqual(-10, -6.0.RoundToNearest(10));
            Assert.AreEqual(-10, -7.0.RoundToNearest(10));
            Assert.AreEqual(0, 0.0.RoundToNearest(10));
            Assert.AreEqual(0, -4.0.RoundToNearest(10));
            Assert.AreEqual(0, -1.0.RoundToNearest(10));
            Assert.AreEqual(0, 1.0.RoundToNearest(10));
            Assert.AreEqual(0, 4.0.RoundToNearest(10));
            Assert.AreEqual(0, 5.0.RoundToNearest(10));
            Assert.AreEqual(10, 7.0.RoundToNearest(10));

            Assert.AreEqual(60, 64.0.RoundToNearest(10));

            Assert.AreEqual(0, 7.0.RoundToNearest(100));
            Assert.AreEqual(100, 55.0.RoundToNearest(100));
        }

        [TestMethod]
        public void RoundDown()
        {
            Assert.AreEqual(0, 4.3.Round(10, RoundingDirection.Down));
            Assert.AreEqual(0, 0.3.Round(10, RoundingDirection.Down));
            Assert.AreEqual(0, 9.3.Round(10, RoundingDirection.Down));
            Assert.AreEqual(0, 6.3.Round(10, RoundingDirection.Down));
        }

        [TestMethod]
        public void RoundUp()
        {
            Assert.AreEqual(10, 4.3.Round(10, RoundingDirection.Up));
            Assert.AreEqual(10, 0.3.Round(10, RoundingDirection.Up));
            Assert.AreEqual(10, 9.3.Round(10, RoundingDirection.Up));
            Assert.AreEqual(10, 6.3.Round(10, RoundingDirection.Up));
        }

        [TestMethod]
        public void RoundNearest()
        {
            Assert.AreEqual(0, 4.3.Round(10, RoundingDirection.Nearest));
            Assert.AreEqual(0, 0.3.Round(10, RoundingDirection.Nearest));
            Assert.AreEqual(10, 9.3.Round(10, RoundingDirection.Nearest));
            Assert.AreEqual(10, 6.3.Round(10, RoundingDirection.Nearest));
        }
    }
}