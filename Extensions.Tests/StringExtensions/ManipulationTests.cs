using Extensions.StringExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class ManipulationTests
    {

        [TestMethod]
        public void MaxLengthTests()
        {
            Assert.AreEqual("This is a long sentence.", "This is a long sentence.".MaxLength(100, "..."));
            Assert.AreEqual("This is a long sentence.", "This is a long sentence.".MaxLength(24, "..."));
            Assert.AreEqual("This is a ", "This is a long sentence.".MaxLength(10));
            Assert.AreEqual("This is a ...", "This is a long sentence.".MaxLength(10, "..."));
        }
    }
}