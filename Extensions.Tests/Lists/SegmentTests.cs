using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions.Lists;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Lists
{
    [TestClass]
    public class SegmentTests
    {
        [TestMethod]
        public void SimpleTest()
        {
            var list = new[] {1, 2, 3, 4, 5, 6, 7, 8};
            var segments = list.Segment(2, p => p[0] + "-" + p[1]).ToList();

            CollectionAssert.AreEqual(new[] {"1-2", "3-4", "5-6", "7-8"}, segments);
        }
    }
}
