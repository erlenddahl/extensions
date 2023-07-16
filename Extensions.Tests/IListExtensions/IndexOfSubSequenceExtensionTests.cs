using Extensions.IListExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IListExtensions
{
    [TestClass]
    public class IndexOfSubSequenceExtensionTests
    {
        [TestMethod]
        public void AtStartOfList()
        {
            var list = new[] { 1, 3, 2, 1, 2, 3, 4, 5 };
            var ix = list.IndexOf(new[] { 1, 3, 2 });
            Assert.AreEqual(0, ix);
        }

        [TestMethod]
        public void MiddleOfList()
        {
            var list = new[] { 1, 3, 2, 1, 2, 3, 4, 5 };
            var ix = list.IndexOf(new[] { 2, 1, 2 });
            Assert.AreEqual(2, ix);
        }

        [TestMethod]
        public void EndOfList()
        {
            var list = new[] { 1, 3, 2, 1, 2, 3, 4, 5 };
            var ix = list.IndexOf(new[] { 2, 3, 4, 5 });
            Assert.AreEqual(4, ix);
        }

        [TestMethod]
        public void SkipAtStartOfList()
        {
            var list = new[] { 1, 3, 2, 1, 2, 3, 4, 1, 3, 2, 5 };
            var ix = list.IndexOf(new[] { 1, 3, 2 }, 3);
            Assert.AreEqual(7, ix);
        }

        [TestMethod]
        public void NotFound()
        {
            var list = new[] { 1, 3, 2, 1, 2, 3, 4, 1, 3, 2, 5 };
            var ix = list.IndexOf(new[] { 1, 3, 2 }, 99);
            Assert.AreEqual(-1, ix);
        }
    }
}