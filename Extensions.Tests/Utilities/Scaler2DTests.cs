using System;
using System.Linq;
using Extensions.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Utilities
{
    [TestClass]
    public class Scaler2DTests
    {
        [TestMethod]
        public void Creation()
        {
            var x = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var y = new double[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var i = Enumerable.Range(0, x.Length).ToArray();

            var scaler = Scaler2D.FromObject<int>(i, p => x[p], p => y[p]);

            Assert.AreEqual(1, scaler.MinX);
            Assert.AreEqual(2, scaler.MinY);
            Assert.AreEqual(10, scaler.MaxX);
            Assert.AreEqual(11, scaler.MaxY);
        }

        [TestMethod]
        public void ExpandRadius()
        {
            var x = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var y = new double[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var i = Enumerable.Range(0, x.Length).ToArray();

            var scaler = Scaler2D.FromObject<int>(i, p => x[p], p => y[p]);
            scaler = scaler.Expand(7);

            Assert.AreEqual(-6, scaler.MinX);
            Assert.AreEqual(-5, scaler.MinY);
            Assert.AreEqual(17, scaler.MaxX);
            Assert.AreEqual(18, scaler.MaxY);
        }

        [TestMethod]
        public void ExpandSame()
        {
            var x = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var y = new double[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var i = Enumerable.Range(0, x.Length).ToArray();

            var scaler = Scaler2D.FromObject<int>(i, p => x[p], p => y[p]);
            scaler = scaler.Expand(i, p => x[p], p => y[p]);

            Assert.AreEqual(1, scaler.MinX);
            Assert.AreEqual(2, scaler.MinY);
            Assert.AreEqual(10, scaler.MaxX);
            Assert.AreEqual(11, scaler.MaxY);
        }

        [TestMethod]
        public void ExpandLess()
        {
            var x = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var y = new double[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var i = Enumerable.Range(0, x.Length).ToArray();

            var scaler = Scaler2D.FromObject<int>(i, p => x[p], p => y[p]);
            scaler = scaler.Expand(i.Skip(2).Take(3).ToArray(), p => x[p], p => y[p]);

            Assert.AreEqual(1, scaler.MinX);
            Assert.AreEqual(2, scaler.MinY);
            Assert.AreEqual(10, scaler.MaxX);
            Assert.AreEqual(11, scaler.MaxY);
        }

        [TestMethod]
        public void ExpandMore()
        {
            var x = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var y = new double[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var i = Enumerable.Range(0, x.Length).ToArray();

            var scaler = Scaler2D.FromObject<int>(i, p => x[p], p => y[p]);

            x = new double[] {0, 1, 2, 3, 4, 5, 17 };
            y = new double[] {-1, 0, 0, 0, 0, 0, 19 };
            i = Enumerable.Range(0, x.Length).ToArray();

            scaler = scaler.Expand(i, p => x[p], p => y[p]);

            Assert.AreEqual(0, scaler.MinX);
            Assert.AreEqual(-1, scaler.MinY);
            Assert.AreEqual(17, scaler.MaxX);
            Assert.AreEqual(19, scaler.MaxY);
        }

        [TestMethod]
        public void PositiveNumbers_ScaleToSameSize()
        {
            var x = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var y = new double[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var i = Enumerable.Range(0, x.Length).ToArray();

            var scaler = Scaler2D.FromObject<int>(i, p => x[p], p => y[p]);

            Assert.AreEqual(1, scaler.MinX);
            Assert.AreEqual(2, scaler.MinY);
            Assert.AreEqual(10, scaler.MaxX);
            Assert.AreEqual(11, scaler.MaxY);

            var res = scaler.Scale(1, 2, 1, 10, 2, 11);
            Assert.AreEqual(1, res.x);
            Assert.AreEqual(2, res.y);

            res = scaler.Scale(6, 3, 1, 10, 2, 11);
            Assert.AreEqual(6, res.x);
            Assert.AreEqual(3, res.y);

            res = scaler.Scale(10, 11, 1, 10, 2, 11);
            Assert.AreEqual(10, res.x);
            Assert.AreEqual(11, res.y);
        }

        [TestMethod]
        public void PositiveNumbers_NoXChange_ScaleToSameSize()
        {
            var x = new double[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
            var y = new double[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var i = Enumerable.Range(0, x.Length).ToArray();

            var scaler = Scaler2D.FromObject<int>(i, p => x[p], p => y[p]);

            Assert.AreEqual(1, scaler.MinX);
            Assert.AreEqual(2, scaler.MinY);
            Assert.AreEqual(1, scaler.MaxX);
            Assert.AreEqual(11, scaler.MaxY);

            var res = scaler.Scale(1, 2, 1, 10, 2, 11);
            Assert.AreEqual(1, res.x);
            Assert.AreEqual(2, res.y);

            res = scaler.Scale(6, 3, 1, 10, 2, 11);
            Assert.AreEqual(6, res.x);
            Assert.AreEqual(3, res.y);

            res = scaler.Scale(10, 11, 1, 10, 2, 11);
            Assert.AreEqual(10, res.x);
            Assert.AreEqual(11, res.y);
        }

        [TestMethod]
        public void PositiveNumbers_NoYChange_ScaleToSameSize()
        {
            var x = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var y = new double[] {2, 2, 2, 2, 2, 2, 2, 2, 2, 2};
            var i = Enumerable.Range(0, x.Length).ToArray();

            var scaler = Scaler2D.FromObject<int>(i, p => x[p], p => y[p]);

            Assert.AreEqual(1, scaler.MinX);
            Assert.AreEqual(2, scaler.MinY);
            Assert.AreEqual(10, scaler.MaxX);
            Assert.AreEqual(2, scaler.MaxY);

            var res = scaler.Scale(1, 2, 1, 10, 2, 11);
            Assert.AreEqual(1, res.x);
            Assert.AreEqual(2, res.y);

            res = scaler.Scale(6, 3, 1, 10, 2, 11);
            Assert.AreEqual(6, res.x);
            Assert.AreEqual(3, res.y);

            res = scaler.Scale(10, 11, 1, 10, 2, 11);
            Assert.AreEqual(10, res.x);
            Assert.AreEqual(11, res.y);
        }

        [TestMethod]
        public void PositiveNumbers_ScaleToDifferentSize()
        {
            var x = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var y = new double[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var i = Enumerable.Range(0, x.Length).ToArray();

            var scaler = Scaler2D.FromObject<int>(i, p => x[p], p => y[p]);

            Assert.AreEqual(1, scaler.MinX);
            Assert.AreEqual(2, scaler.MinY);
            Assert.AreEqual(10, scaler.MaxX);
            Assert.AreEqual(11, scaler.MaxY);

            var res = scaler.Scale(1, 2, 1, 5, 2, 6);
            Assert.AreEqual(1, res.x);
            Assert.AreEqual(2, res.y);

            res = scaler.Scale(5.5, 6.5, 1, 5, 2, 6);
            Assert.AreEqual(3, res.x);
            Assert.AreEqual(4, res.y);

            res = scaler.Scale(10, 11, 1, 5, 2, 6);
            Assert.AreEqual(5, res.x);
            Assert.AreEqual(6, res.y);
        }

        [TestMethod]
        public void NegativeNumbers_ScaleToSameSize()
        {
            var x = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var y = new double[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var i = Enumerable.Range(0, x.Length).ToArray();

            var scaler = Scaler2D.FromObject<int>(i, p => -x[p], p => -y[p]);

            Assert.AreEqual(-10, scaler.MinX);
            Assert.AreEqual(-11, scaler.MinY);
            Assert.AreEqual(-1, scaler.MaxX);
            Assert.AreEqual(-2, scaler.MaxY);

            var res = scaler.Scale(-1, -2, -10, -1, -11, -2);
            Assert.AreEqual(-1, res.x);
            Assert.AreEqual(-2, res.y);

            res = scaler.Scale(-6, -3, -10, -1, -11, -2);
            Assert.AreEqual(-6, res.x);
            Assert.AreEqual(-3, res.y);

            res = scaler.Scale(-10, -11, -10, -1, -11, -2);
            Assert.AreEqual(-10, res.x);
            Assert.AreEqual(-11, res.y);
        }

        [TestMethod]
        public void NegativeNumbers_ScaleToDifferentSize()
        {
            var x = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var y = new double[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var i = Enumerable.Range(0, x.Length).ToArray();

            var scaler = Scaler2D.FromObject<int>(i, p => -x[p], p => -y[p]);

            Assert.AreEqual(-10, scaler.MinX);
            Assert.AreEqual(-11, scaler.MinY);
            Assert.AreEqual(-1, scaler.MaxX);
            Assert.AreEqual(-2, scaler.MaxY);

            var res = scaler.Scale(-1, -2, -5, -1, -6, -2);
            Assert.AreEqual(-1, res.x);
            Assert.AreEqual(-2, res.y);

            res = scaler.Scale(-5.5, -6.5, -5, -1, -6, -2);
            Assert.AreEqual(-3, res.x);
            Assert.AreEqual(-4, res.y);

            res = scaler.Scale(-10, -11, -5, -1, -6, -2);
            Assert.AreEqual(-5, res.x);
            Assert.AreEqual(-6, res.y);
        }

        [TestMethod]
        public void CrossingZero_ScaleToSameSize()
        {
            var x = new double[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 };
            var y = new double[] { -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4 };
            var i = Enumerable.Range(0, x.Length).ToArray();

            var scaler = Scaler2D.FromObject<int>(i, p => x[p], p => y[p]);

            Assert.AreEqual(-5, scaler.MinX);
            Assert.AreEqual(-6, scaler.MinY);
            Assert.AreEqual(5, scaler.MaxX);
            Assert.AreEqual(4, scaler.MaxY);

            var res = scaler.Scale(-5, -6, -5, 5, -6, 4);
            Assert.AreEqual(-5, res.x);
            Assert.AreEqual(-6, res.y);

            res = scaler.Scale(3, 2, -5, 5, -6, 4);
            Assert.AreEqual(3, res.x);
            Assert.AreEqual(2, res.y);

            res = scaler.Scale(5, 4, -5, 5, -6, 4);
            Assert.AreEqual(5, res.x);
            Assert.AreEqual(4, res.y);
        }

        [TestMethod]
        public void CrossingZero_ScaleToDifferentSize()
        {
            var x = new double[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 };
            var y = new double[] { -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4 };
            var i = Enumerable.Range(0, x.Length).ToArray();

            var scaler = Scaler2D.FromObject<int>(i, p => x[p], p => y[p]);

            Assert.AreEqual(-5, scaler.MinX);
            Assert.AreEqual(-6, scaler.MinY);
            Assert.AreEqual(5, scaler.MaxX);
            Assert.AreEqual(4, scaler.MaxY);

            var res = scaler.Scale(-5, -6, 0, 5, 0, 5);
            Assert.AreEqual(0, res.x);
            Assert.AreEqual(0, res.y);

            res = scaler.Scale(3, 2, 0, 5, 0, 5);
            Assert.AreEqual(4, res.x);
            Assert.AreEqual(4, res.y);

            res = scaler.Scale(5, 4, 0, 5, 0, 5);
            Assert.AreEqual(5, res.x);
            Assert.AreEqual(5, res.y);
        }

        [TestMethod]
        public void CrossingZero_Reverse()
        {
            var x = new double[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 };
            var y = new double[] { -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4 };
            var i = Enumerable.Range(0, x.Length).ToArray();

            var scaler = Scaler2D.FromObject<int>(i, p => x[p], p => y[p]);

            Assert.AreEqual(5, scaler.ReverseX(-5));
            Assert.AreEqual(-5, scaler.ReverseX(5));
            Assert.AreEqual(4, scaler.ReverseY(-6));
            Assert.AreEqual(-6, scaler.ReverseY(4));


            Assert.AreEqual(2, scaler.ReverseY(-4));
            Assert.AreEqual(0, scaler.ReverseY(-2));
        }
    }
}
