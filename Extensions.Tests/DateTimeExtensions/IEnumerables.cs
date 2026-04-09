using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.DateTimeExtensions;

namespace Extensions.Tests.DateTimeExtensions
{
    [TestClass]
    public class IEnumerables
    {
        [TestMethod]
        public void UniqueWeeksTests()
        {
            var dt = new DateTime(2014, 5, 8, 9, 41, 51);
            var dt2 = new DateTime(2014, 5, 19, 16, 18, 12);
            var dates = dt.DatesTo(dt2).UniqueWeeks().ToList();
            Assert.AreEqual(3, dates.Count);
            Assert.AreEqual(19, dates[0].GetWeekNumber());
            Assert.AreEqual(20, dates[1].GetWeekNumber());
            Assert.AreEqual(21, dates[2].GetWeekNumber());
        }

        [TestMethod]
        public void UniqueMonthsTests()
        {
            var dt = new DateTime(2014, 5, 8, 9, 41, 51);
            var dt2 = new DateTime(2015, 5, 19, 16, 18, 12);
            var dates = dt.DatesTo(dt2).UniqueMonths().ToList();
            Assert.AreEqual(13, dates.Count);

            Assert.AreEqual(2014, dates[0].Year);
            Assert.AreEqual(2014, dates[1].Year);
            Assert.AreEqual(2014, dates[2].Year);
            Assert.AreEqual(2014, dates[3].Year);
            Assert.AreEqual(2014, dates[4].Year);
            Assert.AreEqual(2014, dates[5].Year);
            Assert.AreEqual(2014, dates[6].Year);
            Assert.AreEqual(2014, dates[7].Year);

            Assert.AreEqual(2015, dates[8].Year);
            Assert.AreEqual(2015, dates[9].Year);
            Assert.AreEqual(2015, dates[10].Year);
            Assert.AreEqual(2015, dates[11].Year);
            Assert.AreEqual(2015, dates[12].Year);

            Assert.AreEqual(5, dates[0].Month);
            Assert.AreEqual(6, dates[1].Month);
            Assert.AreEqual(7, dates[2].Month);
            Assert.AreEqual(8, dates[3].Month);
            Assert.AreEqual(9, dates[4].Month);
            Assert.AreEqual(10, dates[5].Month);
            Assert.AreEqual(11, dates[6].Month);
            Assert.AreEqual(12, dates[7].Month);

            Assert.AreEqual(1, dates[8].Month);
            Assert.AreEqual(2, dates[9].Month);
            Assert.AreEqual(3, dates[10].Month);
            Assert.AreEqual(4, dates[11].Month);
            Assert.AreEqual(5, dates[12].Month);
        }

        [TestMethod]
        public void UniqueYearsTests()
        {
            var dt = new DateTime(2014, 5, 8, 9, 41, 51);
            var dt2 = new DateTime(2015, 5, 19, 16, 18, 12);
            var dates = dt.DatesTo(dt2).UniqueYears().ToList();
            Assert.AreEqual(2, dates.Count);

            Assert.AreEqual(2014, dates[0].Year);
            Assert.AreEqual(2015, dates[1].Year);
        }

        [TestMethod]
        public void DatesToTests()
        {
            var dt = new DateTime(2014, 5, 8, 9, 41, 51);
            var dt2 = new DateTime(2014, 5, 19, 16, 18, 12);
            var dates = dt.DatesTo(dt2).ToList();
            Assert.AreEqual(12, dates.Count());
            Assert.AreEqual(dt.Date, dates.First().Date);
            for (var i = 0; i < 11; i++)
                Assert.AreEqual(dt.Date.AddDays(i), dates.First().Date.AddDays(i));
            Assert.AreEqual(dt2.Date, dates.Last().Date);

            dates = dt2.DatesTo(dt).ToList();
            Assert.AreEqual(0, dates.Count());
        }
    }
}
