using System.Linq;
using Extensions.IEnumerableExtensions;
using Extensions.ListExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class SegmentExtensions
    {
        [TestMethod]
        public void SimpleAggregation()
        {
            var list = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var segments = list.Segment(2, p => p[0] + "-" + p[1]).ToList();

            CollectionAssert.AreEqual(new[] { "1-2", "3-4", "5-6", "7-8" }, segments);
        }

        [TestMethod]
        public void SimpleAggregation_Edge()
        {
            var list = new[] { 1, 2, 3, 4, 5, 6, 7 };
            var segments = list.Segment(2, p => string.Join("-", p)).ToList();

            CollectionAssert.AreEqual(new[] { "1-2", "3-4", "5-6", "7" }, segments);
        }

        [TestMethod]
        public void NoAggregation()
        {
            var list = new[] {1, 2, 3, 4, 5, 6, 7};
            var segments = list.Segment(2).ToList();

            Assert.AreEqual(4, segments.Count);
            CollectionAssert.AreEqual(new[] {1, 2}, segments[0]);
            CollectionAssert.AreEqual(new[] {3, 4}, segments[1]);
            CollectionAssert.AreEqual(new[] {5, 6}, segments[2]);
            CollectionAssert.AreEqual(new[] {7}, segments[3]);
        }
    }
}
