using System;
using Extensions.IEnumerableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class MinMaxExtensions
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

        [TestMethod]
        public void EmptyTest()
        {
            var mm = DoubleMinMax.Empty;
            Assert.AreEqual(mm.Min, double.MaxValue);
            Assert.AreEqual(mm.Max, double.MinValue);
        }

        [TestMethod]
        public void ExtendTests()
        {
            var mm = DoubleMinMax.Empty;
            mm.Extend(0, 10);
            Assert.AreEqual(0, mm.Min);
            Assert.AreEqual(10, mm.Max);

            mm.Extend(0, 10);
            Assert.AreEqual(0, mm.Min);
            Assert.AreEqual(10, mm.Max);

            mm.Extend(0, 7);
            Assert.AreEqual(0, mm.Min);
            Assert.AreEqual(10, mm.Max);

            mm.Extend(5, 10);
            Assert.AreEqual(0, mm.Min);
            Assert.AreEqual(10, mm.Max);

            mm.Extend(-5, 10);
            Assert.AreEqual(-5, mm.Min);
            Assert.AreEqual(10, mm.Max);

            mm.Extend(5, 15);
            Assert.AreEqual(-5, mm.Min);
            Assert.AreEqual(15, mm.Max);
        }

        [TestMethod]
        public void DoubleExtendTests()
        {
            var mm = DoubleMinMax.Empty;
            mm.Extend(0.51251, 20.1511);

            Assert.AreEqual(0.51251, mm.Min);
            Assert.AreEqual(20.1511, mm.Max);
        }
    }
}
