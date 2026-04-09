using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.ListExtensions
{
    [TestClass]
    public class WeightedAverageExtensions
    {
        [TestMethod]
        public void AverageCase()
        {
            var nums = new[] { 1.0, 2.0, 3.0 };
            Assert.AreEqual(nums.Average(), nums.WeightedAverage(p => p, p => 1));
        }

        [TestMethod]
        public void AlwaysOne()
        {
            var nums = new[] { 1.0, 2.0, 3.0 };
            Assert.AreEqual(14d/6d, nums.WeightedAverage(p => p, p => p));
        }

        [TestMethod]
        public void ZeroWeight()
        {
            var nums = new[] {new[] {1.0, 1.0}, new[] {2.0, 0.0}, new[] {2.0, 1.0}};
            Assert.AreEqual(1.5, nums.WeightedAverage(p => p[0], p => p[1]));
        }

        [TestMethod]
        public void SimpleCase1()
        {
            var nums = new[] { new[] { 0.9, 0.25 }, new[] { 0.75, 0.50 }, new[] { 0.87, 0.25 } };
            Assert.AreEqual(0.8175, nums.WeightedAverage(p => p[0], p => p[1]));
        }

        [TestMethod]
        public void SimpleCase2()
        {
            var nums = new[] { new[] { 65, 1.0 }, new[] { 60, 1.0 }, new[] { 80, 2.0 } ,new []{95,3.0}};
            Assert.AreEqual(81.42857, nums.WeightedAverage(p => p[0], p => p[1]), 0.00005);
        }
    }
}
