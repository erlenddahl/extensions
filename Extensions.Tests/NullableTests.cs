using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests
{
    [TestClass]
    public class NullableTests
    {
        [TestMethod]
        public void SafeTests()
        {
            double? d = null;
            Assert.AreEqual(3.14, d.Safe(3.14), 0.000005);
            Assert.AreEqual(0, d.Safe(), 0.000005);

            d = 7.55;
            Assert.AreEqual(7.55, d.Safe(3.14), 0.000005);

            d = 7.55;
            Assert.AreEqual(7.55, d.Safe(), 0.000005);
        }
    }
}
