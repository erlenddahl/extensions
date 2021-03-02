using Extensions.IntExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IntExtensions
{
    [TestClass]
    public class Excel
    {
        [TestMethod]
        public void ToExcelColumnTests()
        {
            Assert.AreEqual("A", 0.ToExcelColumn());
            Assert.AreEqual("B", 1.ToExcelColumn());
            Assert.AreEqual("C", 2.ToExcelColumn());
            Assert.AreEqual("Z", 25.ToExcelColumn());
            Assert.AreEqual("AA", 26.ToExcelColumn());
            Assert.AreEqual("AB", 27.ToExcelColumn());
            Assert.AreEqual("XFE", 16384.ToExcelColumn());
        }
    }
}