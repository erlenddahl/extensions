using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.DateTimeExtensions
{
    [TestClass]
    public class EndOf
    {
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
