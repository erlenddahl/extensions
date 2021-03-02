using System.Linq;
using Extensions.IEnumerableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class Range
    {
        [TestMethod]
        public void CompressToConsecutiveGroupsTests()
        {
            Check(new int[] { });
            Check(new[] {1}, new[] {1, 1});
            Check(new[] {1, 2}, new[] {1, 2});
            Check(new[] {1, 2, 3}, new[] {1, 3});
            Check(new[] {1, 3}, new[] {1, 1}, new[] {3, 3});
            Check(new[] {1, 2, 3, 5, 6, 7}, new[] {1, 3}, new[] {5, 7});
            Check(new[] {3, 6, 7, 2, 1, 5}, new[] {1, 3}, new[] {5, 7});
        }

        private void Check(int[] list, params int[][] results)
        {
            var res = list.CompressToConsecutiveGroups().ToList();
            Assert.AreEqual(results.Length, res.Count);
            for (var i = 0; i < res.Count; i++)
            {
                Assert.AreEqual(results[i][0], res[i].Start);
                Assert.AreEqual(results[i][1], res[i].End);
            }
        }
    }
}