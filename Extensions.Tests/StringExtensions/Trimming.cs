using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.StringExtensions;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class Trimming
    {
        [TestMethod]
        public void RemoveTrailingZeroesTests()
        {
            Assert.AreEqual("0", "".RemoveTrailingZeroes());
            Assert.AreEqual("0", ((string) null).RemoveTrailingZeroes());
            Assert.AreEqual("0", "0.".RemoveTrailingZeroes());
            Assert.AreEqual("0", ".0".RemoveTrailingZeroes());
            Assert.AreEqual("0", ".000".RemoveTrailingZeroes());
            Assert.AreEqual("0", "0.0000".RemoveTrailingZeroes());
            Assert.AreEqual("0.1", "0.1000000".RemoveTrailingZeroes());
            Assert.AreEqual("0.1324354", "0.1324354".RemoveTrailingZeroes());
            Assert.AreEqual("0.1324354", "0.13243540".RemoveTrailingZeroes());
            Assert.AreEqual("0.1324354", "0.132435400".RemoveTrailingZeroes());
            Assert.AreEqual("0.1324354", "0.1324354000".RemoveTrailingZeroes());
            Assert.AreEqual("abcde", "abcde".RemoveTrailingZeroes());
            Assert.AreEqual("abcde", "abcde000000".RemoveTrailingZeroes());
        }
    }
}