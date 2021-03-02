using Extensions.DoubleExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.DoubleExtensions
{
    [TestClass]
    public class Reversing
    {

        [TestMethod]
        public void ReverseTests()
        {
            Assert.AreEqual(-1312.15, 1312.15.Reverse());
            Assert.AreEqual(11576.62, (-11576.62).Reverse());
        }

        [TestMethod]
        public void ReverseIfTests()
        {
            Assert.AreEqual(-1312.15, 1312.15.ReverseIf(true));
            Assert.AreEqual(11576.62, (-11576.62).ReverseIf(true));

            Assert.AreEqual(1312.15, 1312.15.ReverseIf(false));
            Assert.AreEqual(-11576.62, (-11576.62).ReverseIf(false));
        }
    }
}