using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class SkipTakeExtensions
    {

        [TestMethod]
        public void SkipTakeTests()
        {
            var l = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};

            var a = l.SkipTake(5, 5).ToList();
            Assert.AreEqual(5, a.Count);
            for (var i = 0; i < a.Count; i++)
                Assert.AreEqual(a[i], i + 5);

            a = l.SkipTake(25, 5).ToList();
            Assert.AreEqual(0, a.Count);
            
            a = l.SkipTake(12, 5).ToList();
            Assert.AreEqual(2, a.Count);
            for (var i = 0; i < a.Count; i++)
                Assert.AreEqual(a[i], i + 12);
        }
    }
}