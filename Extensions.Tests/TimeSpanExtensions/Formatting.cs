using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.TimeSpanExtensions
{
    [TestClass]
    public class Formatting
    {
        [TestMethod]
        public void ToShortPrettyFormat()
        {
            Assert.AreEqual("0 s", new TimeSpan(0, 0, 0).ToShortPrettyFormat());
            Assert.AreEqual("1 s", new TimeSpan(0, 0, 1).ToShortPrettyFormat());
            Assert.AreEqual("45 s", new TimeSpan(0, 0, 45).ToShortPrettyFormat());
            Assert.AreEqual("01:00 m", new TimeSpan(0, 1, 0).ToShortPrettyFormat());
            Assert.AreEqual("01:35 m", new TimeSpan(0, 1, 35).ToShortPrettyFormat());
            Assert.AreEqual("01:00:00", new TimeSpan(1, 0, 0).ToShortPrettyFormat());
            Assert.AreEqual("01:17:35", new TimeSpan(1, 17, 35).ToShortPrettyFormat());
            Assert.AreEqual("14:45:00", new TimeSpan(14, 45, 0).ToShortPrettyFormat());
            Assert.AreEqual("15:15:00", new TimeSpan(15, 15, 0).ToShortPrettyFormat());
            Assert.AreEqual("1d 00:15:11", new TimeSpan(24, 15, 11).ToShortPrettyFormat());
            Assert.AreEqual("1d 03:15:11", new TimeSpan(27, 15, 11).ToShortPrettyFormat());
            Assert.AreEqual("2d 06:15:11", new TimeSpan(54, 15, 11).ToShortPrettyFormat());
        }
    }
}
