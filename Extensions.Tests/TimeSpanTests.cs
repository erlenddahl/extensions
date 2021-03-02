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

    [TestClass]
    public class CsvReaderTests
    {
        [TestMethod]
        public void QuoteTests()
        {
            var line = "615702,956126,1900094,1948109,0.6,' ',' ',0,0,0,596,0,' ',80,1#2,1,0,'E18 Dørdal-Tvedestrand, arm v Risørkryss',0,0,0,3,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0.45,0,0,99999,99999,1000,80,80,0.45,0.45,1,6,0.45,0.45,1.33068,0.45,0.00133,80,0.79841,0.00998,0.79949,0.12275,0.06442,0.08661,0.18292,0.07449,0,0,0,0,0,0.01966,0.00001,0.04076,0.00202,0.03148,0.01527,0,23.72534,15.59358,0.33226,0.23095,0.34765,0.72914,0.16342,0,0,6.32833,0,0,0.25824,0.00002,0.14569,0.00858,0.13087,0.03172,0,0.45,0.45,1.33068,0.405,0.405,0.405,0.405,0.9,1";
            var reader = new CsvReader(',', '\'', false);
            var row = reader.ReadString(line).First();
            Assert.AreEqual(125, row.Raw.Length);
        }
    }
}
