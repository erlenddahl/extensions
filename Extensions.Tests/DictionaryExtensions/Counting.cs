using System;
using System.Collections.Generic;
using System.Text;
using Extensions.DictionaryExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.DictionaryExtensions
{
    [TestClass]
    public class Counting
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
    }
}
