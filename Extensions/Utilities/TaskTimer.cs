using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Extensions.Utilities
{
    public class TaskTimer
    {
        /// <summary>
        /// The dictionary containing aggregated timings measured in ticks. To get millisecond timings, use the GetTimingsInMs function.
        /// </summary>
        public Dictionary<string, long> Timings = new Dictionary<string, long>();
        private readonly Stopwatch _watch = new Stopwatch();
        private string _prefix = "";
        private TaskTimer _inner;

        public long ElapsedTicks => _watch.ElapsedTicks;
        public long ElapsedMs => _watch.ElapsedMilliseconds;

        public static double MsPerTick { get; } = 1000d / Stopwatch.Frequency;

        public TaskTimer(bool startImmediately = true)
        {
            if (startImmediately) 
                Restart();
        }

        /// <summary>
        /// Restarts the timer to start from 0 now.
        /// </summary>
        public void Restart()
        {
            _watch.Restart();
        }

        /// <summary>
        /// Clears recorded timings.
        /// </summary>
        public void Clear()
        {
            lock (_watch)
            {
                Timings.Clear();
            }
        }

        /// <summary>
        /// Creates a "wrapped" TaskTimer that can be used to both time to an existing timer,
        /// and at the same time keep the timings separate for logging an individual operation.
        /// </summary>
        /// <param name="inner"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static TaskTimer Wrap(TaskTimer inner, string prefix)
        {
            return new TaskTimer(true)
            {
                _prefix = prefix,
                _inner = inner
            };
        }

        public long Time(string key)
        {
            var elapsed = _watch.ElapsedTicks;
            AddManual(key, elapsed);
            return elapsed;
        }

        public void AddManual(string key, long elapsedTicks)
        {
            key = _prefix + key;
            Time(key, elapsedTicks);
            _inner?.Time(key, elapsedTicks);
            _inner?.Restart();
            Restart();
        }

        private void Time(string key, long elapsed)
        {
            lock (_watch)
            {
                if (Timings.ContainsKey(key))
                    Timings[key] += elapsed;
                else
                    Timings.Add(key, elapsed);
            }
        }

        /// <summary>
        /// Adds a totals entry, that is the sum of all entries currently stored in Timings.
        /// </summary>
        /// <param name="name"></param>
        public void AddTotal(string name = "total")
        {
            lock (_watch)
            {
                Timings.Add(name, Timings.Sum(p => p.Value));
            }
        }

        /// <summary>
        /// Returns a dictionary with the timings measured in milliseconds.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, double> GetTimingsInMs()
        {
            lock (_watch)
            {
                return Timings.ToDictionary(k => k.Key, v => v.Value * MsPerTick);
            }
        }

        /// <summary>
        /// Returns timings in milliseconds, one per line.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString("{0}: {1}", Environment.NewLine);
        }

        /// <summary>
        /// Returns timings written in the given format.
        /// Timings are printed as milliseconds.
        /// </summary>
        /// <param name="keyValueFormat">How the keys and values should be formatted (<code>string.Format(keyValueFormat, key, value)</code>)</param>
        /// <param name="lineSeparator">Separator between lines (or not lines)</param>
        /// <param name="reorder">If true, entries will be ordered by their value, descending.</param>
        /// <returns></returns>
        public string ToString(string keyValueFormat, string lineSeparator = null, bool reorder = false)
        {
            lock (_watch)
            {
                var timings = Timings.Select(p => p);
                if (reorder)
                    timings = timings.OrderByDescending(p => p.Value);
                return string.Join(lineSeparator, timings.Select(p => string.Format(keyValueFormat, p.Key, p.Value * MsPerTick)));
            }
        }

        /// <summary>
        /// Appends timings from the given timer to this timer. Any keys that are not in the current timer will be added.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="prefix"></param>
        public void Append(TaskTimer other, string prefix = "")
        {
            lock (_watch)
            {
                foreach (var kvp in other.Timings)
                {
                    var key = prefix + kvp.Key;
                    AddManual(key, kvp.Value);
                }
            }
        }
    }
}