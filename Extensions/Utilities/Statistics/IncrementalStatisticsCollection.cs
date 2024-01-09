using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Extensions.Utilities.Csv;
using Extensions.Utilities.EqualityComparers;

namespace Extensions.Utilities.Statistics
{
    public class IncrementalStatisticsCollection
    {
        private readonly int? _countInBucketsOfSize;
        public Dictionary<object[], IncrementalStatistics> Stats { get; set; } = new Dictionary<object[], IncrementalStatistics>(new ArrayEqualityComparer<object>());
        private object _locker = new object();

        public IncrementalStatisticsCollection(int? countInBucketsOfSize = null)
        {
            _countInBucketsOfSize = countInBucketsOfSize;
        }

        public void AddObservation(double observation, params object[] key)
        {
            AddObservation(observation, null, key);
        }

        public void AddObservation(double observation, int? countInBucketsOfSize, params object[] key)
        {
            lock (_locker)
            {
                if (Stats.TryGetValue(key, out var stats))
                    stats.AddObservation(observation);
                else
                {
                    var s = new IncrementalStatistics(countInBucketsOfSize ?? _countInBucketsOfSize);
                    s.AddObservation(observation);
                    Stats.Add(key, s);
                }
            }
        }

        public override string ToString()
        {
            return ToString("; ");
        }

        public string ToString(string separator, string linePrefix = "")
        {
            var sb = new StringBuilder();
            foreach (var kvp in Stats)
            {
                sb.AppendLine(string.Join(", ", kvp.Key.Select(p => p.ToString())) + ":");
                sb.AppendLine(kvp.Value.ToString(separator, linePrefix));
            }

            return sb.ToString();
        }

        public void WriteCsv(string targetFile, params string[] keyTitles)
        {
            using (var csv = new CsvWriter(targetFile, ";"))
                WriteCsv(csv, keyTitles);
        }

        public void WriteCsv(CsvWriter csv, params string[] keyTitles)
        {
            WriteCsv(csv, CultureInfo.InvariantCulture, keyTitles);
        }

        public void WriteCsv(CsvWriter csv, CultureInfo c, params string[] keyTitles)
        {
            if (_countInBucketsOfSize.HasValue) throw new Exception("Statistics with histograms cannot currently be saved as CSV.");
            csv.WriteLine(keyTitles.Concat(Stats.First().Value.GetKeyValues().Select(p => p.Key)));
            foreach (var kvp in Stats)
            {
                csv.WriteLine(kvp.Key.Select(p => p.ToString()).Concat(kvp.Value.GetKeyValues().Select(p => p.Value.ToString(c))));
            }
        }

        public static IncrementalStatisticsCollection FromCsv(string file, CultureInfo c = null, Func<int, string, object> keyParserFunc = null)
        {
            return FromCsv(new CsvReader(), file, c, keyParserFunc);
        }

        public static IncrementalStatisticsCollection FromCsv(CsvReader reader, string file, CultureInfo c = null, Func<int, string, object> keyParserFunc = null)
        {
            c = c ?? CultureInfo.InvariantCulture;
            keyParserFunc = keyParserFunc ?? ((i, v) => v);

            var headers = reader.ReadHeaders(file);
            foreach (var statsHeader in new IncrementalStatistics().GetKeyValues().Select(p => p.Key))
                headers.Remove(statsHeader);
            var keyHeaderIndices = headers.Select(p => p.Value).OrderBy(p => p).ToArray();

            var coll = new IncrementalStatisticsCollection();

            foreach (var row in reader.ReadFile(file))
            {
                try
                {
                    var stats = IncrementalStatistics.FromCsv(row, c);
                    coll.Stats.Add(keyHeaderIndices.Select(p => keyParserFunc(p, row[p])).ToArray(), stats);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to parse row with headers=[{string.Join("; ", row.Headers)}] and values=[{string.Join("; ", row.Raw)}]", ex);
                }
            }

            return coll;
        }
    }
}