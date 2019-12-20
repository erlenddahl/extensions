using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Utilities
{
    public class Scaler2D
    {
        public double MinX { get; } = double.MaxValue;
        public double MinY { get; } = double.MaxValue;
        public double MaxX { get; } = double.MinValue;
        public double MaxY { get; } = double.MinValue;

        public double SizeX { get; }
        public double SizeY { get; }

        public Scaler2D(IEnumerable<double> xs, IEnumerable<double> ys)
        {
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
