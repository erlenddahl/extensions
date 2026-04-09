using System;
using System.Collections.Generic;
using System.Linq;
using Extensions.Tests.IntExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IntExtensions
{
    [TestClass]
    public class Range
    {
        [TestMethod]
        public void ToTests()
        {
            var p = 1.To(1).ToList();
            Assert.AreEqual(1, p.Count);
            Assert.AreEqual(1, p[0]);

            p = 1.To(1, 10).ToList();
            Assert.AreEqual(1, p.Count);
            Assert.AreEqual(1, p[0]);

            p = 10.To(100, 10).ToList();
            Assert.AreEqual(10, p.Count);
            for (int i = 1; i < 11; i++)
                Assert.AreEqual(i * 10, p[i - 1]);

            p = 0.To(10).ToList();
            Assert.AreEqual(11, p.Count);
            for (int i = 0; i < 11; i++)
                Assert.AreEqual(i, p[i]);

            p = 0.To(-10, -1).ToList();
            Assert.AreEqual(11, p.Count);
            for (int i = 0; i < 11; i++)
                Assert.AreEqual(-i, p[i]);
        }
    }
}
