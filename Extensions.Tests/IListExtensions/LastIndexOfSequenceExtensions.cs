using Extensions.IListExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IListExtensions
{
    [TestClass]
    public class LastIndexOfSequenceExtensions
    {
        [TestMethod]
        public void AtStartOfList()
        {
            var list = new[] { 1, 1, 1, 1, 2, 3, 4, 5 };
            var ix = list.LastIndexOfSequence(p => p == 1, 3);
            Assert.AreEqual(0, ix.Start);
            Assert.AreEqual(3, ix.End);
        }

        [TestMethod]
        public void InMiddleOfList()
        {
            var list = new[] { 0, 2, 1, 1, 1, 1, 4, 5 };
            var ix = list.LastIndexOfSequence(p => p == 1, 3);
            Assert.AreEqual(2, ix.Start);
            Assert.AreEqual(5, ix.End);
        }

        [TestMethod]
        public void AtEndOfList()
        {
            var list = new[] { 1, 2, 3, 1, 2, 1, 1, 1 };
            var ix = list.LastIndexOfSequence(p => p == 1, 3);
            Assert.AreEqual(5, ix.Start);
            Assert.AreEqual(7, ix.End);
        }

        [TestMethod]
        public void NoMatch()
        {
            var list = new[] { 1, 2, 3, 1, 2, 3, 1, 1 };
            var ix = list.LastIndexOfSequence(p => p == 1, 3);
            Assert.AreEqual(-1, ix.Start);
            Assert.AreEqual(-1, ix.End);
        }

        [TestMethod]
        public void EntireList()
        {
            var list = new[] { 1, 1, 1, 1, 1, 1, 1, 1 };
            var ix = list.LastIndexOfSequence(p => p == 1, 3);
            Assert.AreEqual(0, ix.Start);
            Assert.AreEqual(7, ix.End);
        }

        [TestMethod]
        public void MultipleMatches()
        {
            var list = new[] { 1, 1, 1, 1, 3, 2, 1, 1, 1, 1 };
            var ix = list.LastIndexOfSequence(p => p == 1, 3);
            Assert.AreEqual(6, ix.Start);
            Assert.AreEqual(9, ix.End);
        }
    }
}