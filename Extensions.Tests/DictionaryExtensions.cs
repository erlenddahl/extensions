using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests
{
    [TestClass]
    public class DictionaryExtensions
    {
        [TestMethod]
        public void IncrementTests()
        {
            var d = new Dictionary<string, double>()
            {
                {"a", 4},
                {"b", 2}
            };

            d.Increment("a");
            Assert.AreEqual(5, d["a"]);
            Assert.AreEqual(2, d["b"]);

            d.Increment("a", 5);
            Assert.AreEqual(10, d["a"]);
            Assert.AreEqual(2, d["b"]);

            d.Increment("b", 2);
            Assert.AreEqual(10, d["a"]);
            Assert.AreEqual(4, d["b"]);

            d.Increment("c");
            Assert.AreEqual(10, d["a"]);
            Assert.AreEqual(4, d["b"]);
            Assert.AreEqual(1, d["c"]);

            d.Increment("d", 10);
            Assert.AreEqual(10, d["a"]);
            Assert.AreEqual(4, d["b"]);
            Assert.AreEqual(1, d["c"]);
            Assert.AreEqual(10, d["d"]);

            d.Increment("c", 4);
            Assert.AreEqual(10, d["a"]);
            Assert.AreEqual(4, d["b"]);
            Assert.AreEqual(5, d["c"]);
            Assert.AreEqual(10, d["d"]);
        }

        [TestMethod]
        public void GetValueOrDefaultTests()
        {
            var d = new Dictionary<string, double>()
            {
                {"a", 4},
                {"b", 2}
            };

            Assert.AreEqual(10, d.GetValueOrDefault("c", 10));
            Assert.AreEqual(0, d.GetValueOrDefault("c", 0));
            Assert.AreEqual(0, d.GetValueOrDefault("c"));
            Assert.AreEqual(4, d.GetValueOrDefault("a"));
            Assert.AreEqual(2, d.GetValueOrDefault("b"));
        }

        [TestMethod]
        public void ToDictionarySafeTests()
        {
            var arr = new int[] { 1, 2, 3, 4, 5 };
            var d = arr.ToDictionarySafe(k => k.ToString(), v => v);

            Assert.AreEqual(5, d.Count);
            for (var i = 1; i < 6; i++)
                Assert.AreEqual(i, d[i.ToString()]);


            arr = new int[] { 1, 2, 3, 4, 5, 5 };
            d = arr.ToDictionarySafe(k => k.ToString(), v => v);

            Assert.AreEqual(6, d.Count);
            for (var i = 1; i < 6; i++)
                Assert.AreEqual(i, d[i.ToString()]);

            Assert.AreEqual(5, d["5 (1)"]);
        }
    }
}
