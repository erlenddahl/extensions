using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.Utilities.Caching;

namespace Extensions.Tests.Utilities.Caching
{
    [TestClass]
    public class LruHashSetTests
    {
        [TestMethod]
        public void ContainsTest()
        {
            var s = new LruHashSet<int>(5);

            Assert.AreEqual(false, s.Contains(5));
            s.Add(5);
            Assert.AreEqual(true, s.Contains(5));


            Assert.AreEqual(false, s.Contains(4));
            s.Add(4);
            Assert.AreEqual(true, s.Contains(4));
            Assert.AreEqual(true, s.Contains(5));
            Assert.AreEqual(2, s.Count);

            s.Add(1);
            s.Add(2);
            s.Add(3);

            Assert.AreEqual(5, s.Count);
            Assert.AreEqual(true, s.Contains(1));
            Assert.AreEqual(true, s.Contains(2));
            Assert.AreEqual(true, s.Contains(3));
            Assert.AreEqual(true, s.Contains(4));
            Assert.AreEqual(true, s.Contains(5));

            s.Add(6);
            Assert.AreEqual(false, s.Contains(5));

            s.Add(7);
            Assert.AreEqual(false, s.Contains(4));
        }

        [TestMethod]
        public void DuplicateAddTest()
        {
            var s = new LruHashSet<int>(5);

            s.Add(5);
            Assert.AreEqual(true, s.Contains(5));
            Assert.AreEqual(1, s.Count);

            s.Add(5);
            s.Add(5);
            s.Add(5);

            Assert.AreEqual(true, s.Contains(5));
            Assert.AreEqual(1, s.Count);
        }
    }
}
