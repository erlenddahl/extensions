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
    }
}