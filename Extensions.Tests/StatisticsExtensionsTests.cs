using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests
{
    [TestClass]
    public class StatisticsExtensionsTests
    {
        [TestMethod]
        public void StandardDeviationTests()
        {
            var nums = new double[] { 12, 52, 22, 43, 12.43, 42.51, 19.999, 25, 29, 49, 38, 40.14 };
            Assert.AreEqual(13.84207, nums.StandardDeviation(), 0.00001);
            Assert.AreEqual(13.25277, nums.StandardDeviation(StdType.Population), 0.00001);

            Assert.AreEqual(double.NaN, new double[0].StandardDeviation());
            Assert.AreEqual(double.NaN, new double[0].StandardDeviation(StdType.Population));

            Assert.AreEqual(double.NaN, new double[] { 5 }.StandardDeviation());
            Assert.AreEqual(0, new double[] { 5 }.StandardDeviation(StdType.Population));
        }

        [TestMethod]
        public void VarianceTests()
        {
            var nums = new double[] { 12, 52, 22, 43, 12.43, 42.51, 19.999, 25, 29, 49, 38, 40.14 };
            Assert.AreEqual(191.60287, nums.Variance(), 0.00001);
            Assert.AreEqual(175.63597, nums.Variance(StdType.Population), 0.00001);

            Assert.AreEqual(double.NaN, new double[0].Variance());
            Assert.AreEqual(double.NaN, new double[0].Variance(StdType.Population));

            Assert.AreEqual(double.NaN, new double[] { 5 }.Variance());
            Assert.AreEqual(0, new double[] { 5 }.Variance(StdType.Population));
        }
    }
}
