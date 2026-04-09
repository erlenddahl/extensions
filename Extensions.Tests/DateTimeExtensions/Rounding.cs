using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.DateTimeExtensions;

namespace Extensions.Tests.DateTimeExtensions
{
    [TestClass]
    public class Rounding
    {
        [TestMethod]
        public void RoundTests()
        {
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 00, 00), new DateTime(2017, 01, 27, 14, 45, 00).Round(new TimeSpan(1, 0, 0)));
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 00, 00), new DateTime(2017, 01, 27, 15, 15, 00).Round(new TimeSpan(1, 0, 0)));
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 00, 00), new DateTime(2017, 01, 27, 14, 30, 01).Round(new TimeSpan(1, 0, 0)));
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 00, 00), new DateTime(2017, 01, 27, 15, 29, 59).Round(new TimeSpan(1, 0, 0)));
            Assert.AreEqual(new DateTime(2017, 01, 27, 00, 00, 00), new DateTime(2017, 01, 27, 00, 00, 01).Round(new TimeSpan(1, 0, 0)));
            Assert.AreEqual(new DateTime(2017, 01, 28, 00, 00, 00), new DateTime(2017, 01, 27, 23, 48, 54).Round(new TimeSpan(1, 0, 0)));

            Assert.AreEqual(new DateTime(2017, 01, 27, 14, 45, 00), new DateTime(2017, 01, 27, 14, 45, 00).Round(new TimeSpan(0, 15, 0)));
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 15, 00), new DateTime(2017, 01, 27, 15, 14, 00).Round(new TimeSpan(0, 15, 0)));
            Assert.AreEqual(new DateTime(2017, 01, 27, 00, 00, 00), new DateTime(2017, 01, 27, 00, 07, 01).Round(new TimeSpan(0, 15, 0)));
            Assert.AreEqual(new DateTime(2017, 01, 27, 00, 30, 00), new DateTime(2017, 01, 27, 00, 29, 59).Round(new TimeSpan(0, 15, 0)));
        }

        [TestMethod]
        public void RoundUpTests()
        {
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 00, 00), new DateTime(2017, 01, 27, 14, 45, 00).Round(new TimeSpan(1, 0, 0), RoundingDirection.Up));
            Assert.AreEqual(new DateTime(2017, 01, 27, 16, 00, 00), new DateTime(2017, 01, 27, 15, 15, 00).Round(new TimeSpan(1, 0, 0), RoundingDirection.Up));
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 00, 00), new DateTime(2017, 01, 27, 14, 30, 01).Round(new TimeSpan(1, 0, 0), RoundingDirection.Up));
            Assert.AreEqual(new DateTime(2017, 01, 27, 16, 00, 00), new DateTime(2017, 01, 27, 15, 29, 59).Round(new TimeSpan(1, 0, 0), RoundingDirection.Up));
            Assert.AreEqual(new DateTime(2017, 01, 27, 01, 00, 00), new DateTime(2017, 01, 27, 00, 00, 01).Round(new TimeSpan(1, 0, 0), RoundingDirection.Up));
            Assert.AreEqual(new DateTime(2017, 01, 28, 00, 00, 00), new DateTime(2017, 01, 27, 23, 48, 54).Round(new TimeSpan(1, 0, 0), RoundingDirection.Up));

            Assert.AreEqual(new DateTime(2017, 01, 27, 14, 45, 00), new DateTime(2017, 01, 27, 14, 45, 00).Round(new TimeSpan(0, 15, 0), RoundingDirection.Up));
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 15, 00), new DateTime(2017, 01, 27, 15, 14, 00).Round(new TimeSpan(0, 15, 0), RoundingDirection.Up));
            Assert.AreEqual(new DateTime(2017, 01, 27, 00, 15, 00), new DateTime(2017, 01, 27, 00, 07, 01).Round(new TimeSpan(0, 15, 0), RoundingDirection.Up));
            Assert.AreEqual(new DateTime(2017, 01, 27, 00, 30, 00), new DateTime(2017, 01, 27, 00, 29, 59).Round(new TimeSpan(0, 15, 0), RoundingDirection.Up));
        }

        [TestMethod]
        public void RoundDownTests()
        {
            Assert.AreEqual(new DateTime(2017, 01, 27, 14, 00, 00), new DateTime(2017, 01, 27, 14, 45, 00).Round(new TimeSpan(1, 0, 0), RoundingDirection.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 00, 00), new DateTime(2017, 01, 27, 15, 15, 00).Round(new TimeSpan(1, 0, 0), RoundingDirection.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 14, 00, 00), new DateTime(2017, 01, 27, 14, 30, 01).Round(new TimeSpan(1, 0, 0), RoundingDirection.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 00, 00), new DateTime(2017, 01, 27, 15, 29, 59).Round(new TimeSpan(1, 0, 0), RoundingDirection.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 00, 00, 00), new DateTime(2017, 01, 27, 00, 00, 01).Round(new TimeSpan(1, 0, 0), RoundingDirection.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 23, 00, 00), new DateTime(2017, 01, 27, 23, 48, 54).Round(new TimeSpan(1, 0, 0), RoundingDirection.Down));

            Assert.AreEqual(new DateTime(2017, 01, 27, 14, 45, 00), new DateTime(2017, 01, 27, 14, 45, 00).Round(new TimeSpan(0, 15, 0), RoundingDirection.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 00, 00), new DateTime(2017, 01, 27, 15, 14, 00).Round(new TimeSpan(0, 15, 0), RoundingDirection.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 00, 00, 00), new DateTime(2017, 01, 27, 00, 07, 01).Round(new TimeSpan(0, 15, 0), RoundingDirection.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 00, 15, 00), new DateTime(2017, 01, 27, 00, 29, 59).Round(new TimeSpan(0, 15, 0), RoundingDirection.Down));
        }
    }
}
