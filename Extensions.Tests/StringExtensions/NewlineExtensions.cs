using System;
using Extensions.StringExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class NewlineExtensions
    {

        [TestMethod]
        public void BrTests()
        {
            Assert.AreEqual(Environment.NewLine, "".Br());
            Assert.AreEqual(Environment.NewLine, "".Br(1));
            Assert.AreEqual(Environment.NewLine + Environment.NewLine, "".Br(2));
            Assert.AreEqual("hei" + Environment.NewLine, "hei".Br());
            Assert.AreEqual("hei" + Environment.NewLine, "hei".Br(1));
            Assert.AreEqual("hei" + Environment.NewLine + Environment.NewLine, "hei".Br(2));
        }

        [TestMethod]
        public void RemoveEmptyLinesTests()
        {
            Assert.AreEqual("", "".RemoveEmptyLines());
            Assert.AreEqual("a", "a".RemoveEmptyLines());
            Assert.AreEqual("abcµ*", "abcµ*".RemoveEmptyLines());
            Assert.AreEqual("a", "a".Br().RemoveEmptyLines());
            Assert.AreEqual("a".Br() + "b", ("a".Br(3) + Environment.NewLine + "\t   " + Environment.NewLine + "b").RemoveEmptyLines());

            var answer = "Vind- og friksjonsvarsling Dovre             4.0".Br() + "All                                         44.0";
            var test = @"Vind- og friksjonsvarsling Dovre             4.0

All                                         44.0".RemoveEmptyLines();
            Assert.AreEqual(answer, test);
        }
    }
}