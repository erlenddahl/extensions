using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.DictionaryExtensions
{
    [TestClass]
    public class BucketizeExtensions
    {
        [TestMethod]
        public void IntDictTests()
        {
            var items = Enumerable.Range(10, 10).ToDictionary(p => (int) p, p => 3);
            var correct = Enumerable.Range(0, 5).Select(p => 6).ToArray();
            var buckets = items.Bucketize(correct.Length);
            Assert.AreEqual(5, buckets.Length);

            for (var i = 0; i < correct.Length; i++)
                Assert.AreEqual(correct[i], buckets[i]);
        }

        [TestMethod]
        public void DoubleDictTests()
        {
            var items = Enumerable.Range(10, 10).ToDictionary(p => (double) p, p => 3);
            var correct = Enumerable.Range(0, 5).Select(p => 6).ToArray();
            var buckets = items.Bucketize(correct.Length);
            Assert.AreEqual(5, buckets.Length);

            for (var i = 0; i < correct.Length; i++)
                Assert.AreEqual(correct[i], buckets[i]);
        }
    }
}
