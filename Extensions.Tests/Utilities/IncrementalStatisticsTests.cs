using System;
using System.Diagnostics;
using System.Linq;
using Extensions.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Utilities
{
    [TestClass]
    public class IncrementalStatisticsTests
    {
        [TestMethod]
        public void AverageTests()
        {
            var list = new[] { 0d, 1d, 2d, 3d, 4d, 5d, 5d, 5d, 5d, 5d, 10d };

            var inc = new IncrementalStatistics();
            for (var i = 0; i < list.Length; i++)
            {
                inc.AddObservation(list[i]);

                var target = list.Take(i + 1).Average();
                Assert.AreEqual(target, inc.Average);
            }
        }

        [TestMethod]
        public void MinTests()
        {
            var list = new[] { 0d, 1d, 2d, 3d, 4d, 5d, 5d, 5d, 5d, 5d, 10d };

            var inc = new IncrementalStatistics();
            for (var i = 0; i < list.Length; i++)
            {
                inc.AddObservation(list[i]);

                var target = list.Take(i + 1).Min();
                Assert.AreEqual(target, inc.Min);
            }
        }

        [TestMethod]
        public void MaxTests()
        {
            var list = new[] { 0d, 1d, 2d, 3d, 4d, 5d, 5d, 5d, 5d, 5d, 10d };

            var inc = new IncrementalStatistics();
            for (var i = 0; i < list.Length; i++)
            {
                inc.AddObservation(list[i]);

                var target = list.Take(i + 1).Max();
                Assert.AreEqual(target, inc.Max);
            }
        }

        [TestMethod]
        public void CountTests()
        {
            var list = new[] { 0d, 1d, 2d, 3d, 4d, 5d, 5d, 5d, 5d, 5d, 10d };

            var inc = new IncrementalStatistics();
            for (var i = 0; i < list.Length; i++)
            {
                inc.AddObservation(list[i]);
                Assert.AreEqual(i + 1, inc.Count);
            }
        }

        [TestMethod]
        public void VarianceTests()
        {
            var list = new[] { 0d, 1d, 2d, 3d, 4d, 5d, 5d, 5d, 5d, 5d, 10d };

            var inc = new IncrementalStatistics();
            for (var i = 0; i < list.Length; i++)
            {
                inc.AddObservation(list[i]);

                var target = list.Take(i + 1).Variance();
                if (double.IsNaN(target)) Assert.IsTrue(double.IsNaN(inc.Variance));
                else Assert.AreEqual(target, inc.Variance, 0.0000005);
            }
        }

        [TestMethod]
        public void StandardDeviationTests()
        {
            var list = new[] { 0d, 1d, 2d, 3d, 4d, 5d, 5d, 5d, 5d, 5d, 10d };

            var inc = new IncrementalStatistics();
            for (var i = 0; i < list.Length; i++)
            {
                inc.AddObservation(list[i]);

                var target = list.Take(i + 1).StandardDeviation();
                if (double.IsNaN(target)) Assert.IsTrue(double.IsNaN(inc.StandardDeviation));
                else Assert.AreEqual(target, inc.StandardDeviation, 0.0000005);
            }
        }
    }
}
