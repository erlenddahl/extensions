using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace net.erlenddahl.Extensions.Utilities
{
    public class Scaler2D
    {
        public double MinX { get; }
        public double MinY { get; }
        public double MaxX { get; }
        public double MaxY { get; }

        public double SizeX { get; }
        public double SizeY { get; }

        public Scaler2D(IEnumerable<double> xs, IEnumerable<double> ys) : this(double.MaxValue, double.MinValue, double.MaxValue, double.MinValue, xs, ys) { }

        public Scaler2D(double minX, double maxX, double minY, double maxY) : this(minX, maxX, minY, maxY, new double[0], new double[0]) { }

        public Scaler2D(double minX, double maxX, double minY, double maxY, IEnumerable<double> xs, IEnumerable<double> ys)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;

            foreach (var (x, y) in xs.Zip(ys, (x, y) => (x, y)))
            {
                if (x < MinX) MinX = x;
                if (x > MaxX) MaxX = x;
                if (y < MinY) MinY = y;
                if (y > MaxY) MaxY = y;
            }

            SizeX = MaxX - MinX;
            SizeY = MaxY - MinY;
        }

        public Scaler2D Expand<T>(IList<T> line, Func<T, double> xFunc, Func<T, double> yFunc)
        {
            return new Scaler2D(MinX, MaxX, MinY, MaxY, line.Select(xFunc), line.Select(yFunc));
        }

        public Scaler2D Expand(double radius)
        {
            return new Scaler2D(MinX - radius, MaxX + radius, MinY - radius, MaxY + radius);
        }

        public static Scaler2D FromObject<T>(IList<T> line, Func<T, double> xFunc, Func<T, double> yFunc)
        {
            return new Scaler2D(line.Select(xFunc), line.Select(yFunc));
        }

        public (double x, double y) Scale(double valueX, double valueY, Rectangle rect, bool maintainAspectRatio = true)
        {
            return Scale(valueX, valueY, rect.X, rect.X + rect.Width, rect.Y, rect.Y + rect.Height, maintainAspectRatio);
        }

        public (double x, double y) Scale(double valueX, double valueY, double boundaryMinX, double boundaryMaxX, double boundaryMinY, double boundaryMaxY, bool maintainAspectRatio = true)
        {
            var boundarySizeX = boundaryMaxX - boundaryMinX;
            var boundarySizeY = boundaryMaxY - boundaryMinY;

            var facX = boundarySizeX / SizeX;
            var facY = boundarySizeY / SizeY;

            var offsetX = 0d;
            var offsetY = 0d;
            if (maintainAspectRatio)
            {
                facX = facY = Math.Min(facX, facY);

                offsetY = (boundarySizeY - SizeY * facY) / 2d;
                offsetX = (boundarySizeX - SizeX * facX) / 2d;
            }

            var x = (valueX - MinX) * facX + boundaryMinX + offsetX;
            var y = (valueY - MinY) * facY + boundaryMinY + offsetY;

            if (SizeY == 0) y = valueY;
            if (SizeX == 0) x = valueX;

            return (x, y);
        }

        public double ReverseY(double y)
        {
            return MaxY - y + MinY;
        }

        public double ReverseX(double x)
        {
            return MaxX - x + MinX;
        }
    }
}
