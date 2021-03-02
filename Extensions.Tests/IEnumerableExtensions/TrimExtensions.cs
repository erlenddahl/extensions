using System.Collections.Generic;
using System.Linq;
using Extensions.IEnumerableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class TrimExtensions
    {
        [TestMethod]
        public void TrimTests()
        {
            var l = new[] { 0, 1, 0, 0, 0, 4, 1, 0, 0, 1, 1, 2, 5, 6, 7, 19, 25, 25, 30, 40, 49, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var a = l.Trim(5, p => Enumerable.Average((IEnumerable<int>) p) > 5);
            Assert.AreEqual(14, a.Count);
            for (var i = 0; i < a.Count; i++)
                Assert.AreEqual(a[i], l[i + 11]);

            l = new[] { 0, 0, 0 };
            a = l.Trim(5, p => p.Average() > 5);
            Assert.AreEqual(0, a.Count);

            l = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            a = l.Trim(5, p => p.Average() > 5);
            Assert.AreEqual(0, a.Count);
        }
    }
}