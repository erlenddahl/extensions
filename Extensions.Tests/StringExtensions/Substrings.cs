using System.Linq;
using Extensions.StringExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class Substrings
    {
        [TestMethod]
        public void SubstringsOfLengthTests()
        {
            var abc = "abcdefghijklmnopqrstuvwxyz";
            var strs = abc.SubstringsOfLength(1, 1, 2, 3, 4, 5).ToArray();
            Assert.AreEqual("a", strs[0]);
            Assert.AreEqual("b", strs[1]);
            Assert.AreEqual("cd", strs[2]);
            Assert.AreEqual("efg", strs[3]);
            Assert.AreEqual("hijk", strs[4]);
            Assert.AreEqual("lmnop", strs[5]);

            strs = abc.SubstringsOfLength(10, 10).ToArray();
            Assert.AreEqual("abcdefghij", strs[0]);
            Assert.AreEqual("klmnopqrst", strs[1]);
        }
    }
}