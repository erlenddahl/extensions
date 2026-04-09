using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class Indices
    {

        [TestMethod]
        public void IndicesOfTest()
        {
            var indices = "abcdabcdeafghijA".IndicesOf("a").ToArray();
            Assert.AreEqual(3, indices.Length);
            Assert.AreEqual(0, indices[0]);
            Assert.AreEqual(4, indices[1]);
            Assert.AreEqual(9, indices[2]);

            indices = "".IndicesOf("a").ToArray();
            Assert.AreEqual(0, indices.Length);

            indices = "aaa".IndicesOf("a").ToArray();
            Assert.AreEqual(3, indices.Length);
            Assert.AreEqual(0, indices[0]);
            Assert.AreEqual(1, indices[1]);
            Assert.AreEqual(2, indices[2]);
        }

        [TestMethod]
        public void IndexOfBefore()
        {
            var i = "abcdabcdeafghijA".IndexOfBefore("a", 0);
            Assert.AreEqual(-1, i);

            i = "abcdabcdeafghijA".IndexOfBefore("a", 1);
            Assert.AreEqual(0, i);

            i = "abcdabcdeafghijA".IndexOfBefore("a", 2);
            Assert.AreEqual(0, i);

            i = "abcdabcdeafghijA".IndexOfBefore("a", 3);
            Assert.AreEqual(0, i);

            i = "abcdabcdeafghijA".IndexOfBefore("a", 4);
            Assert.AreEqual(0, i);

            i = "abcdabcdeafghijA".IndexOfBefore("a", 5);
            Assert.AreEqual(4, i);

            i = "abcdabcdeafghijA".IndexOfBefore("a", 6);
            Assert.AreEqual(4, i);
        }

        [TestMethod]
        public void IndexOfAfter()
        {
            var i = "abcdabcdeafghijA".IndexOfAfter("a", 0);
            Assert.AreEqual(4, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 1);
            Assert.AreEqual(4, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 2);
            Assert.AreEqual(4, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 3);
            Assert.AreEqual(4, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 4);
            Assert.AreEqual(9, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 5);
            Assert.AreEqual(9, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 6);
            Assert.AreEqual(9, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 9);
            Assert.AreEqual(-1, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 10);
            Assert.AreEqual(-1, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 11);
            Assert.AreEqual(-1, i);
        }
    }
}