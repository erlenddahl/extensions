using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.IEnumerableExtensions;

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

        [TestMethod]
        public void AFewNulls()
        {
            var a = new object();
            var b = new object();
            var l = new[] { null, null, null, a, a, a, a, a, b, b, b, b, b, b, b, b, b, b, b, b };
            Assert.AreEqual(b, l.Majority());
        }

        [TestMethod]
        public void MostNulls()
        {
            var a = new object();
            var b = new object();
            var l = new[] { null, null, null,null,null,null,null, a, a, a, a, a, b, b, b, b, b };
            Assert.AreEqual(null, l.Majority());
        }
    }
}