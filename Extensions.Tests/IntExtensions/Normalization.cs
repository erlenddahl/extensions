using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IntExtensions
{
    [TestClass]
    public class Normalization
    {
        [TestMethod]
        public void NormalizeTests()
        {
            Assert.AreEqual(0, 0.Normalize(0, 0, 0, 0));
            Assert.AreEqual(1, 1.Normalize(1, 1, 1, 1));

            Assert.AreEqual(1, 1.Normalize(0, 1, 0, 1));
            Assert.AreEqual(0, 0.Normalize(0, 1, 0, 1));
        }
    }
}