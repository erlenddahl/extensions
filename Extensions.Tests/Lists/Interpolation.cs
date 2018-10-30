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
    public class NeighbourTests
    {
        [TestMethod]
        public void GeneralInterpolation__Empty_Int()
        {
            var list = new int?[] { 0, 1, 2, null, 4, 5, null, null, 8, 9, null, null, null, null, null, null, 16 };
            var correct = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            var interpolated = list.Interpolate(p => (int)p.ToNonNullable().WeightedAverage(), p => p == null).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void Interpolation__Empty_Int()
        {
            var list = new int?[] { 0, 1, 2, null, 4, 5, null, null, 8, 9, null, null, null, null, null, null, 16 };
            var correct = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            var interpolated = list.Interpolate().ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void Interpolation__Empty_Int_AsDouble()
        {
            var list = new int?[] { 0, 1, 2, null, 4, 5, null, null, 8, 9, null, null, null, null, null, null, 16 };
            var correct = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            var interpolated = list.Interpolate().ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void GeneralInterpolation__Int()
        {
            var list = new int[] { 0, 1, 2, -1, 4, 5, -1, -1, 8, 9, -1, -1, -1, -1, -1, -1, 16 };
            var correct = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            var interpolated = list.Interpolate(p => (int)p.WeightedAverage(), p => p < 0).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void GeneralInterpolation__String()
        {
            var list = new [] { "hei", "hallo", "heidå", null, null, "halla" };
            var correct = new [] { "hei", "hallo", "heidå", "heidå", "halla", "halla" };

            var interpolated = list.Interpolate().ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void GeneralInterpolation__Empty_Double()
        {
            var list = new double?[] { 0, 1, 2, null, 4, 5, null, null, 8, 9, null, null, null, null, null, null, 16 };
            var correct = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            var interpolated = list.Interpolate(p => (int)p.ToNonNullable().WeightedAverage(), p => p == null).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }
    }
}
