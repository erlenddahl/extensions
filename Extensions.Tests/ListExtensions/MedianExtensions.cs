using System;
using System.IO;
using System.Linq;
using Extensions.ListExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.ListExtensions
{
    [TestClass]
    public class MedianExtensions
    {
        [TestMethod]
        public void MedianTests()
        {
            Assert.AreEqual(29, new double[] { 12, 52, 22, 43, 12.43, 42.51, 19.999, 25, 29, 49, 38, 40.14, 1 }.Median(), 0.00001);
            Assert.AreEqual(33.5, new double[] { 12, 52, 22, 43, 12.43, 42.51, 19.999, 25, 29, 49, 38, 40.14 }.Median(), 0.00001);
            Assert.AreEqual(2m, new decimal[] { 1, 2, 3 }.Median());
            Assert.AreEqual(1.5f, new float[] { 1, 2 }.Median(), 0.000001f);
            Assert.AreEqual(15f, new float[] { 15 }.Median(), 0.000001f);
            Assert.AreEqual(2.5m, new decimal[] { 1, 2, 3, 4 }.Median());

            try
            {
                Console.WriteLine(new float[] { }.Median());
                Assert.Fail();
            }
            catch (InvalidDataException ex)
            {

            }
        }

        [TestMethod]
        public void MedianAround()
        {
            var list = new[] { 1, 2, 3, 4, 5, 6, 6, 8, 7, 10, 11, 12 };

            Assert.AreEqual(2, list.MedianAround(1, 1, p => p));
            Assert.AreEqual(1.5, list.MedianAround(0, 1, p => p), 0.000005);
            Assert.AreEqual(2, list.MedianAround(0, 2, p => p), 0.000005);
            Assert.AreEqual(12, list.MedianAround(11, 0, p => p), 0.000005);
            Assert.AreEqual(6, list.MedianAround(6, 3, p => p), 0.000005);
            Assert.AreEqual(8, list.MedianAround(8, 1, p => p), 0.000005);
            Assert.AreEqual(8, list.MedianAround(8, 1, p => p), 0.000005);
        }

        public class MsTest
        {
            public double Value { get; set; }
            public int Other { get; set; }
        }

        [TestMethod]
        public void MedianSmooth_WithObject()
        {
            var list = new[]
            {
                new MsTest() {Value = 1, Other = 15},
                new MsTest() {Value = 2, Other = 15},
                new MsTest() {Value = 3, Other = 15},
                new MsTest() {Value = 4, Other = 15},
                new MsTest() {Value = 5, Other = 15},
                new MsTest() {Value = 6, Other = 15},
                new MsTest() {Value = 6, Other = 15},
                new MsTest() {Value = 8, Other = 15},
                new MsTest() {Value = 7, Other = 15},
                new MsTest() {Value = 10, Other = 15},
                new MsTest() {Value = 11, Other = 15},
                new MsTest() {Value = 12, Other = 15}
            };
            var answer = new[] { 2, 2.5, 3, 4, 5, 6, 6, 7, 8, 10, 10.5, 11 };

            list.MedianSmooth("Value", 2);

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(answer[i], list[i].Value, 0.00000005);

            list.MedianSmooth("Value", 15);

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(6, list[i].Value, 0.00000005);
        }

        [TestMethod]
        public void MedianSmooth_Directly()
        {
            var list = new[] { 1, 2, 3, 4, 5, 6, 6, 8, 7, 10, 11, 12 };
            var answer = new[] { 2, 2.5, 3, 4, 5, 6, 6, 7, 8, 10, 10.5, 11 };

            var smoothed = list.MedianSmooth(2).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(answer[i], smoothed[i], 0.00000005);

            smoothed = list.MedianSmooth(15).ToList();

            for (var i = 0; i < list.Length; i++)
                Assert.AreEqual(6, smoothed[i], 0.00000005);
        }
    }
}
