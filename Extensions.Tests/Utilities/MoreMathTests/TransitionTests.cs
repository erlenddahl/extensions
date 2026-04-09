using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Utilities.MoreMathTests
{
    [TestClass]
    public class TransitionTests
    {
        [TestMethod]
        public void Lower()
        {
            Assert.AreEqual(225, MoreMath.Transition(0, 225, 60, 1800, 250));
        }

        [TestMethod]
        public void Higher()
        {
            Assert.AreEqual(250, MoreMath.Transition(10_000, 225, 60, 1800, 250));
        }

        [TestMethod]
        public void OnMinThreshold()
        {
            Assert.AreEqual(225, MoreMath.Transition(60, 225, 60, 1800, 250));
        }

        [TestMethod]
        public void OnMaxThreshold()
        {
            Assert.AreEqual(250, MoreMath.Transition(1800, 225, 60, 1800, 250));
        }

        [TestMethod]
        public void SlightlyBelowMaxThreshold()
        {
            Assert.AreEqual(249.856, MoreMath.Transition(1790, 225, 60, 1800, 250), 0.005);
        }

        [TestMethod]
        public void SlightlyAboveMinThreshold()
        {
            Assert.AreEqual(225.144, MoreMath.Transition(70, 225, 60, 1800, 250), 0.005);
        }

        [TestMethod]
        public void OneThirdBetween()
        {
            Assert.AreEqual(232.759, MoreMath.Transition(600, 225, 60, 1800, 250), 0.005);
        }

        [TestMethod]
        public void HalfwayBetween()
        {
            Assert.AreEqual(237.5, MoreMath.Transition(930, 225, 60, 1800, 250));
        }

        [TestMethod]
        public void TwoThirdsBetween()
        {
            Assert.AreEqual(241.379, MoreMath.Transition(1200, 225, 60, 1800, 250), 0.005);
        }
    }
}