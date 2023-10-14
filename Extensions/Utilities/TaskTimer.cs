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

        public void Time(string key)
        {
            var elapsed = _watch.ElapsedTicks;
            lock (_watch)
            {
                if (Timings.ContainsKey(key))
                    Timings[key] += elapsed;
                else
                    Timings.Add(key, elapsed);
            }
            Restart();
        }

        /// <summary>
        /// Returns a dictionary with the timings measured in milliseconds.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, double> GetTimingsInMs()
        {
            lock (_watch)
            {
                return Timings.ToDictionary(k => k.Key, v => v.Value / 10_000d);
            }
        }

        /// <summary>
        /// Returns timings in milliseconds, one per line.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString("{0}: {1}", Environment.NewLine, 10_000);
        }

        /// <summary>
        /// Returns timings written in the given format, 
        /// </summary>
        /// <param name="keyValueFormat">How the keys and values should be formatted (<code>string.Format(keyValueFormat, key, value)</code>)</param>
        /// <param name="lineSeparator">Separator between lines (or not lines)</param>
        /// <param name="factor">Any conversion factor for the timing. It is originally in ticks, and will be divided by this value. A millisecond is 10 000 ticks, so the default value returns timings in ms.</param>
        /// <param name="reorder">If true, entries will be ordered by their value, descending.</param>
        /// <returns></returns>
        public string ToString(string keyValueFormat = "{0}: {1}", string lineSeparator = null, double factor = 10_000, bool reorder = false)
        {
            lock (_watch)
            {
                var timings = Timings.Select(p => p);
                if (reorder)
                    timings = timings.OrderByDescending(p => p.Value);
                return string.Join(lineSeparator, timings.Select(p => string.Format(keyValueFormat, p.Key, p.Value / factor)));
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
                    if (Timings.ContainsKey(key))
                        Timings[key] += kvp.Value;
                    else
                        Timings.Add(key, kvp.Value);
                }
            }
        }
    }
}