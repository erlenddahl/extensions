using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests
{
    [TestClass]
    public class NumericTests
    {
        [TestMethod]
        public void ToTests()
        {
            var p = 1.To(1).ToList();
            Assert.AreEqual(1, p.Count);
            Assert.AreEqual(1, p[0]);

            p = 1.To(1, 10).ToList();
            Assert.AreEqual(1, p.Count);
            Assert.AreEqual(1, p[0]);

            p = 10.To(100, 10).ToList();
            Assert.AreEqual(10, p.Count);
            for (int i = 1; i < 11; i++)
                Assert.AreEqual(i * 10, p[i - 1]);

            p = 0.To(10).ToList();
            Assert.AreEqual(11, p.Count);
            for (int i = 0; i < 11; i++)
                Assert.AreEqual(i, p[i]);

            p = 0.To(-10, -1).ToList();
            Assert.AreEqual(11, p.Count);
            for (int i = 0; i < 11; i++)
                Assert.AreEqual(-i, p[i]);
        }

        [TestMethod]
        public void RestrictTests()
        {
            //Double
            Assert.AreEqual(12.54, 12.54.Restrict(0, 100));
            Assert.AreEqual(12.54, 12.54.Restrict(12.54, 100));
            Assert.AreEqual(12.54, 12.54.Restrict(0, 12.54));
            Assert.AreEqual(12.54, 12.54.Restrict(12.54, 12.54));
            Assert.AreEqual(0, 12.54.Restrict(-100, 0));
            Assert.AreEqual(0, 15331.0.Restrict(-100, 0));
            Assert.AreEqual(-100, (-15331.0).Restrict(-100, 0));

            //Int
            Assert.AreEqual(12, 12.Restrict(0, 100));
            Assert.AreEqual(12, 12.Restrict(12, 100));
            Assert.AreEqual(12, 12.Restrict(0, 12));
            Assert.AreEqual(12, 12.Restrict(12, 12));
            Assert.AreEqual(0, 12.Restrict(-100, 0));
            Assert.AreEqual(0, 15331.Restrict(-100, 0));
            Assert.AreEqual(-100, (-15331).Restrict(-100, 0));
        }

        [TestMethod]
        public void ReverseTests()
        {
            Assert.AreEqual(-1, 1.Reverse());
            Assert.AreEqual(1, (-1).Reverse());
            Assert.AreEqual(-1312, 1312.Reverse());
            Assert.AreEqual(11576, (-11576).Reverse());
            Assert.AreEqual(-1312.15, 1312.15.Reverse());
            Assert.AreEqual(11576.62, (-11576.62).Reverse());
            Assert.AreEqual(0, 0.Reverse());
            Assert.AreEqual(0, (-0).Reverse());
        }

        [TestMethod]
        public void ReverseIfTests()
        {
            Assert.AreEqual(-1, 1.ReverseIf(true));
            Assert.AreEqual(1, (-1).ReverseIf(true));
            Assert.AreEqual(-1312, 1312.ReverseIf(true));
            Assert.AreEqual(11576, (-11576).ReverseIf(true));
            Assert.AreEqual(-1312.15, 1312.15.ReverseIf(true));
            Assert.AreEqual(11576.62, (-11576.62).ReverseIf(true));
            Assert.AreEqual(0, 0.ReverseIf(true));
            Assert.AreEqual(0, (-0).ReverseIf(true));

            Assert.AreEqual(1, 1.ReverseIf(false));
            Assert.AreEqual(-1, (-1).ReverseIf(false));
            Assert.AreEqual(1312, 1312.ReverseIf(false));
            Assert.AreEqual(-11576, (-11576).ReverseIf(false));
            Assert.AreEqual(1312.15, 1312.15.ReverseIf(false));
            Assert.AreEqual(-11576.62, (-11576.62).ReverseIf(false));
            Assert.AreEqual(0, 0.ReverseIf(false));
            Assert.AreEqual(0, (-0).ReverseIf(false));
        }

        [TestMethod]
        public void NormalizeTests()
        {
            Assert.AreEqual(0, 0.Normalize(0, 0, 0, 0));
            Assert.AreEqual(1, 1.Normalize(1, 1, 1, 1));

            try
            {
                Assert.AreEqual(0, 0.5.Normalize(0, 0, 0, 0));
                Assert.Fail();
            }catch(ArgumentOutOfRangeException aoore)
            {

            }

            Assert.AreEqual(1, 1.Normalize(0, 1, 0, 1));
            Assert.AreEqual(0, 0.Normalize(0, 1, 0, 1));
            Assert.AreEqual(0.5, 0.5.Normalize(0, 1, 0, 1));

            for (int i = 0; i < 100; i++)
                Assert.AreEqual(i, (i / 100d).Normalize(0, 1, 0, 100), 0.00005);
        }

        [TestMethod]
        public void ToExcelColumnTests()
        {
            Assert.AreEqual("A", 0.ToExcelColumn());
            Assert.AreEqual("B", 1.ToExcelColumn());
            Assert.AreEqual("C", 2.ToExcelColumn());
            Assert.AreEqual("Z", 25.ToExcelColumn());
            Assert.AreEqual("AA", 26.ToExcelColumn());
            Assert.AreEqual("AB", 27.ToExcelColumn());
            Assert.AreEqual("XFE", 16384.ToExcelColumn());
        }

        [TestMethod]
        public void RoundToNearestTests()
        {
            Assert.AreEqual(0, -4.RoundToNearest(10));
            Assert.AreEqual(0, -5.RoundToNearest(10));
            Assert.AreEqual(-10, -6.RoundToNearest(10));
            Assert.AreEqual(-10, -7.RoundToNearest(10));
            Assert.AreEqual(0, 0.RoundToNearest(10));
            Assert.AreEqual(0, -4.RoundToNearest(10));
            Assert.AreEqual(0, -1.RoundToNearest(10));
            Assert.AreEqual(0, 1.RoundToNearest(10));
            Assert.AreEqual(0, 4.RoundToNearest(10));
            Assert.AreEqual(0, 5.RoundToNearest(10));
            Assert.AreEqual(10, 7.RoundToNearest(10));

            Assert.AreEqual(60, 64.RoundToNearest(10));

            Assert.AreEqual(0, 7.RoundToNearest(100));
            Assert.AreEqual(100, 55.RoundToNearest(100));
        }
    }
}
