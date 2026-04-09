using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.ArrayExtensions
{
    [TestClass]
    public class RangeIndex
    {
        [TestMethod]
        public void GetRange_Simple()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, arr.GetRangeByIndex(0, 2));
            CollectionAssert.AreEqual(new[] { 2, 3, 4 }, arr.GetRangeByIndex(1, 3));
            CollectionAssert.AreEqual(new[] { 3, 4, 5 }, arr.GetRangeByIndex(2, 4));
        }

        [TestMethod]
        public void GetRange_Full()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(arr, arr.GetRangeByIndex(0, 4));
        }

        [TestMethod]
        public void GetRange_Outside_Throws()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            try
            {
                arr.GetRangeByIndex(0, 6);
                Assert.Fail();
            }
            catch (ArgumentException aex)
            {

            }
        }

        [TestMethod]
        public void GetRange_Empty()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(Array.Empty<int>(), arr.GetRangeByIndex(0, 0));
        }
    }
}