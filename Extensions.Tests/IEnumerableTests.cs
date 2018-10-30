using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests
{
    [TestClass]
    public class IEnumerableExtensionsTests
    {
        [TestMethod]
        public void EveryTests()
        {
            var l = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

            var a = l.Every(1).ToList();
            Assert.AreEqual(16, a.Count);
            for (var i = 0; i < a.Count; i++)
                Assert.AreEqual(i, a[i]);

            a = l.Every(5).ToList();
            Assert.AreEqual(4, a.Count);
            Assert.AreEqual(0, a[0]);
            Assert.AreEqual(5, a[1]);
            Assert.AreEqual(10, a[2]);
            Assert.AreEqual(15, a[3]);
        }

        [TestMethod]
        public void SkipTakeTests()
        {
            var l = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};

            var a = l.SkipTake(5, 5).ToList();
            Assert.AreEqual(5, a.Count);
            for (var i = 0; i < a.Count; i++)
                Assert.AreEqual(a[i], i + 5);

            a = l.SkipTake(25, 5).ToList();
            Assert.AreEqual(0, a.Count);
            
            a = l.SkipTake(12, 5).ToList();
            Assert.AreEqual(2, a.Count);
            for (var i = 0; i < a.Count; i++)
                Assert.AreEqual(a[i], i + 12);
        }

        [TestMethod]
        public void TrimTests()
        {
            var l = new[] { 0, 1, 0, 0, 0, 4, 1, 0, 0, 1, 1, 2, 5, 6, 7, 19, 25, 25, 30, 40, 49, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var a = l.Trim(5, p => p.Average() > 5);
            Assert.AreEqual(14, a.Count);
            for (var i = 0; i < a.Count; i++)
                Assert.AreEqual(a[i], l[i + 11]);

            l = new[] { 0, 0, 0 };
            a = l.Trim(5, p => p.Average() > 5);
            Assert.AreEqual(0, a.Count);

            l = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            a = l.Trim(5, p => p.Average() > 5);
            Assert.AreEqual(0, a.Count);
        }

        [TestMethod]
        public void IndexOfTests()
        {
            Assert.AreEqual(1, new[] { 0, 1, 0, 0, 0 }.IndexOf(p => p == 1));
            Assert.AreEqual(0, new[] { 0, 1, 0, 0, 0 }.IndexOf(p => p == 0));

            var v = new[] { new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new[] { 7, 8, 9 } };
            Assert.AreEqual(0, v.IndexOf(p => p.Contains(1)));
            Assert.AreEqual(1, v.IndexOf(p => p.Contains(5)));
            Assert.AreEqual(2, v.IndexOf(p => p.Contains(9)));
        }

        [TestMethod]
        public void SafeAverageTests()
        {
            Assert.AreEqual(new[] { 51, 1, 41, 13, 31.1 }.Average(p => p), new[] { 51, 1, 41, 13, 31.1 }.SafeAverage(p => p));
            Assert.AreEqual(new[] { 51, 1, 41, 13, 31.1 }.Average(), new[] { 51, 1, 41, 13, 31.1 }.SafeAverage());

            Assert.AreEqual(new[] { 151, 1, 41, 143.771, 31.1 }.Average(p => p), new[] { 151, 1, 41, 143.771, 31.1 }.SafeAverage(p => p));
            Assert.AreEqual(new[] { 151, 1, 41, 143.771, 31.1 }.Average(), new[] { 151, 1, 41, 143.771, 31.1 }.SafeAverage());

            Assert.AreEqual(double.MinValue, new double[0].SafeAverage(p => p));
            Assert.AreEqual(-999, new double[0].SafeAverage(p => p, -999));
        }

        [TestMethod]
        public void SafeMinTests()
        {
            Assert.AreEqual(new[] { 51, 1, 41, 13, 31.1 }.Min(p => p), new[] { 51, 1, 41, 13, 31.1 }.SafeMin(p => p));
            Assert.AreEqual(new[] { 51, 1, 41, 13, 31.1 }.Min(), new[] { 51, 1, 41, 13, 31.1 }.SafeMin());

            Assert.AreEqual(new[] { 151, 1, 41, 143.771, 31.1 }.Min(p => p), new[] { 151, 1, 41, 143.771, 31.1 }.SafeMin(p => p));
            Assert.AreEqual(new[] { 151, 1, 41, 143.771, 31.1 }.Min(), new[] { 151, 1, 41, 143.771, 31.1 }.SafeMin());

            Assert.AreEqual(double.MinValue, new double[0].SafeMin(p => p));
            Assert.AreEqual(-999, new double[0].SafeMin(p => p, -999));
        }

        [TestMethod]
        public void SafeMaxTests()
        {
            Assert.AreEqual(new[] { 51, 1, 41, 13, 31.1 }.Max(p => p), new[] { 51, 1, 41, 13, 31.1 }.SafeMax(p => p));
            Assert.AreEqual(new[] { 51, 1, 41, 13, 31.1 }.Max(), new[] { 51, 1, 41, 13, 31.1 }.SafeMax());

            Assert.AreEqual(new[] { 151, 1, 41, 143.771, 31.1 }.Max(p => p), new[] { 151, 1, 41, 143.771, 31.1 }.SafeMax(p => p));
            Assert.AreEqual(new[] { 151, 1, 41, 143.771, 31.1 }.Max(), new[] { 151, 1, 41, 143.771, 31.1 }.SafeMax());

            Assert.AreEqual(double.MinValue, new double[0].SafeMax(p => p));
            Assert.AreEqual(-999, new double[0].SafeMax(p => p, -999));
        }

        [TestMethod]
        public void NullableAverageTests()
        {
            Assert.AreEqual(new[] {51, 1, 13, 31.1}.Average(p => p), new double?[] {51, 1, null, 13, 31.1}.NullableAverage(p => p));
            Assert.AreEqual(new[] {51, 1, 13, 31.1}.Average(), new double?[] {51, 1, null, 13, 31.1}.NullableAverage());

            Assert.AreEqual(new[] {151, 143.771, 31.1}.Average(p => p), new double?[] {151, null, null, 143.771, 31.1}.NullableAverage(p => p));
            Assert.AreEqual(new[] {151, 143.771, 31.1}.Average(), new double?[] {151, null, null, 143.771, 31.1}.NullableAverage());

            Assert.AreEqual(null, new double[0].NullableAverage(p => p));
            Assert.AreEqual(-999, new double[0].NullableAverage(p => p, -999));
        }

        [TestMethod]
        public void GetOrDefaultTests()
        {
            var arr = new[] {51, 1, 41, 13, 31.1};
            for (var i = 0; i < arr.Length; i++)
                Assert.AreEqual(arr[i], arr.GetOrDefault(i));

            Assert.AreEqual(default(int), arr.GetOrDefault(-1));
            Assert.AreEqual(default(int), arr.GetOrDefault(5));
            Assert.AreEqual(default(int), arr.GetOrDefault(100));

            Assert.AreEqual(int.MinValue, arr.GetOrDefault(-1, int.MinValue));
            Assert.AreEqual(int.MinValue, arr.GetOrDefault(5, int.MinValue));
            Assert.AreEqual(int.MinValue, arr.GetOrDefault(100, int.MinValue));
        }

        [TestMethod]
        public void CompressToConsecutiveGroupsTests()
        {
            Check(new int[] { });
            Check(new[] {1}, new[] {1, 1});
            Check(new[] {1, 2}, new[] {1, 2});
            Check(new[] {1, 2, 3}, new[] {1, 3});
            Check(new[] {1, 3}, new[] {1, 1}, new[] {3, 3});
            Check(new[] {1, 2, 3, 5, 6, 7}, new[] {1, 3}, new[] {5, 7});
            Check(new[] {3, 6, 7, 2, 1, 5}, new[] {1, 3}, new[] {5, 7});
        }

        private void Check(int[] list, params int[][] results)
        {
            var res = list.CompressToConsecutiveGroups().ToList();
            Assert.AreEqual(results.Length, res.Count);
            for (var i = 0; i < res.Count; i++)
            {
                Assert.AreEqual(results[i][0], res[i].Start);
                Assert.AreEqual(results[i][1], res[i].End);
            }
        }
    }
}
