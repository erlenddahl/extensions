using System;
using Extensions.StringExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class ManipulationTests
    {
        [TestMethod]
        public void MaxLength_NotShortened_TooShort()
        {
            Assert.AreEqual("This is a long sentence.", "This is a long sentence.".MaxLength(100, "..."));
        }

        [TestMethod]
        public void MaxLength_NotShortened_ExactLength()
        {
            Assert.AreEqual("This is a long sentence.", "This is a long sentence.".MaxLength(24, "..."));
        }
        
        [TestMethod]
        public void MaxLengthTests_Shortened_WithoutSuffix()
        {
            Assert.AreEqual("This is a ", "This is a long sentence.".MaxLength(10));
        }

        [TestMethod]
        public void MaxLength_Shortened_WithSuffix()
        {
            Assert.AreEqual("This is a ...", "This is a long sentence.".MaxLength(10, "..."));
        }

        [TestMethod]
        public void Or_Null_Replaced()
        {
            Assert.AreEqual("replacement", ((string)null).Or("replacement"));
        }

        [TestMethod]
        public void Or_Empty_Replaced()
        {
            Assert.AreEqual("replacement", "".Or("replacement"));
        }

        [TestMethod]
        public void Or_Spaces_Replaced()
        {
            Assert.AreEqual("replacement", " ".Or("replacement"));
        }

        [TestMethod]
        public void Or_Newline_Replaced()
        {
            Assert.AreEqual("replacement", Environment.NewLine.Or("replacement"));
        }

        [TestMethod]
        public void Or_Mixed_Replaced()
        {
            Assert.AreEqual("replacement", ("\t\t" + Environment.NewLine + "    ").Or("replacement"));
        }

        [TestMethod]
        public void Or_Contents_NotReplaced()
        {
            Assert.AreEqual("original", "original".Or("replacement"));
        }
    }
}