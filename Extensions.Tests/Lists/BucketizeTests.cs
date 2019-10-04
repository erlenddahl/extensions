using System;
using System.Collections.Generic;
using System.Linq;
using Extensions.IEnumerable;
using Extensions.Lists;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Lists
{
    [TestClass]
    public class BucketizeExtensionsTests
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

        [TestMethod]
        public void IntDictTests()
        {
            var items = Enumerable.Range(10, 10).ToDictionary(p => (int)p, p => 3);
            var correct = Enumerable.Range(0, 5).Select(p => 6).ToArray();
            var buckets = items.Bucketize(correct.Length);
            Assert.AreEqual(5, buckets.Length);

            for (var i = 0; i < correct.Length; i++)
                Assert.AreEqual(correct[i], buckets[i]);
        }

        [TestMethod]
        public void DoubleDictTests()
        {
            var items = Enumerable.Range(10, 10).ToDictionary(p => (double)p, p => 3);
            var correct = Enumerable.Range(0, 5).Select(p => 6).ToArray();
            var buckets = items.Bucketize(correct.Length);
            Assert.AreEqual(5, buckets.Length);

            for (var i = 0; i < correct.Length; i++)
                Assert.AreEqual(correct[i], buckets[i]);
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
