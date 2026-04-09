using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.IEnumerableExtensions;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class SublistExtensions
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

        [TestMethod]
        public void Basic_Overlap()
        {
            var list = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var sublists = list.Sublists(2, 1).ToList();

            Assert.AreEqual(9, sublists.Count);
            CollectionAssert.AreEqual(new List<int>() { 0, 1 }, sublists[0]);
            CollectionAssert.AreEqual(new List<int>() { 1, 2 }, sublists[1]);
            CollectionAssert.AreEqual(new List<int>() { 2, 3 }, sublists[2]);
            CollectionAssert.AreEqual(new List<int>() { 3, 4 }, sublists[3]);
            CollectionAssert.AreEqual(new List<int>() { 4, 5 }, sublists[4]);
            CollectionAssert.AreEqual(new List<int>() { 5, 6 }, sublists[5]);
            CollectionAssert.AreEqual(new List<int>() { 6, 7 }, sublists[6]);
            CollectionAssert.AreEqual(new List<int>() { 7, 8 }, sublists[7]);
            CollectionAssert.AreEqual(new List<int>() { 8, 9 }, sublists[8]);
        }

        [TestMethod]
        public void Basic_Overlap2()
        {
            var list = new List<int>() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

            var sublists = list.Sublists(3, 1).ToList();

            Assert.AreEqual(5, sublists.Count);
            CollectionAssert.AreEqual(new List<int>() {0, 1, 2}, sublists[0]);
            CollectionAssert.AreEqual(new List<int>() {2, 3, 4}, sublists[1]);
            CollectionAssert.AreEqual(new List<int>() {4, 5, 6}, sublists[2]);
            CollectionAssert.AreEqual(new List<int>() {6, 7, 8}, sublists[3]);
            CollectionAssert.AreEqual(new List<int>() {8, 9}, sublists[4]);
        }

        [TestMethod]
        public void Basic_Overlap3()
        {
            var list = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var sublists = list.Sublists(3, 2).ToList();

            Assert.AreEqual(8, sublists.Count);
            CollectionAssert.AreEqual(new List<int>() { 0, 1, 2 }, sublists[0]);
            CollectionAssert.AreEqual(new List<int>() { 1, 2, 3 }, sublists[1]);
            CollectionAssert.AreEqual(new List<int>() { 2, 3, 4 }, sublists[2]);
            CollectionAssert.AreEqual(new List<int>() { 3, 4, 5 }, sublists[3]);
            CollectionAssert.AreEqual(new List<int>() { 4, 5, 6 }, sublists[4]);
            CollectionAssert.AreEqual(new List<int>() { 5, 6, 7 }, sublists[5]);
            CollectionAssert.AreEqual(new List<int>() { 6, 7, 8 }, sublists[6]);
            CollectionAssert.AreEqual(new List<int>() { 7, 8, 9 }, sublists[7]);
        }

        [TestMethod]
        public void TooLargeOverlap_Throws()
        {
            var list = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            try
            {
                var sublists = list.Sublists(3, 3).ToList();
                Assert.Fail();
            }
            catch (ArgumentException aex)
            {

            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
        }
    }
}
