using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions.Utilities.Statistics
{
    public class IncrementalStatisticsCollection
    {
        public Dictionary<string, IncrementalStatistics> Stats { get; set; } = new Dictionary<string, IncrementalStatistics>();
        private object _locker = new object();

        public void AddObservation(double observation, params object[] keyParts)
        {
            var key = string.Join("_", keyParts.Select(p => p.ToString()));

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