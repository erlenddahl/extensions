using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.ArrayExtensions
{
    [TestClass]
    public class Range
    {
        [TestMethod]
        public void GetRange_Simple()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, arr.GetRange(0, 3));
            CollectionAssert.AreEqual(new[] { 2, 3, 4 }, arr.GetRange(1, 3));
            CollectionAssert.AreEqual(new[] { 3, 4, 5 }, arr.GetRange(2, 3));
        }

        [TestMethod]
        public void GetRange_Full()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(arr, arr.GetRange(0, 5));
        }

        [TestMethod]
        public void GetRange_Outside_Throws()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            try
            {
                arr.GetRange(0, 6);
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
            CollectionAssert.AreEqual(Array.Empty<int>(), arr.GetRange(0, 0));
        }
    }
}
