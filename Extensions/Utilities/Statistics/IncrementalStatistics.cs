using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Extensions.Utilities.Csv;

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

        public double StandardDeviation => Math.Sqrt(Variance);
        public double Average { get; private set; }
        public double WeightedAverage { get; private set; }
        public double Min { get; private set; } = double.MaxValue;
        public double Max { get; private set; } = double.MinValue;
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


            Min = Math.Min(Min, other.Min);
            Max = Math.Max(Max, other.Max);
        }

        public override string ToString()
        {
            return ToString("; ");
        }

        public string ToString(string separator, string linePrefix = "")
        {
            return linePrefix + string.Join(separator + linePrefix, GetKeyValues().Select(p => p.Key + ": " + p.Value)) + separator;
        }

        public IEnumerable<(string Key, double Value)> GetKeyValues()
        {
            yield return (nameof(Count), Count);
            yield return (nameof(Min), Min);
            yield return (nameof(Max), Max);
            yield return (nameof(Sum), Sum);
            yield return (nameof(SumSquared), SumSquared);
            yield return (nameof(Average), Average);
            if (Math.Abs(WeightedAverage - Average) > 0.000001)
                yield return (nameof(WeightedAverage), WeightedAverage);
            yield return (nameof(StandardDeviation), StandardDeviation);
            yield return (nameof(Variance), Variance);
        }

        public static IncrementalStatistics FromCsv(CsvRow row, CultureInfo c = null)
        {
            c = c ?? CultureInfo.InvariantCulture;

            var stats = new IncrementalStatistics()
            {
                Sum = row.GetDouble(nameof(Sum), c),
                SumSquared = row.GetDouble(nameof(SumSquared), c),
                Min = row.GetDouble(nameof(Min), c),
                Max = row.GetDouble(nameof(Max), c),
                Count = row.GetInt32(nameof(Count)),
                Average = row.GetDouble(nameof(Average), c),
                Variance = row.GetDouble(nameof(Variance), c)
            };

            if (row.HasHeader(nameof(WeightedAverage)))
            {
                stats.WeightedAverage = row.GetDouble(nameof(WeightedAverage), c);
                stats.WeightSum = stats.WeightedAverage * stats.Count;
            }

            stats.Variance = (stats.SumSquared - 2 * stats.Average * stats.Sum + stats.Count * stats.Average * stats.Average) / stats.Count;

            return stats;
        }

        public static IncrementalStatistics Concatenate(IEnumerable<IncrementalStatistics> stats)
        {
            var sum = new IncrementalStatistics();
            foreach (var s in stats) sum.Append(s);
            return sum;
        }
    }
}
