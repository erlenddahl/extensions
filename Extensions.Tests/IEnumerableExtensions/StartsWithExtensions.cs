using System;
using System.Collections.Generic;
using System.Text;
using Extensions.ArrayExtensions;
using Extensions.IEnumerableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class StartsWithExtensions
    {
        [TestMethod]
        public void Itself()
        {
            var arr = new byte[] { 5, 2, 7, 8, 112 };

            Assert.AreEqual(true, arr.StartsWith(arr));
        }

        [TestMethod]
        public void SimpleTrue()
        {
            var arr = new[] { 5, 2, 7, 8, 112 };

            Assert.AreEqual(true, arr.StartsWith(5));
        }

        [TestMethod]
        public void SimpleFalse()
        {
            var arr = new[] { 5, 2, 7, 8, 112 };

            Assert.AreEqual(false, arr.StartsWith(3));
        }

        [TestMethod]
        public void PartsOfItself()
        {
            var arr = new byte[] { 5, 2, 7, 8, 112 };

            for (var i = 0; i < arr.Length; i++)
                Assert.AreEqual(true, arr.StartsWith(arr.GetRange(0, i)));
        }

        [TestMethod]
        public void EndPartsOfItself()
        {
            var arr = new byte[] { 5, 2, 7, 8, 112 };

            for (var i = 1; i < arr.Length; i++)
                Assert.AreEqual(false, arr.StartsWith(arr.GetRange(i, arr.Length - i)));
        }

        [TestMethod]
        public void ListTooShort()
        {
            var arr = new[] { 5, 2, 7, 8, 112 };

            Assert.AreEqual(false, arr.StartsWith(5, 2, 7, 8, 112, 115));
        }

        [TestMethod]
        public void MatchingNulls()
        {
            var arr = new int?[] { 5, 2, null, 8, 112 };

            Assert.AreEqual(true, arr.StartsWith(5, 2, null, 8));
        }

        [TestMethod]
        public void NonMatchingNulls()
        {
            var arr = new int?[] { 5, 2, null, 8, 112 };

            Assert.AreEqual(false, arr.StartsWith(5, null, 7, 8));
            Assert.AreEqual(false, arr.StartsWith(5, 2, null, null));
        }
    }
}
