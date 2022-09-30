using System;
using System.Linq;
using Extensions.IEnumerableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class MajorityExtensions
    {
        [TestMethod]
        public void AllIdentical()
        {
            var l = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
            Assert.AreEqual(3, l.Majority());
        }

        [TestMethod]
        public void ClearMajority()
        {
            var l = new[] { 3, 3, 3, 2, 3, 3, 3, 3, 3, 3 };
            Assert.AreEqual(3, l.Majority());
        }

        [TestMethod]
        public void SlightMajority()
        {
            var l = new[] { 3, 2, 3, 2, 1, 3, 1, 2, 1, 3 };
            Assert.AreEqual(3, l.Majority());
        }

        [TestMethod]
        public void Tie()
        {
            var l = new[] { 1, 4, 1, 4, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 1, 4, 6, 55, 6 };
            var m = l.Majority();
            if (m != 2 && m != 3)
                Assert.Fail();
        }

        [TestMethod]
        public void EmptyList()
        {
            var l = Array.Empty<int>();
            try
            {
                l.Majority();
                Assert.Fail();
            }
            catch (Exception tex)
            {
                Console.WriteLine(tex.Message);
            }
        }
    }
}