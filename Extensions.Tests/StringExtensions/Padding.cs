using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class Padding
    {

        [TestMethod]
        public void PadBeforeTests()
        {
            Assert.AreEqual("  ", "".PadBefore(2));
            Assert.AreEqual("    Erlend", "Erlend".PadBefore(10));
            Assert.AreEqual("!!!!!Heppa!!!!!", "Heppa!!!!!".PadBefore(15, '!'));
        }

        [TestMethod]
        public void PadAfterTests()
        {
            Assert.AreEqual("", "".PadAfter(0));
            Assert.AreEqual("  ", "".PadAfter(2));
            Assert.AreEqual("Erlend    ", "Erlend".PadAfter(10));
            Assert.AreEqual("Erlend Dahl", "Erlend Dahl".PadAfter(2));
            Assert.AreEqual("Heppa!!!!!", "Heppa".PadAfter(10, '!'));
        }

        [TestMethod]
        public void PadCenterTests()
        {
            Assert.AreEqual("", "".PadCenter(0));
            Assert.AreEqual(" ", "".PadCenter(1));
            Assert.AreEqual("  ", "".PadCenter(2));

            Assert.AreEqual("A", "A".PadCenter(0));
            Assert.AreEqual("A", "A".PadCenter(1));
            Assert.AreEqual("A ", "A".PadCenter(2));
            Assert.AreEqual(" A ", "A".PadCenter(3));

            Assert.AreEqual("A", "A".PadCenter(0,'='));
            Assert.AreEqual("===ABBA===", "ABBA".PadCenter(10, '='));
            Assert.AreEqual("=====ABBA======", "ABBA".PadCenter(15, '='));
        }
    }
}