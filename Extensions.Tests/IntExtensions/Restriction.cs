using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IntExtensions
{
    [TestClass]
    public class Restriction
    {

        [TestMethod]
        public void RestrictTests()
        {
            //Int
            Assert.AreEqual(12, 12.Restrict(0, 100));
            Assert.AreEqual(12, 12.Restrict(12, 100));
            Assert.AreEqual(12, 12.Restrict(0, 12));
            Assert.AreEqual(12, 12.Restrict(12, 12));
            Assert.AreEqual(0, 12.Restrict(-100, 0));
            Assert.AreEqual(0, 15331.Restrict(-100, 0));
            Assert.AreEqual(-100, (-15331).Restrict(-100, 0));
        }
    }
}