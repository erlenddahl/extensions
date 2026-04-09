using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class SafeMinExtensions
    {
        [TestMethod]
        public void SafeMinTests()
        {
            Assert.AreEqual(new[] { 51, 1, 41, 13, 31.1 }.Min(p => p), new[] { 51, 1, 41, 13, 31.1 }.SafeMin(p => p));
            Assert.AreEqual(new[] { 51, 1, 41, 13, 31.1 }.Min(), new[] { 51, 1, 41, 13, 31.1 }.SafeMin());

            Assert.AreEqual(new[] { 151, 1, 41, 143.771, 31.1 }.Min(p => p), new[] { 151, 1, 41, 143.771, 31.1 }.SafeMin(p => p));
            Assert.AreEqual(new[] { 151, 1, 41, 143.771, 31.1 }.Min(), new[] { 151, 1, 41, 143.771, 31.1 }.SafeMin());

            Assert.AreEqual(double.MinValue, new double[0].SafeMin(p => p));
            Assert.AreEqual(-999, new double[0].SafeMin(p => p, -999));
        }
    }
}