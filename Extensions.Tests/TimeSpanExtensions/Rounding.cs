using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.TimeSpanExtensions
{
    [TestClass]
    public class Rounding
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
    }
}