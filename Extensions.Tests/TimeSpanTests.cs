using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests
{
    [TestClass]
    public class TimeSpanTests
    {
        [TestMethod]
        public void RoundTests()
        {
            Assert.AreEqual(new TimeSpan(15, 0, 0), new TimeSpan(14, 45, 0).Round(new TimeSpan(1, 0, 0)));
            Assert.AreEqual(new TimeSpan(15, 0, 0), new TimeSpan(15, 15, 0).Round(new TimeSpan(1, 0, 0)));
            Assert.AreEqual(new TimeSpan(15, 0, 0), new TimeSpan(14, 30, 1).Round(new TimeSpan(1, 0, 0)));
            Assert.AreEqual(new TimeSpan(15, 0, 0), new TimeSpan(15, 29, 59).Round(new TimeSpan(1, 0, 0)));

            Assert.AreEqual(new TimeSpan(14, 45, 0), new TimeSpan(14, 45, 0).Round(new TimeSpan(0, 15, 0)));
            Assert.AreEqual(new TimeSpan(15, 15, 0), new TimeSpan(15, 14, 0).Round(new TimeSpan(0, 15, 0)));
            Assert.AreEqual(new TimeSpan(0, 0, 0), new TimeSpan(0, 7, 1).Round(new TimeSpan(0, 15, 0)));
            Assert.AreEqual(new TimeSpan(0, 30, 0), new TimeSpan(0, 29, 59).Round(new TimeSpan(0, 15, 0)));
        }

        [TestMethod]
        public void RoundTestsMinute()
        {
            Assert.AreEqual(new TimeSpan(15, 0, 0), new TimeSpan(14, 45, 0).RoundMinutes(60));
            Assert.AreEqual(new TimeSpan(15, 0, 0), new TimeSpan(15, 15, 0).RoundMinutes(60));
            Assert.AreEqual(new TimeSpan(15, 0, 0), new TimeSpan(14, 30, 1).RoundMinutes(60));
            Assert.AreEqual(new TimeSpan(15, 0, 0), new TimeSpan(15, 29, 59).RoundMinutes(60));

            Assert.AreEqual(new TimeSpan(14, 45, 0), new TimeSpan(14, 45, 0).RoundMinutes(15));
            Assert.AreEqual(new TimeSpan(15, 15, 0), new TimeSpan(15, 14, 0).RoundMinutes(15));
            Assert.AreEqual(new TimeSpan(0, 0, 0), new TimeSpan(0, 7, 1).RoundMinutes(15));
            Assert.AreEqual(new TimeSpan(0, 30, 0), new TimeSpan(0, 29, 59).RoundMinutes(15));
        }

        [TestMethod]
        public void ToShortPrettyFormat()
        {
            Assert.AreEqual("0", new TimeSpan(0, 0, 0).ToShortPrettyFormat());
            Assert.AreEqual("1", new TimeSpan(0, 0, 1).ToShortPrettyFormat());
            Assert.AreEqual("45", new TimeSpan(0,0,45).ToShortPrettyFormat());
            Assert.AreEqual("01:00", new TimeSpan(0, 1, 0).ToShortPrettyFormat());
            Assert.AreEqual("01:35", new TimeSpan(0, 1, 35).ToShortPrettyFormat());
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
