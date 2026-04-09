using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.IEnumerableExtensions;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class NullableAverageExtensions
    {
        [TestMethod]
        public void NullableAverageTests()
        {
            Assert.AreEqual(new[] { 51, 1, 13, 31.1 }.Average(p => p), new double?[] { 51, 1, null, 13, 31.1 }.NullableAverage(p => p));
            Assert.AreEqual(new[] { 51, 1, 13, 31.1 }.Average(), new double?[] { 51, 1, null, 13, 31.1 }.NullableAverage());

            Assert.AreEqual(new[] { 151, 143.771, 31.1 }.Average(p => p), new double?[] { 151, null, null, 143.771, 31.1 }.NullableAverage(p => p));
            Assert.AreEqual(new[] { 151, 143.771, 31.1 }.Average(), new double?[] { 151, null, null, 143.771, 31.1 }.NullableAverage());

            Assert.AreEqual(null, new double[0].NullableAverage(p => p));
            Assert.AreEqual(-999, new double[0].NullableAverage(p => p, -999));
        }
    }
}