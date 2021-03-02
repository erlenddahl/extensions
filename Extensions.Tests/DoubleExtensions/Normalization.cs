using System;
using Extensions.DoubleExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.DoubleExtensions
{
    [TestClass]
    public class Normalization
    {
        [TestMethod]
        public void NormalizeTests()
        {
            try
            {
                Assert.AreEqual(0, 0.5.Normalize(0, 0, 0, 0));
                Assert.Fail();
            }catch(ArgumentOutOfRangeException aoore)
            {

            }

            Assert.AreEqual(0.5, 0.5.Normalize(0, 1, 0, 1));

            for (int i = 0; i < 100; i++)
                Assert.AreEqual(i, (i / 100d).Normalize(0, 1, 0, 100), 0.00005);
        }
    }
}