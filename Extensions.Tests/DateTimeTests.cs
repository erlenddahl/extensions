using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests
{
    [TestClass]
    public class DateTimeTests
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
            for(var i = 0; i < 11; i++)
                Assert.AreEqual(dt.Date.AddDays(i), dates.First().Date.AddDays(i));
            Assert.AreEqual(dt2.Date, dates.Last().Date);

            dates = dt2.DatesTo(dt).ToList();
            Assert.AreEqual(0, dates.Count());
        }

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
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 00, 00), new DateTime(2017, 01, 27, 14, 45, 00).Round(new TimeSpan(1, 0, 0), Rounding.Up));
            Assert.AreEqual(new DateTime(2017, 01, 27, 16, 00, 00), new DateTime(2017, 01, 27, 15, 15, 00).Round(new TimeSpan(1, 0, 0), Rounding.Up));
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 00, 00), new DateTime(2017, 01, 27, 14, 30, 01).Round(new TimeSpan(1, 0, 0), Rounding.Up));
            Assert.AreEqual(new DateTime(2017, 01, 27, 16, 00, 00), new DateTime(2017, 01, 27, 15, 29, 59).Round(new TimeSpan(1, 0, 0), Rounding.Up));
            Assert.AreEqual(new DateTime(2017, 01, 27, 01, 00, 00), new DateTime(2017, 01, 27, 00, 00, 01).Round(new TimeSpan(1, 0, 0), Rounding.Up));
            Assert.AreEqual(new DateTime(2017, 01, 28, 00, 00, 00), new DateTime(2017, 01, 27, 23, 48, 54).Round(new TimeSpan(1, 0, 0), Rounding.Up));

            Assert.AreEqual(new DateTime(2017, 01, 27, 14, 45, 00), new DateTime(2017, 01, 27, 14, 45, 00).Round(new TimeSpan(0, 15, 0), Rounding.Up));
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 15, 00), new DateTime(2017, 01, 27, 15, 14, 00).Round(new TimeSpan(0, 15, 0), Rounding.Up));
            Assert.AreEqual(new DateTime(2017, 01, 27, 00, 15, 00), new DateTime(2017, 01, 27, 00, 07, 01).Round(new TimeSpan(0, 15, 0), Rounding.Up));
            Assert.AreEqual(new DateTime(2017, 01, 27, 00, 30, 00), new DateTime(2017, 01, 27, 00, 29, 59).Round(new TimeSpan(0, 15, 0), Rounding.Up));
        }

        [TestMethod]
        public void RoundDownTests()
        {
            Assert.AreEqual(new DateTime(2017, 01, 27, 14, 00, 00), new DateTime(2017, 01, 27, 14, 45, 00).Round(new TimeSpan(1, 0, 0), Rounding.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 00, 00), new DateTime(2017, 01, 27, 15, 15, 00).Round(new TimeSpan(1, 0, 0), Rounding.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 14, 00, 00), new DateTime(2017, 01, 27, 14, 30, 01).Round(new TimeSpan(1, 0, 0), Rounding.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 00, 00), new DateTime(2017, 01, 27, 15, 29, 59).Round(new TimeSpan(1, 0, 0), Rounding.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 00, 00, 00), new DateTime(2017, 01, 27, 00, 00, 01).Round(new TimeSpan(1, 0, 0), Rounding.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 23, 00, 00), new DateTime(2017, 01, 27, 23, 48, 54).Round(new TimeSpan(1, 0, 0), Rounding.Down));

            Assert.AreEqual(new DateTime(2017, 01, 27, 14, 45, 00), new DateTime(2017, 01, 27, 14, 45, 00).Round(new TimeSpan(0, 15, 0), Rounding.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 15, 00, 00), new DateTime(2017, 01, 27, 15, 14, 00).Round(new TimeSpan(0, 15, 0), Rounding.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 00, 00, 00), new DateTime(2017, 01, 27, 00, 07, 01).Round(new TimeSpan(0, 15, 0), Rounding.Down));
            Assert.AreEqual(new DateTime(2017, 01, 27, 00, 15, 00), new DateTime(2017, 01, 27, 00, 29, 59).Round(new TimeSpan(0, 15, 0), Rounding.Down));
        }

        [TestMethod]
        public void StartOfWeekTests()
        {
            Assert.AreEqual(new DateTime(2017, 02, 20, 00, 00, 00), new DateTime(2017, 02, 26, 00, 00, 00).StartOfWeek());

            Assert.AreEqual(new DateTime(2017, 02, 27, 00, 00, 00), new DateTime(2017, 02, 27, 00, 00, 00).StartOfWeek());
            Assert.AreEqual(new DateTime(2017, 02, 27, 00, 00, 00), new DateTime(2017, 02, 28, 00, 00, 00).StartOfWeek());
            Assert.AreEqual(new DateTime(2017, 02, 27, 00, 00, 00), new DateTime(2017, 03, 01, 00, 00, 00).StartOfWeek());
            Assert.AreEqual(new DateTime(2017, 02, 27, 00, 00, 00), new DateTime(2017, 03, 02, 00, 00, 00).StartOfWeek());
            Assert.AreEqual(new DateTime(2017, 02, 27, 00, 00, 00), new DateTime(2017, 03, 03, 00, 00, 00).StartOfWeek());
            Assert.AreEqual(new DateTime(2017, 02, 27, 00, 00, 00), new DateTime(2017, 03, 04, 00, 00, 00).StartOfWeek());
            Assert.AreEqual(new DateTime(2017, 02, 27, 00, 00, 00), new DateTime(2017, 03, 05, 00, 00, 00).StartOfWeek());

            Assert.AreEqual(new DateTime(2017, 03, 06, 00, 00, 00), new DateTime(2017, 03, 06, 00, 00, 00).StartOfWeek());


            Assert.AreEqual(new DateTime(2017, 03, 06, 00, 00, 00), new DateTime(2017, 03, 06, 14, 14, 14).StartOfWeek());
        }

        [TestMethod]
        public void StartOfMonthTests()
        {
            Assert.AreEqual(new DateTime(2017, 02, 01, 00, 00, 00), new DateTime(2017, 02, 28, 00, 00, 00).StartOfMonth());

            foreach (var dt in new DateTime(2017, 03, 01).To(new DateTime(2017, 03, 31)).Enumerate(p => p.AddDays(1)))
                Assert.AreEqual(new DateTime(2017, 03, 01), dt.StartOfMonth());

            Assert.AreEqual(new DateTime(2017, 04, 01, 00, 00, 00), new DateTime(2017, 04, 06, 00, 00, 00).StartOfMonth());

            Assert.AreEqual(new DateTime(2017, 04, 01, 00, 00, 00), new DateTime(2017, 04, 06, 14, 14, 14).StartOfMonth());
        }

        [TestMethod]
        public void EndOfWeekTests()
        {
            Assert.AreEqual(new DateTime(2017, 02, 26, 00, 00, 00), new DateTime(2017, 02, 23, 00, 00, 00).EndOfWeek());

            Assert.AreEqual(new DateTime(2017, 03, 05, 00, 00, 00), new DateTime(2017, 02, 27, 00, 00, 00).EndOfWeek());
            Assert.AreEqual(new DateTime(2017, 03, 05, 00, 00, 00), new DateTime(2017, 02, 28, 00, 00, 00).EndOfWeek());
            Assert.AreEqual(new DateTime(2017, 03, 05, 00, 00, 00), new DateTime(2017, 03, 01, 00, 00, 00).EndOfWeek());
            Assert.AreEqual(new DateTime(2017, 03, 05, 00, 00, 00), new DateTime(2017, 03, 02, 00, 00, 00).EndOfWeek());
            Assert.AreEqual(new DateTime(2017, 03, 05, 00, 00, 00), new DateTime(2017, 03, 03, 00, 00, 00).EndOfWeek());
            Assert.AreEqual(new DateTime(2017, 03, 05, 00, 00, 00), new DateTime(2017, 03, 04, 00, 00, 00).EndOfWeek());
            Assert.AreEqual(new DateTime(2017, 03, 05, 00, 00, 00), new DateTime(2017, 03, 05, 00, 00, 00).EndOfWeek());

            Assert.AreEqual(new DateTime(2017, 03, 12, 00, 00, 00), new DateTime(2017, 03, 06, 00, 00, 00).EndOfWeek());


            Assert.AreEqual(new DateTime(2017, 03, 05, 00, 00, 00), new DateTime(2017, 03, 03, 14, 14, 14).EndOfWeek());
        }

        [TestMethod]
        public void EndOfMonthTests()
        {
            Assert.AreEqual(new DateTime(2017, 02, 28, 00, 00, 00), new DateTime(2017, 02, 23, 00, 00, 00).EndOfMonth());
            Assert.AreEqual(new DateTime(2017, 02, 28, 00, 00, 00), new DateTime(2017, 02, 28, 00, 00, 00).EndOfMonth());

            foreach (var dt in new DateTime(2017, 03, 01).To(new DateTime(2017, 03, 31)).Enumerate(p => p.AddDays(1)))
                Assert.AreEqual(new DateTime(2017, 03, 31), dt.EndOfMonth());

            Assert.AreEqual(new DateTime(2017, 04, 30, 00, 00, 00), new DateTime(2017, 04, 06, 00, 00, 00).EndOfMonth());

            Assert.AreEqual(new DateTime(2017, 04, 30, 00, 00, 00), new DateTime(2017, 04, 06, 14, 14, 14).EndOfMonth());
        }
    }
}
