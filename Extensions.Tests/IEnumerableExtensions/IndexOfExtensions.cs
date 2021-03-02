using System.Linq;
using Extensions.IEnumerableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class IndexOfExtensions
    {
        [TestMethod]
        public void IndexOfTests()
        {
            Assert.AreEqual(1, new[] { 0, 1, 0, 0, 0 }.IndexOf(p => p == 1));
            Assert.AreEqual(0, new[] { 0, 1, 0, 0, 0 }.IndexOf(p => p == 0));

            var v = new[] { new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new[] { 7, 8, 9 } };
            Assert.AreEqual(0, v.IndexOf(p => p.Contains(1)));
            Assert.AreEqual(1, v.IndexOf(p => p.Contains(5)));
            Assert.AreEqual(2, v.IndexOf(p => p.Contains(9)));
        }
    }
}