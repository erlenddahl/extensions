using System;
using Extensions.IEnumerable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerable
{
    [TestClass]
    public class MinMaxExtensionTests
    {
        [TestMethod]
        public void DoubleTests()
        {
            var items = new double[] { 4, 1, 5, 4, 2 };
            var mm = items.MinMax();
            Assert.AreEqual(1, mm.Min);
            Assert.AreEqual(5, mm.Max);
        }

        [TestMethod]
        public void IntTests()
        {
            var items = new int[] { 4, 1, 5, 4, 2 };
            var mm = items.MinMax();
            Assert.AreEqual(1, mm.Min);
            Assert.AreEqual(5, mm.Max);
        }

        [TestMethod]
        public void DoubleSelectorTests()
        {
            var items = new double[] { 4, 1, 5, 4, 2 };
            var mm = items.MinMax(p => 2 * p);
            Assert.AreEqual(2, mm.Min);
            Assert.AreEqual(10, mm.Max);
        }

        [TestMethod]
        public void IntSelectorTests()
        {
            var items = new int[] { 4, 1, 5, 4, 2 };
            var mm = items.MinMax(p => 2 * p);
            Assert.AreEqual(2, mm.Min);
            Assert.AreEqual(10, mm.Max);
        }
    }
}
