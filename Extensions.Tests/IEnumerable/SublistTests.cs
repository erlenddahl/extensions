using System;
using System.Collections.Generic;
using System.Linq;
using Extensions.IEnumerable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerable
{
    [TestClass]
    public class SublistTests
    {
        [TestMethod]
        public void Basic()
        {
            var list = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var sublists = list.Sublists(2).ToList();

            Assert.AreEqual(5, sublists.Count);
            CollectionAssert.AreEqual(new List<int>() { 0, 1 }, sublists[0]);
            CollectionAssert.AreEqual(new List<int>() { 2, 3 }, sublists[1]);
            CollectionAssert.AreEqual(new List<int>() { 4, 5 }, sublists[2]);
            CollectionAssert.AreEqual(new List<int>() { 6, 7 }, sublists[3]);
            CollectionAssert.AreEqual(new List<int>() { 8, 9 }, sublists[4]);
        }

        [TestMethod]
        public void Overflow()
        {
            var list = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

            var sublists = list.Sublists(2).ToList();

            Assert.AreEqual(6, sublists.Count);
            CollectionAssert.AreEqual(new List<int>() { 0, 1 }, sublists[0]);
            CollectionAssert.AreEqual(new List<int>() { 2, 3 }, sublists[1]);
            CollectionAssert.AreEqual(new List<int>() { 4, 5 }, sublists[2]);
            CollectionAssert.AreEqual(new List<int>() { 6, 7 }, sublists[3]);
            CollectionAssert.AreEqual(new List<int>() { 8, 9 }, sublists[4]);
            CollectionAssert.AreEqual(new List<int>() { 10 }, sublists[5]);
        }

        [TestMethod]
        public void Single()
        {
            var list = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var sublists = list.Sublists(20).ToList();

            Assert.AreEqual(1, sublists.Count);
            CollectionAssert.AreEqual(list, sublists[0]);
        }
    }
}
