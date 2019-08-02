using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Utilities
{
    public class IncrementalStatistics
    {
        private double _variance;

        public double Variance
        {
            get => Count < 2 ? double.NaN : _variance;
            private set => _variance = value;
        }

        public double StandardDeviation => Math.Sqrt(Variance);
        public double Average { get; private set; }
        public double Min { get; private set; }
        public double Max { get; private set; }
        public int Count { get; private set; }

        public void AddObservation(double observation)
        {
            Count++;
            if (Count == 1)
            {
                Average = Min = Max = observation;
                return;
            }

            var prevAverage = Average;
            var prevVariance = _variance;

            Average = (prevAverage * (Count - 1) + observation) / Count;
            Variance = (Count - 2) * prevVariance / (Count - 1) + Math.Pow(observation - prevAverage, 2) / Count;
            Min = Math.Min(Min, observation);
            Max = Math.Max(Max, observation);
        }
    }
}
