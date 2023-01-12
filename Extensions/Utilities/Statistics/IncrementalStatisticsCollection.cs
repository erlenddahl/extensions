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
        public Dictionary<object[], IncrementalStatistics> Stats { get; set; } = new Dictionary<object[], IncrementalStatistics>(new ArrayEqualityComparer<object>());
        private object _locker = new object();

        public void AddObservation(double observation, params object[] key)
        {
            lock (_locker)
            {
                if (Stats.TryGetValue(key, out var stats))
                    stats.AddObservation(observation);
                else
                {
                    var s = new IncrementalStatistics();
                    s.AddObservation(observation);
                    Stats.Add(key, s);
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var kvp in Stats)
            {
                sb.AppendLine(kvp.Key + ":");
                sb.AppendLine(kvp.Value.ToString(Environment.NewLine, "\t"));
            }

            return sb.ToString();
        }
    }
}