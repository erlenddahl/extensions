using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.DateTimeExtensions;
using net.erlenddahl.Extensions.Utilities;

namespace Extensions.Tests.DateTimeExtensions
{
    [TestClass]
    public class Ranges
    {
        [TestMethod]
        public void ToTests()
        {
            var a = new DateTime(2015, 11, 16, 12, 15, 51);
            var b = new DateTime(2016, 12, 11, 16, 11, 19);
            var dr = a.To(b);
            Assert.AreEqual(a, dr.Start);
            Assert.AreEqual(b, dr.End);
        }

        [TestMethod]
        public void DateTimeRangeOverlapTests()
        {
            Func<int, int, int, int, Range<DateTime>> range = (a, b, c, d) => new DateTime(2016, 11, 28, a, b, 0).To(new DateTime(2016, 11, 28, c, d, 0));
            var r = range(19, 20, 19, 40);

            Assert.AreEqual(0, r.Overlap(range(18, 0, 18, 0)).TotalMinutes);
            Assert.AreEqual(0, r.Overlap(range(18, 0, 18, 20)).TotalMinutes);
            Assert.AreEqual(0, r.Overlap(range(20, 0, 20, 0)).TotalMinutes);
            Assert.AreEqual(0, r.Overlap(range(19, 10, 19, 20)).TotalMinutes);
            Assert.AreEqual(0, r.Overlap(range(19, 40, 19, 50)).TotalMinutes);
            Assert.AreEqual(10, r.Overlap(range(14, 0, 19, 30)).TotalMinutes);
            Assert.AreEqual(20, r.Overlap(range(19, 20, 19, 40)).TotalMinutes);
            Assert.AreEqual(20, r.Overlap(range(19, 10, 19, 50)).TotalMinutes);
            Assert.AreEqual(10, r.Overlap(range(19, 30, 19, 40)).TotalMinutes);
            Assert.AreEqual(10, r.Overlap(range(19, 30, 19, 50)).TotalMinutes);
            Assert.AreEqual(10, r.Overlap(range(19, 25, 19, 35)).TotalMinutes);
        }
    }
}
