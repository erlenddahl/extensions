using System.Linq;
using Extensions.IEnumerableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class EveryExtensions
    {
        [TestMethod]
        public void EveryTests()
        {
            var l = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

            var a = l.Every(1).ToList();
            Assert.AreEqual(16, a.Count);
            for (var i = 0; i < a.Count; i++)
                Assert.AreEqual(i, a[i]);

            a = l.Every(5).ToList();
            Assert.AreEqual(4, a.Count);
            Assert.AreEqual(0, a[0]);
            Assert.AreEqual(5, a[1]);
            Assert.AreEqual(10, a[2]);
            Assert.AreEqual(15, a[3]);
        }
    }
}
