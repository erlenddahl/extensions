using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.ArrayExtensions;

namespace Extensions.Tests.ArrayExtensions
{
    [TestClass]
    public class MergeArraysTests
    {
        [TestMethod]
        public void Two()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            var arr2 = new[] { 6, 7, 8, 9, 10 };

            CollectionAssert.AreEqual(arr.Concat(arr2).ToArray(), arr.Merge(arr2));
        }

        [TestMethod]
        public void Two_FirstEmpty()
        {
            var arr = Array.Empty<int>();
            var arr2 = new[] { 6, 7, 8, 9, 10 };

            CollectionAssert.AreEqual(arr.Concat(arr2).ToArray(), arr.Merge(arr2));
        }
        [TestMethod]
        public void Two_SecondEmpty()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            var arr2 = Array.Empty<int>();

            CollectionAssert.AreEqual(arr.Concat(arr2).ToArray(), arr.Merge(arr2));
        }

        [TestMethod]
        public void Three()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            var arr2 = new[] { 6, 7, 8, 9, 10 };
            var arr3 = new[] { 11, 12, 13 };

            CollectionAssert.AreEqual(arr.Concat(arr2).Concat(arr3).ToArray(), arr.Merge(arr2, arr3));
        }

        [TestMethod]
        public void Four()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            var arr2 = new[] { 6, 7, 8, 9, 10 };
            var arr3 = new[] { 11, 12, 13 };
            var arr4 = new[] { 14, 15, 16, 17 };

            CollectionAssert.AreEqual(arr.Concat(arr2).Concat(arr3).Concat(arr4).ToArray(), arr.Merge(arr2, arr3, arr4));
        }

        [TestMethod]
        public void Five()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            var arr2 = new[] { 6, 7, 8, 9, 10 };
            var arr3 = new[] { 11, 12, 13 };
            var arr4 = new[] { 14, 15, 16, 17 };
            var arr5 = new[] { 18, 19, 20 };

            CollectionAssert.AreEqual(arr.Concat(arr2).Concat(arr3).Concat(arr4).Concat(arr5).ToArray(), arr.Merge(arr2, arr3, arr4, arr5));
        }

        [TestMethod]
        public void Five_OneEmpty()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            var arr2 = new[] { 6, 7, 8, 9, 10 };
            var arr3 = new[] { 11, 12, 13 };
            var arr4 = Array.Empty<int>();
            var arr5 = new[] { 18, 19, 20 };

            CollectionAssert.AreEqual(arr.Concat(arr2).Concat(arr3).Concat(arr4).Concat(arr5).ToArray(), arr.Merge(arr2, arr3, arr4, arr5));
        }
    }
}