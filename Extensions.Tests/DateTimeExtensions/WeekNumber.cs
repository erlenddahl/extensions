using System;
using System.Collections.Generic;
using System.Text;
using Extensions.DateTimeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.DateTimeExtensions
{
    [TestClass]
    public class WeekNumber
    {

        [TestMethod]
        public void GetWeekNumber()
        {
            for (var i = 22; i <= 28; i++)
                Assert.AreEqual(52, new DateTime(2014, 12, i).GetWeekNumber());
            for (var i = 29; i <= 31; i++)
                Assert.AreEqual(1, new DateTime(2014, 12, i).GetWeekNumber());

            for (var i = 1; i <= 4; i++)
                Assert.AreEqual(1, new DateTime(2015, 1, i).GetWeekNumber());
            for (var i = 2; i <= 8; i++)
                Assert.AreEqual(6, new DateTime(2015, 2, i).GetWeekNumber());
            for (var i = 15; i <= 21; i++)
                Assert.AreEqual(25, new DateTime(2015, 6, i).GetWeekNumber());
            for (var i = 28; i <= 31; i++)
                Assert.AreEqual(53, new DateTime(2015, 12, i).GetWeekNumber());

            for (var i = 1; i <= 3; i++)
                Assert.AreEqual(53, new DateTime(2016, 1, i).GetWeekNumber());
            for (var i = 4; i <= 10; i++)
                Assert.AreEqual(1, new DateTime(2016, 1, i).GetWeekNumber());
        }
    }
}