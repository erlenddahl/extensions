using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class Replacing
    {

        [TestMethod]
        public void ReplaceFirstTests()
        {
            Assert.AreEqual("", "".ReplaceFirst("", ""));
            Assert.AreEqual("", "".ReplaceFirst("alfa", "bravo"));
            Assert.AreEqual("bravo", "alfa".ReplaceFirst("alfa", "bravo"));
            Assert.AreEqual("bravoalfa", "alfaalfa".ReplaceFirst("alfa", "bravo"));
            Assert.AreEqual("baa", "aaa".ReplaceFirst("a", "b"));
            Assert.AreEqual("bba", "aaa".ReplaceFirst("a", "b").ReplaceFirst("a", "b"));
        }

        [TestMethod]
        public void ReplaceSurroundingTests()
        {
            var i = "abcdabcdeafghijA".ReplaceSurrounding(5, "a", "c", "ABC");
            Assert.AreEqual("abcdABCdeafghijA", i);

            i = "abcdabcdeafghijA".ReplaceSurrounding(5, "c", "c", "");
            Assert.AreEqual("abdeafghijA", i);

            i = "abcdabcdeafghijA".ReplaceSurrounding(5, "EKKO", "c", "");
            Assert.AreEqual("abcdabcdeafghijA", i);

            i = "abcdabcdeafghijA".ReplaceSurrounding(5, "c", "PEKKO", "");
            Assert.AreEqual("abcdabcdeafghijA", i);
        }
    }
}