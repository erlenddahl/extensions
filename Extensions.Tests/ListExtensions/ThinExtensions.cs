using System;
using System.Linq;
using Extensions.ListExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.ListExtensions
{
    [TestClass]
    public class ThinExtensions
    {
        [TestMethod]
        public void Thin_SameSize()
        {
            var arr = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            var narr = arr.Thin(20).ToArray();
            CollectionAssert.AreEqual(arr, narr);
        }

        [TestMethod]
        public void Thin_Too_Small_Exception()
        {
            try
            {
                var arr = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19};
                var narr = arr.Thin(1).ToArray();
                Assert.Fail();
            }
            catch (ArgumentException aex)
            {

            }
        }

        [TestMethod]
        public void Thin_Smallest()
        {
            var arr = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            var narr = arr.Thin(2).ToArray();
            CollectionAssert.AreEqual(new[] {0, 19}, narr);
        }

        [TestMethod]
        public void Thin_Smaller()
        {
            var arr = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            var narr = arr.Thin(10).ToArray();
            CollectionAssert.AreEqual(new[] { 0, 2, 4, 6, 8, 11, 13, 15, 17, 19 }, narr);
        }

        [TestMethod]
        public void Thin_Larger()
        {
            var arr = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            var narr = arr.Thin(100).ToArray();
            CollectionAssert.AreEqual(arr, narr);
        }
    }
}
