using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.DoubleExtensions;

namespace Extensions.Tests.DoubleExtensions
{
    [TestClass]
    public class Restriction
    {

        [TestMethod]
        public void RestrictTests()
        {
            //Double
            Assert.AreEqual(12.54, 12.54.Restrict(0, 100));
            Assert.AreEqual(12.54, 12.54.Restrict(12.54, 100));
            Assert.AreEqual(12.54, 12.54.Restrict(0, 12.54));
            Assert.AreEqual(12.54, 12.54.Restrict(12.54, 12.54));
            Assert.AreEqual(0, 12.54.Restrict(-100, 0));
            Assert.AreEqual(0, 15331.0.Restrict(-100, 0));
            Assert.AreEqual(-100, (-15331.0).Restrict(-100, 0));
        }
    }
}
