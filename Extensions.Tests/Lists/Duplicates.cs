using Extensions.Lists;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Tests.Lists
{
    [TestClass]
    public class Duplicates
    {
        [TestMethod]
        public void RemoveSequentialDuplicates_Int()
        {
            var arr = new[] { 0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 8, 8, 9, 9, 9, 9, 9, 9, 9, 9, 9, 8, 8, 8, 7, 7, 7, 6, 6, 5, 4, 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 0 };
            var correct = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };

            CollectionAssert.AreEqual(correct, arr.RemoveSequentialDuplicates().ToList());
        }

        [TestMethod]
        public void RemoveSequentialDuplicates_Int_NoChange()
        {
            var arr = new[] { 0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 8, 8, 9, 9, 9, 9, 9, 9, 9, 9, 9, 8, 8, 8, 7, 7, 7, 6, 6, 5, 4, 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 0 };

            CollectionAssert.AreEqual(arr, arr.RemoveSequentialDuplicates((a, b) => false).ToList());
        }

        [TestMethod]
        public void RemoveSequentialDuplicates_Int_IndexBounds()
        {
            CollectionAssert.AreEqual(new[] { 0 }, new[] { 0 }.RemoveSequentialDuplicates().ToArray());
            CollectionAssert.AreEqual(new[] { 0 }, new[] { 0, 0, 0, 0, 0, 0, 0 }.RemoveSequentialDuplicates().ToArray());
            CollectionAssert.AreEqual(new int[0], new int[0].RemoveSequentialDuplicates().ToArray());
        }

        [TestMethod]
        public void RemoveSequentialDuplicates_Object()
        {
            var arr = new[] { 0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 8, 8, 9, 9, 9, 9, 9, 9, 9, 9, 9, 8, 8, 8, 7, 7, 7, 6, 6, 5, 4, 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 0 }.Select(p => new { A = p });
            var correct = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }.Select(p => new { A = p }).ToList();

            CollectionAssert.AreEqual(correct, arr.RemoveSequentialDuplicates().ToList());
        }

        [TestMethod]
        public void RemoveSequentialDuplicates_Object_Custom()
        {
            var arr = new[] { 0, 1, 2, 2, 3, 3, 7, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 8, 8, 9, 9, 9, 9, 9, 9, 9, 9, 9, 8, 8, 8, 7, 7, 7, 6, 6, 5, 4, 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 0 }.Select(p => new { A = p });
            var correct = new[] { 0, 2, 7, 3, 5, 7, 9, 7, 5, 3, 1 }.Select(p => new { A = p }).ToList();

            var nope = arr.RemoveSequentialDuplicates((a, b) => Math.Abs(a.A - b.A) < 2).ToList();
            CollectionAssert.AreEqual(correct, nope);
        }
    }
}
