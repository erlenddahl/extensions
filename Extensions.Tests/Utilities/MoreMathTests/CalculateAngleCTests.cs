using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Utilities.MoreMathTests
{
    [TestClass]
    public class CalculateAngleCTests
    {
        [TestMethod]
        public void Basic()
        {
            Assert.AreEqual(37, MoreMath.CalculateAngleC(8, 11, 6.67), 0.1);
            Assert.AreEqual(60, MoreMath.CalculateAngleC(10, 10, 10), 0.1);
            Assert.AreEqual(62.2, MoreMath.CalculateAngleC(9, 5, 8), 0.1);
            Assert.AreEqual(180, MoreMath.CalculateAngleC(10, 10, 20), 0.1);
        }
    }
}
