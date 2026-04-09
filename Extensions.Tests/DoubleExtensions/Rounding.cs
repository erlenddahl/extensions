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
        public void RoundDown_Positive()
        {
            Assert.AreEqual(0, 4.3.Round(10, RoundingDirection.Down));
            Assert.AreEqual(0, 0.3.Round(10, RoundingDirection.Down));
            Assert.AreEqual(0, 9.3.Round(10, RoundingDirection.Down));
            Assert.AreEqual(0, 6.3.Round(10, RoundingDirection.Down));
        }

        [TestMethod]
        public void RoundUp_Positive()
        {
            Assert.AreEqual(10, 4.3.Round(10, RoundingDirection.Up));
            Assert.AreEqual(10, 0.3.Round(10, RoundingDirection.Up));
            Assert.AreEqual(10, 9.3.Round(10, RoundingDirection.Up));
            Assert.AreEqual(10, 6.3.Round(10, RoundingDirection.Up));
        }

        [TestMethod]
        public void RoundDown_Negative()
        {
            Assert.AreEqual(-10, (-4.3).Round(10, RoundingDirection.Down));
            Assert.AreEqual(-10, (-0.3).Round(10, RoundingDirection.Down));
            Assert.AreEqual(-10, (-9.3).Round(10, RoundingDirection.Down));
            Assert.AreEqual(-10, (-6.3).Round(10, RoundingDirection.Down));
        }

        [TestMethod]
        public void RoundUp_Negative()
        {
            Assert.AreEqual(0, (-4.3).Round(10, RoundingDirection.Up));
            Assert.AreEqual(0, (-0.3).Round(10, RoundingDirection.Up));
            Assert.AreEqual(0, (-9.3).Round(10, RoundingDirection.Up));
            Assert.AreEqual(0, (-6.3).Round(10, RoundingDirection.Up));
        }

        [TestMethod]
        public void RoundNearest_Positive()
        {
            Assert.AreEqual(0, 4.3.Round(10, RoundingDirection.Nearest));
            Assert.AreEqual(0, 0.3.Round(10, RoundingDirection.Nearest));
            Assert.AreEqual(10, 9.3.Round(10, RoundingDirection.Nearest));
            Assert.AreEqual(10, 6.3.Round(10, RoundingDirection.Nearest));
        }

        [TestMethod]
        public void RoundNearest_Negative()
        {
            Assert.AreEqual(0, (-4.3).Round(10, RoundingDirection.Nearest));
            Assert.AreEqual(0, (-0.3).Round(10, RoundingDirection.Nearest));
            Assert.AreEqual(-10, (-9.3).Round(10, RoundingDirection.Nearest));
            Assert.AreEqual(-10, (-6.3).Round(10, RoundingDirection.Nearest));
        }

        [TestMethod]
        public void RoundTowardsZero_Positive()
        {
            Assert.AreEqual(0, 4.3.Round(10, RoundingDirection.TowardsZero));
            Assert.AreEqual(0, 0.3.Round(10, RoundingDirection.TowardsZero));
            Assert.AreEqual(0, 9.3.Round(10, RoundingDirection.TowardsZero));
            Assert.AreEqual(0, 6.3.Round(10, RoundingDirection.TowardsZero));
            Assert.AreEqual(10, 19.3.Round(10, RoundingDirection.TowardsZero));
            Assert.AreEqual(10, 16.3.Round(10, RoundingDirection.TowardsZero));
        }

        [TestMethod]
        public void RoundTowardsZero_Negative()
        {
            Assert.AreEqual(0,( -4.3).Round(10, RoundingDirection.TowardsZero));
            Assert.AreEqual(0, (-0.3).Round(10, RoundingDirection.TowardsZero));
            Assert.AreEqual(-0, (-9.3).Round(10, RoundingDirection.TowardsZero));
            Assert.AreEqual(-0, (-6.3).Round(10, RoundingDirection.TowardsZero));
            Assert.AreEqual(-10, (-19.3).Round(10, RoundingDirection.TowardsZero));
            Assert.AreEqual(-10, (-16.3).Round(10, RoundingDirection.TowardsZero));
        }

        [TestMethod]
        public void RoundAwayFromZero_Positive()
        {
            Assert.AreEqual(10, 4.3.Round(10, RoundingDirection.AwayFromZero));
            Assert.AreEqual(10, 0.3.Round(10, RoundingDirection.AwayFromZero));
            Assert.AreEqual(10, 9.3.Round(10, RoundingDirection.AwayFromZero));
            Assert.AreEqual(10, 6.3.Round(10, RoundingDirection.AwayFromZero));
            Assert.AreEqual(20, 19.3.Round(10, RoundingDirection.AwayFromZero));
            Assert.AreEqual(20, 16.3.Round(10, RoundingDirection.AwayFromZero));
        }

        [TestMethod]
        public void RoundAwayFromZero_Negative()
        {
            Assert.AreEqual(-10,( -4.3).Round(10, RoundingDirection.AwayFromZero));
            Assert.AreEqual(-10, (-0.3).Round(10, RoundingDirection.AwayFromZero));
            Assert.AreEqual(-10, (-9.3).Round(10, RoundingDirection.AwayFromZero));
            Assert.AreEqual(-10, (-6.3).Round(10, RoundingDirection.AwayFromZero));
            Assert.AreEqual(-20, (-19.3).Round(10, RoundingDirection.AwayFromZero));
            Assert.AreEqual(-20, (-16.3).Round(10, RoundingDirection.AwayFromZero));
        }
    }
}