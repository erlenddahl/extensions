using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using net.erlenddahl.Extensions.DateTimeExtensions;
using net.erlenddahl.Extensions.Utilities.Csv;

namespace net.erlenddahl.Extensions.Utilities.Statistics
{
    public class IncrementalStatistics
    {
        private double _variance;

        /// <summary>
        /// If configured when creating the object (using the countInBucketsOfSize parameter), these buckets will represent
        /// a histogram of the observed values.
        /// </summary>
        public Dictionary<long, double> Buckets { get; }

        public long? BucketSize { get; }

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
        public long Count { get; private set; }

        public IncrementalStatistics(long? countInBucketsOfSize = null)
        {
            if (countInBucketsOfSize.HasValue)
            {
                Buckets = new Dictionary<long, double>();
                BucketSize = countInBucketsOfSize.Value;
            }
        }

        public IncrementalStatistics(IEnumerable<double> values, long? countInBucketsOfSize = null) : this(countInBucketsOfSize)
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

            Buckets?.Increment(observation.Round(BucketSize.Value, RoundingDirection.Down), weight);

            if (Count == 1)
            {
                WeightedAverage = Average = Min = Max = observation;
                return;
            }

            Average = Sum / Count;
            WeightedAverage = Sum / WeightSum;

            Variance = (SumSquared - 2 * Average * Sum + Count * Average * Average) / Count;
            Min = Math.Min(Min, observation);
            Max = Math.Max(Max, observation);
        }

        public void Append(IncrementalStatistics other)
        {
            if (BucketSize != other.BucketSize)
                throw new Exception("Incompatible bucket sizes. IncrementalStatistics can only be merged if they have the same bucket size configuration.");

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

            if (other.Buckets != null)
            {
                foreach (var kvp in other.Buckets)
                    Buckets.Increment(kvp.Key, kvp.Value);
            }
        }

        public override string ToString()
        {
            return ToString("; ");
        }

        public string ToString(string separator, string linePrefix = "")
        {
            return linePrefix + string.Join(separator + linePrefix, GetKeyValues().Select(p => p.Key + ": " + p.Value)) + separator;
        }

        public string PrettyPrint(string header, string separator = "\r\n", string linePrefix = "\t")
        {
            return header + Environment.NewLine + ToString(separator, linePrefix);
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

        public IEnumerable<(long x, double y)> BucketsCutOff(double cutoffPercentage = 95)
        {
            // Calculate the total count
            var totalCount = Buckets.Values.Sum();

            // Calculate the cumulative threshold
            var threshold = (long)(totalCount * (cutoffPercentage / 100.0));

            // Sort the dictionary by key (x value)
            var sortedData = Buckets.OrderBy(kvp => kvp.Key);

            var cumulativeCount = 0d;
            var lastXValue = 0L;
            foreach (var kvp in sortedData)
            {
                cumulativeCount += kvp.Value;
                if (cumulativeCount >= threshold)
                {
                    // Once we hit the threshold, we stop and add the "x+" bucket
                    yield return (lastXValue + 1, sortedData.Where(d => d.Key > lastXValue).Sum(d => d.Value));
                    yield break; // Stop the iteration
                }

                yield return (kvp.Key, kvp.Value);
                lastXValue = kvp.Key;
            }

            // In case the threshold is never reached (e.g., cutoffPercentage is set very high),
            // return all data without grouping into "x+"
        }

        public IEnumerable<(long x, double y)> BucketsMinMax(long min = long.MinValue, long max = long.MaxValue)
        {
            return Buckets.GroupBy(p =>
            {
                if (p.Key <= min) return min;
                if (p.Key >= max) return max;
                return p.Key;
            }).Select(p => (p.Key, p.Sum(c => c.Value)));
        }
    }
}
