using Extensions.IntExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IntExtensions
{
    [TestClass]
    public class Rounding
    {
        [TestMethod]
        public void RoundToNearestTests()
        {
            Assert.AreEqual(0, -4.RoundToNearest(10));
            Assert.AreEqual(0, -5.RoundToNearest(10));
            Assert.AreEqual(-10, -6.RoundToNearest(10));
            Assert.AreEqual(-10, -7.RoundToNearest(10));
            Assert.AreEqual(0, 0.RoundToNearest(10));
            Assert.AreEqual(0, -4.RoundToNearest(10));
            Assert.AreEqual(0, -1.RoundToNearest(10));
            Assert.AreEqual(0, 1.RoundToNearest(10));
            Assert.AreEqual(0, 4.RoundToNearest(10));
            Assert.AreEqual(0, 5.RoundToNearest(10));
            Assert.AreEqual(10, 7.RoundToNearest(10));

            Assert.AreEqual(60, 64.RoundToNearest(10));

            Assert.AreEqual(0, 7.RoundToNearest(100));
            Assert.AreEqual(100, 55.RoundToNearest(100));
        }
    }
}