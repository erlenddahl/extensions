using System.Linq;
using Extensions.IEnumerableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class SafeAverageExtensions
    {
        [TestMethod]
        public void SafeAverageTests()
        {
            Assert.AreEqual(new[] { 51, 1, 41, 13, 31.1 }.Average(p => p), new[] { 51, 1, 41, 13, 31.1 }.SafeAverage(p => p));
            Assert.AreEqual(new[] { 51, 1, 41, 13, 31.1 }.Average(), new[] { 51, 1, 41, 13, 31.1 }.SafeAverage());

            Assert.AreEqual(new[] { 151, 1, 41, 143.771, 31.1 }.Average(p => p), new[] { 151, 1, 41, 143.771, 31.1 }.SafeAverage(p => p));
            Assert.AreEqual(new[] { 151, 1, 41, 143.771, 31.1 }.Average(), new[] { 151, 1, 41, 143.771, 31.1 }.SafeAverage());

            Assert.AreEqual(double.MinValue, new double[0].SafeAverage(p => p));
            Assert.AreEqual(-999, new double[0].SafeAverage(p => p, -999));
        }
    }
}