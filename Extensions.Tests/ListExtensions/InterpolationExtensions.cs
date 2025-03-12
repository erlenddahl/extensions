using System.Linq;
using Extensions.ListExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.ListExtensions
{
    [TestClass]
    public class InterpolationExtensions
    {
        [TestMethod]
        public void Int_VariousHoles()
        {
            var list = new int?[] { 0, 1, 2, null, 4, 5, null, null, 8, 9, null, null, null, null, null, null, 16 };
            var correct = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            var interpolated = list.Interpolate(Extensions.ListExtensions.InterpolationExtensions.WeightedAverage, p => p == null).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void Int_HoleAtStart()
        {
            var list = new int?[] { null, null, null, null, 4, 5, null, null, 8, 9, null, null, null, null, null, null, 16 };
            var correct = new int[] { 4, 4, 4, 4, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            var interpolated = list.Interpolate(Extensions.ListExtensions.InterpolationExtensions.WeightedAverage, p => p == null).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void Int_HoleAtEnd()
        {
            var list = new int?[] { 0, 1, 2, null, 4, 5, null, null, 8, 9, null, null, null, null, null, null, null };
            var correct = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 9, 9, 9, 9, 9, 9, 9, 9 };

            var interpolated = list.Interpolate(Extensions.ListExtensions.InterpolationExtensions.WeightedAverage, p => p == null).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void Int_SingleMissingAtEnd()
        {
            var list = new int?[] { 0, 1, 2, null, 4, 5, null, null, 8, 9, null };
            var correct = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 9 };

            var interpolated = list.Interpolate(Extensions.ListExtensions.InterpolationExtensions.WeightedAverage, p => p == null).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void Int_VariousHoles_DefaultInterpolation()
        {
            var list = new int?[] { 0, 1, 2, null, 4, 5, null, null, 8, 9, null, null, null, null, null, null, 16 };
            var correct = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            var interpolated = list.Interpolate().ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void Int_VariousHoles_CustomInterpolationReason()
        {
            var list = new int[] { 0, 1, 2, -1, 4, 5, -1, -1, 8, 9, -1, -1, -1, -1, -1, -1, 16 };
            var correct = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            var interpolated = list.Interpolate(Extensions.ListExtensions.InterpolationExtensions.WeightedAverage, p => p < 0).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void String_DefaultInterpolation()
        {
            var list = new [] { "hei", "hallo", "heidå", null, null, "halla" };
            var correct = new [] { "hei", "hallo", "heidå", "heidå", "halla", "halla" };

            var interpolated = list.Interpolate().ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void Double_VariousHoles()
        {
            var list = new double?[] { 0, 1, 2, null, 4, 5, null, null, 8, 9, null, null, null, null, null, null, 16 };
            var correct = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            var interpolated = list.Interpolate(Extensions.ListExtensions.InterpolationExtensions.WeightedAverage, p => p == null).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void Double_WithDecimals()
        {
            var list = new double?[] { -1, null, 0, null, null, 0.5 };
            var correct = new double[] { -1, -0.5, 0, 0.166667, 0.3333333, 0.5 };

            var interpolated = list.Interpolate(Extensions.ListExtensions.InterpolationExtensions.WeightedAverage, p => p == null).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i].Value, 0.01);
        }

        [TestMethod]
        public void Double_NoValidValues()
        {
            var list = new double?[] { null, null, null };
            var correct = new double?[] { null, null, null };

            var interpolated = list.Interpolate(Extensions.ListExtensions.InterpolationExtensions.WeightedAverage, p => p == null).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void Double_NoValidButDifferentValues()
        {
            var list = new double?[] { -1, -5, -3 };
            var correct = new double?[] { null, null, null };

            var interpolated = list.Interpolate(Extensions.ListExtensions.InterpolationExtensions.WeightedAverage, p => p < 0).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void Double_OnlyFirst()
        {
            var list = new double?[] { 1, null, null };
            var correct = new double?[] { 1, 1, 1 };

            var interpolated = list.Interpolate(Extensions.ListExtensions.InterpolationExtensions.WeightedAverage, p => p == null).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void Double_OnlyLast()
        {
            var list = new double?[] { null, null, 1 };
            var correct = new double?[] { 1, 1, 1 };

            var interpolated = list.Interpolate(Extensions.ListExtensions.InterpolationExtensions.WeightedAverage, p => p == null).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i], interpolated[i]);
        }

        [TestMethod]
        public void CustomClass_OnlyLast()
        {
            var list = new Item[] { new(-1), new(-1), new(5) };
            var correct = new Item[] { new(5), new(5), new(5) };

            list.Interpolate(p => p < 0,p => p?.Value, (p, v) => p.Value = v.Value);

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i].Value, list[i].Value);
        }

        [TestMethod]
        public void CustomClass_OnlyFirst()
        {
            var list = new Item[] { new(5), new(-1), new(-1) };
            var correct = new Item[] { new(5), new(5), new(5) };

            list.Interpolate(p => p < 0, p => p?.Value, (p, v) => p.Value = v.Value);

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i].Value, list[i].Value);
        }

        [TestMethod]
        public void CustomClass_VariousHoles()
        {
            var list = new Item[] { new(5), new(-1), new(5), new(-1), new(-1), new(8) };
            var correct = new Item[] { new(5), new(5), new(5), new(6), new(7), new(8) };

            list.Interpolate(p => p < 0, p => p?.Value, (p, v) => p.Value = v.Value);

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(correct[i].Value, list[i].Value);
        }

        private class Item
        {
            public double Value { get; set; }

            public Item(double value)
            {
                Value = value;
            }
        }
    }
}
