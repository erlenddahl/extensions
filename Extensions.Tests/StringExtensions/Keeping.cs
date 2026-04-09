using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class Keeping
    {
        [TestMethod]
        public void KeepBetweenTests()
        {
            try
            {
                "123=?erlend?=321".KeepBetween("µ", "<cat>");
                Assert.Fail();
            }
            catch (InvalidDataException ide)
            {
            }

            try
            {
                "123=?erlend?=321".KeepBetween("?=", "=?");
                Assert.Fail();
            }
            catch (InvalidDataException ide)
            {
            }

            try
            {
                "12µ3=?erlend?=321".KeepBetween("µ", "<cat>");
                Assert.Fail();
            }
            catch (InvalidDataException ide)
            {
            }

            try
            {
                "123=?erlend?=<cat>321".KeepBetween("µ", "<cat>");
                Assert.Fail();
            }
            catch (InvalidDataException ide)
            {
            }

            Assert.AreEqual("erlend", "123=?erlend?=321".KeepBetween("=?", "?="));
            Assert.AreEqual("=?erlend?=", "123=?erlend?=321".KeepBetween("=?", "?=", false));
            Assert.AreEqual("erlend", "=?erlend?=".KeepBetween("=?", "?="));
            Assert.AreEqual("=?erlend?=", "=?erlend?=".KeepBetween("=?", "?=", false));
            Assert.AreEqual("erlend", "?=xx?=xsa=?erlend?=da?=ffas?=".KeepBetween("=?", "?="));
            Assert.AreEqual("=?erlend?=", "?=xx?=xsa=?erlend?=da?=ffas?=".KeepBetween("=?", "?=", false));
        }

        [TestMethod]
        public void KeepBeforeTests()
        {
            Assert.AreEqual("", "".KeepBefore("<cat"));
            Assert.AreEqual("Someting cat hello", "Someting cat hello".KeepBefore("<cat"));
            Assert.AreEqual("Someting ", "Someting <cat> hello".KeepBefore("<cat"));
            Assert.AreEqual("", "Someting <cat> hello".KeepBefore("So"));
        }

        [TestMethod]
        public void KeepAfterTests()
        {
            Assert.AreEqual("", "".KeepAfter("<cat"));
            Assert.AreEqual("Someting cat hello", "Someting cat hello".KeepAfter("<cat"));
            Assert.AreEqual("<cat> hello", "Someting <cat> hello".KeepAfter("<cat>"));
            Assert.AreEqual("Someting <cat> hello", "Someting <cat> hello".KeepAfter("So"));

            Assert.AreEqual("Someting cat hello", "Someting cat hello".KeepAfter("<cat", false));
            Assert.AreEqual(" hello", "Someting <cat> hello".KeepAfter("<cat>", false));
            Assert.AreEqual("meting <cat> hello", "Someting <cat> hello".KeepAfter("So", false));
        }
    }
}
