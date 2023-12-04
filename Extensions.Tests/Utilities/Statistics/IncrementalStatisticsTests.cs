using System;
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

        [TestMethod]
        public void SimpleHistogram_FromList()
        {
            var inc = new IncrementalStatistics(new double[] { 1, 2, 3, 4, 5, 11, 16, 19, 25 }, 10);

            CollectionAssert.AreEquivalent(new long[] { 0, 10, 20 }, inc.Buckets.Keys);
            Assert.AreEqual(5, inc.Buckets[0]);
            Assert.AreEqual(3, inc.Buckets[10]);
            Assert.AreEqual(1, inc.Buckets[20]);
        }

        [TestMethod]
        public void SimpleHistogram_Added()
        {
            var inc = new IncrementalStatistics(10);

            foreach (var v in new double[] { 1, 2, 3, 4, 5, 11, 16, 19, 25 })
                inc.AddObservation(v);

            CollectionAssert.AreEquivalent(new long[] { 0, 10, 20 }, inc.Buckets.Keys);
            Assert.AreEqual(5, inc.Buckets[0]);
            Assert.AreEqual(3, inc.Buckets[10]);
            Assert.AreEqual(1, inc.Buckets[20]);
        }

        [TestMethod]
        public void SimpleHistogram_Merged()
        {
            var inc = new IncrementalStatistics(new double[] { 1, 2, 3, 4, 5, 11, 16, 19, 25 }, 10);
            var inc2 = new IncrementalStatistics(new double[] { 1, 2, 7, 8, 11, 17, 14, 16, 19, 55 }, 10);

            inc.Append(inc2);

            CollectionAssert.AreEquivalent(new long[] { 0, 10, 20, 50 }, inc.Buckets.Keys);
            Assert.AreEqual(9, inc.Buckets[0]);
            Assert.AreEqual(8, inc.Buckets[10]);
            Assert.AreEqual(1, inc.Buckets[20]);
            Assert.AreEqual(1, inc.Buckets[50]);
        }

        [TestMethod]
        public void SimpleHistogram_MergeWithDifferentBuckets_Fails()
        {
            var inc = new IncrementalStatistics(new double[] { 1, 2, 3, 4, 5, 11, 16, 19, 25 }, 10);
            var inc2 = new IncrementalStatistics(new double[] { 1, 2, 7, 8, 11, 17, 14, 16, 19, 55 }, 100);

            try
            {
                inc.Append(inc2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void SimpleHistogram_DifferentBucketSize()
        {
            var inc = new IncrementalStatistics(new double[] { 1, 2, 7, 8, 11, 17, 14, 16, 19, 55, 548 }, 100);

            CollectionAssert.AreEquivalent(new long[] { 0, 500 }, inc.Buckets.Keys);
            Assert.AreEqual(10, inc.Buckets[0]);
            Assert.AreEqual(1, inc.Buckets[500]);
        }

        [TestMethod]
        public void SimpleHistogram_NegativeNumbers()
        {
            var inc = new IncrementalStatistics(new double[] { -1, -2, -7, 8, 11, 17, 14, 16, 19, 55, -548, 499 }, 100);

            CollectionAssert.AreEquivalent(new long[] { -600, -100, 0, 400 }, inc.Buckets.Keys);
            Assert.AreEqual(1, inc.Buckets[-600]);
            Assert.AreEqual(3, inc.Buckets[-100]);
            Assert.AreEqual(7, inc.Buckets[0]);
            Assert.AreEqual(1, inc.Buckets[400]);
        }
    }
}
