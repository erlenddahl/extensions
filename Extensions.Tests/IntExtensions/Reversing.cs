using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.IntExtensions;

namespace Extensions.Tests.IntExtensions
{
    [TestClass]
    public class Reversing
    {
        [TestMethod]
        public void ReverseTests()
        {
            Assert.AreEqual(-1, 1.Reverse());
            Assert.AreEqual(1, (-1).Reverse());
            Assert.AreEqual(-1312, 1312.Reverse());
            Assert.AreEqual(11576, (-11576).Reverse());
            Assert.AreEqual(0, 0.Reverse());
            Assert.AreEqual(0, (-0).Reverse());
        }

        [TestMethod]
        public void ReverseIfTests()
        {
            Assert.AreEqual(-1, 1.ReverseIf(true));
            Assert.AreEqual(1, (-1).ReverseIf(true));
            Assert.AreEqual(-1312, 1312.ReverseIf(true));
            Assert.AreEqual(11576, (-11576).ReverseIf(true));
            Assert.AreEqual(0, 0.ReverseIf(true));
            Assert.AreEqual(0, (-0).ReverseIf(true));

            Assert.AreEqual(1, 1.ReverseIf(false));
            Assert.AreEqual(-1, (-1).ReverseIf(false));
            Assert.AreEqual(1312, 1312.ReverseIf(false));
            Assert.AreEqual(-11576, (-11576).ReverseIf(false));
            Assert.AreEqual(0, 0.ReverseIf(false));
            Assert.AreEqual(0, (-0).ReverseIf(false));
        }
    }
}