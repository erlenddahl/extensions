using Extensions.StringExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class Repetition
    {

        [TestMethod]
        public void RepeatTests()
        {
            Assert.AreEqual("", "".Repeat(0));
            Assert.AreEqual("", "fgaegaew".Repeat(0));
            Assert.AreEqual("", "".Repeat(100));
            Assert.AreEqual("aaaaa", "a".Repeat(5));
            Assert.AreEqual("aaaaaaaaaa", "aa".Repeat(5));
            Assert.AreEqual("aaaaaaaaaa".Repeat(2), "aa".Repeat(10));
            Assert.AreEqual(".-€.-€.-€", ".-€".Repeat(3));
        }
    }
}