using System.Linq;
using Extensions.IEnumerableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class SafeMaxExtensions
    {
        [TestMethod]
        public void SafeMaxTests()
        {
            Assert.AreEqual(new[] { 51, 1, 41, 13, 31.1 }.Max(p => p), new[] { 51, 1, 41, 13, 31.1 }.SafeMax(p => p));
            Assert.AreEqual(new[] { 51, 1, 41, 13, 31.1 }.Max(), new[] { 51, 1, 41, 13, 31.1 }.SafeMax());

            Assert.AreEqual(new[] { 151, 1, 41, 143.771, 31.1 }.Max(p => p), new[] { 151, 1, 41, 143.771, 31.1 }.SafeMax(p => p));
            Assert.AreEqual(new[] { 151, 1, 41, 143.771, 31.1 }.Max(), new[] { 151, 1, 41, 143.771, 31.1 }.SafeMax());

            Assert.AreEqual(double.MinValue, new double[0].SafeMax(p => p));
            Assert.AreEqual(-999, new double[0].SafeMax(p => p, -999));
        }
    }
}