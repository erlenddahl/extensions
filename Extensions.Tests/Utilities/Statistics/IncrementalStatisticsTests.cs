using System.Linq;
using Extensions.Tests.IEnumerableExtensions;
using Extensions.Utilities.Csv;
using Extensions.Utilities.Statistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Utilities.Statistics
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
        public void IdenticallyWeightedAverageTests()
        {
            var list = new[] { 0d, 1d, 2d, 3d, 4d, 5d, 5d, 5d, 5d, 5d, 10d };

            var inc = new IncrementalStatistics();
            for (var i = 0; i < list.Length; i++)
            {
                inc.AddObservation(list[i], 2);

                var target = list.Take(i + 1).Average();
                Assert.AreEqual(target, inc.WeightedAverage);
            }
        }

        [TestMethod]
        public void WeightedAverageTests_ZeroWeightIsIgnored()
        {
            var inc = new IncrementalStatistics();
            inc.AddObservation(5, 0);
            inc.AddObservation(3);
            Assert.AreEqual(3, inc.WeightedAverage);
        }

        [TestMethod]
        public void WeightedAverageTests_SingleObservation()
        {
            var inc = new IncrementalStatistics();
            inc.AddObservation(5, 100);
            Assert.AreEqual(5, inc.WeightedAverage, 0.05);
        }

        [TestMethod]
        public void WeightedAverageTests_WeightWorks()
        {
            var inc = new IncrementalStatistics();
            inc.AddObservation(5, 100);
            inc.AddObservation(3);
            Assert.AreEqual(5, inc.WeightedAverage, 0.05);
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

        [TestMethod]
        public void AppendEmpty()
        {
            var list = new[] { 0d, 1d, 2d, 3d, 4d, 5d, 5d, 5d, 5d, 5d, 10d };
            var correct = new IncrementalStatistics(list);

            var inc = new IncrementalStatistics(list);
            var inc2 = new IncrementalStatistics();
            inc.Append(inc2);

            Assert.AreEqual(correct.Count, inc.Count);
            Assert.AreEqual(correct.Min, inc.Min);
            Assert.AreEqual(correct.Max, inc.Max);
            Assert.AreEqual(correct.Average, inc.Average);
            Assert.AreEqual(correct.Sum, inc.Sum);
            Assert.AreEqual(correct.StandardDeviation, inc.StandardDeviation);
            Assert.AreEqual(correct.Variance, inc.Variance);
        }

        [TestMethod]
        public void AppendFromEmpty()
        {
            var list = new[] { 0d, 1d, 2d, 3d, 4d, 5d, 5d, 5d, 5d, 5d, 10d };
            var correct = new IncrementalStatistics(list);

            var inc = new IncrementalStatistics();
            var inc2 = new IncrementalStatistics(list);
            inc.Append(inc2);

            Assert.AreEqual(correct.Count, inc.Count);
            Assert.AreEqual(correct.Min, inc.Min);
            Assert.AreEqual(correct.Max, inc.Max);
            Assert.AreEqual(correct.Average, inc.Average);
            Assert.AreEqual(correct.Sum, inc.Sum);
            Assert.AreEqual(correct.Variance, inc.Variance);
            Assert.AreEqual(correct.StandardDeviation, inc.StandardDeviation);
        }

        [TestMethod]
        public void Append()
        {
            var list = new[] { 1d, 2d, 3d, 4d, 5d, 5d, 5d, 5d, 5d, 10d };
            var correct = new IncrementalStatistics(list);

            var inc = new IncrementalStatistics(list.Take(5));
            var inc2 = new IncrementalStatistics(list.Skip(5));
            inc.Append(inc2);

            Assert.AreEqual(correct.Count, inc.Count);
            Assert.AreEqual(correct.Min, inc.Min);
            Assert.AreEqual(correct.Max, inc.Max);
            Assert.AreEqual(correct.Average, inc.Average);
            Assert.AreEqual(correct.Sum, inc.Sum);
            Assert.AreEqual(correct.Variance, inc.Variance);
            Assert.AreEqual(correct.StandardDeviation, inc.StandardDeviation);
        }

        [TestMethod]
        public void Concatenate()
        {
            var list = new[] { 1d, 2d, 3d, 4d, 5d, 5d, 5d, 5d, 5d, 10d };
            var correct = new IncrementalStatistics(list);

            var partA = new IncrementalStatistics(list.Take(5));
            var partB = new IncrementalStatistics(list.Skip(5));
            var conc = IncrementalStatistics.Concatenate(new[] { partA, partB });

            Assert.AreEqual(correct.Count, conc.Count);
            Assert.AreEqual(correct.Min, conc.Min);
            Assert.AreEqual(correct.Max, conc.Max);
            Assert.AreEqual(correct.Average, conc.Average);
            Assert.AreEqual(correct.Sum, conc.Sum);
            Assert.AreEqual(correct.Variance, conc.Variance);
            Assert.AreEqual(correct.StandardDeviation, conc.StandardDeviation);
        }
    }
}
