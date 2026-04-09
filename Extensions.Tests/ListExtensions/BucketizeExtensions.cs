using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.IEnumerableExtensions;
using net.erlenddahl.Extensions.ListExtensions;

namespace Extensions.Tests.ListExtensions
{
    [TestClass]
    public class BucketizeExtensions
    {
        [TestMethod]
        public void BasicTest()
        {
            Test(new double[] { 0, 1, 2, 3, 4 }, new[] { 1, 1, 1, 1, 1 });
        }

        [TestMethod]
        public void SimpleTests()
        {
            Test(new double[] { 1, 2, 3, 4, 5 }, new[] { 1, 1, 1, 1, 1 });
            Test(new double[] { 5, 6, 7, 8, 9 }, new[] { 1, 1, 1, 1, 1 });
            Test(new double[] { 500, 600, 700, 800, 900 }, new[] { 1, 1, 1, 1, 1 });
        }

        [TestMethod]
        public void CustomMinMaxTests()
        {
            var items = new double[] {1, 2, 3, 4, 5};
            var correct = new[] {1, 1, 1, 1, 1, 0, 0, 0, 0, 0};

            var buckets = items.Bucketize(10, new DoubleMinMax(1, 10));
            Assert.AreEqual(10, buckets.Length);

            for (var i = 0; i < correct.Length; i++)
                Assert.AreEqual(correct[i], buckets[i]);
        }

        [TestMethod]
        public void LargerListsTests()
        {
            Test(Enumerable.Range(10, 10).Select(p => (double)p).ToArray(), Enumerable.Range(0, 5).Select(p => 2).ToArray());
            Test(Enumerable.Range(10, 15).Select(p => (double)p).ToArray(), Enumerable.Range(0, 5).Select(p => 3).ToArray());
        }

        private void Test(double[] items, int[] correct)
        {
            var buckets = items.Bucketize(correct.Length);
            Assert.AreEqual(5, buckets.Length);

            for (var i = 0; i < correct.Length; i++)
                Assert.AreEqual(correct[i], buckets[i]);
        }
    }
}
