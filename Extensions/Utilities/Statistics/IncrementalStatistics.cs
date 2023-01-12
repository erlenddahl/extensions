using System.Collections.Generic;

namespace Extensions.Utilities.Statistics
{
    public class IncrementalStatistics
    {
        private double _variance;

        public double Variance
        {
            get => Count < 1 ? double.NaN : _variance;
            private set => _variance = value;
        }
        
        public double Sum { get; private set; }
        public double WeightSum { get; private set; }

        public double SumSquared { get; private set; }

        public double StandardDeviation => System.Math.Sqrt(Variance);
        public double Average { get; private set; }
        public double WeightedAverage { get; private set; }
        public double Min { get; private set; }
        public double Max { get; private set; }
        public int Count { get; private set; }

        public IncrementalStatistics()
        {

        }

        public IncrementalStatistics(IEnumerable<double> values)
        {
            foreach (var value in values)
                AddObservation(value);
        }

        public void AddObservation(double observation, double weight = 1)
        {
            Count++;
            Sum += observation * weight;
            WeightSum += weight;
            SumSquared += observation * observation;

            if (Count == 1)
            {
                WeightedAverage = Average = Min = Max = observation;
                return;
            }

            Average = Sum / Count;
            WeightedAverage = Sum / WeightSum;

            Variance = (SumSquared - 2 * Average * Sum + Count * Average * Average) / Count;
            Min = System.Math.Min(Min, observation);
            Max = System.Math.Max(Max, observation);
        }

        public void Append(IncrementalStatistics other)
        {
            Sum += other.Sum;
            SumSquared += other.SumSquared;
            Count += other.Count;
            Average = Sum / Count;
            WeightedAverage = Sum / WeightSum;

            if (!double.IsNaN(StandardDeviation) && !double.IsNaN(other.StandardDeviation))
            {
                Variance = (SumSquared - 2 * Average * Sum + Count * Average * Average) / Count;
            }
            else if (double.IsNaN(StandardDeviation))
                Variance = other.Variance;


            Min = System.Math.Min(Min, other.Min);
            Max = System.Math.Max(Max, other.Max);
        }

        public override string ToString()
        {
            return ToString("; ");
        }

        public string ToString(string separator, string linePrefix = "")
        {
            return linePrefix + "Variance: " + Variance + separator +
                   linePrefix + "Sum: " + Sum + separator +
                   linePrefix + "StandardDeviation: " + StandardDeviation + separator +
                   linePrefix + "Average: " + Average + separator +
                   linePrefix + "Min: " + Min + separator +
                   linePrefix + "Max: " + Max + separator +
                   linePrefix + "Count: " + Count + separator;
        }

        public static IncrementalStatistics Concatenate(IEnumerable<IncrementalStatistics> stats)
        {
            var sum = new IncrementalStatistics();
            foreach (var s in stats) sum.Append(s);
            return sum;
        }
    }
}
