using System;
using System.Collections.Generic;
using System.Text;
using Extensions.DateTimeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.DateTimeExtensions
{
    [TestClass]
   public class StartOf
    {
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
    }
}
