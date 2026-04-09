using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.DateTimeExtensions;
using net.erlenddahl.Extensions.TimeSpanExtensions;
using net.erlenddahl.Extensions.Utilities;

namespace Extensions.Tests.Utilities
{
    [TestClass]
    public class Range
    {
        [TestMethod]
        public void EnumerateTests()
        {
            var r = new Range<int>(0, 10);
            var i = 0;

            var arr = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            foreach (var v in r.Enumerate(p => p + 1))
            {
                Assert.AreEqual(arr[i++], v);
            }

            Assert.AreEqual(11, i);
        }

        [TestMethod]
        public void OverlapDateTimeTests()
        {
            Func<int, int, DateTime> d = (h, m) => new DateTime(2015, 12, 16, h, m, 0);
            Func<int, DateTime> f = h => d(h, 0);

            var n = new DateTime(2015, 12, 16, 10, 0, 0).To(new DateTime(2015, 12, 16, 18, 0, 0));

            Assert.AreEqual(false, n.Overlaps(f(2), f(4)));
            Assert.AreEqual(false, n.Overlaps(f(2), f(10)));
            Assert.AreEqual(true, n.Overlaps(f(8), f(20)));
            Assert.AreEqual(true, n.Overlaps(f(2), f(11)));
            Assert.AreEqual(true, n.Overlaps(f(10), f(11)));
            Assert.AreEqual(true, n.Overlaps(f(11), f(12)));
            Assert.AreEqual(true, n.Overlaps(f(11), f(18)));
            Assert.AreEqual(true, n.Overlaps(d(17, 59), f(18)));
            Assert.AreEqual(false, n.Overlaps(f(18), f(19)));
            Assert.AreEqual(false, n.Overlaps(f(20), f(22)));
        }

        [TestMethod]
        public void OverlapTimeSpanTests()
        {
            Func<int, int, TimeSpan> d = (h, m) => new TimeSpan(h, m, 0);
            Func<int, TimeSpan> f = h => d(h, 0);

            var n = new TimeSpan(10, 0, 0).To(new TimeSpan(18, 0, 0));

            Assert.AreEqual(false, n.Overlaps(f(2), f(4)));
            Assert.AreEqual(false, n.Overlaps(f(2), f(10)));
            Assert.AreEqual(true, n.Overlaps(f(8), f(20)));
            Assert.AreEqual(true, n.Overlaps(f(2), f(11)));
            Assert.AreEqual(true, n.Overlaps(f(10), f(11)));
            Assert.AreEqual(true, n.Overlaps(f(11), f(12)));
            Assert.AreEqual(true, n.Overlaps(f(11), f(18)));
            Assert.AreEqual(true, n.Overlaps(d(17, 59), f(18)));
            Assert.AreEqual(false, n.Overlaps(f(18), f(19)));
            Assert.AreEqual(false, n.Overlaps(f(20), f(22)));
        }

        [TestMethod]
        public void MoreOverlapTests()
        {
            Func<int, int, DateTime> f2 = (h, m) => new DateTime(2015, 12, 16, h, m, 0);
            Func<int, DateTime> f = h => f2(h, 0);
            Action<int, int, int, int> t = (a, b, c, d) => Assert.AreEqual(f(a).To(f(b)).Overlaps(f(c).To(f(d))), f(c).To(f(d)).Overlaps(f(a).To(f(b))));

            t(2, 4, 5, 6);
            t(2, 4, 2, 4);
            t(2, 4, 4, 5);
            t(1, 2, 2, 4);
            t(5, 6, 2, 4);
        }
    }
}
