using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Extensions.StringExtensions;

namespace ConsoleUtilities.ConsoleInfoPanel
{
    public class ConsoleInformationPanel : IDisposable
    {

        private readonly TimeSpan _animationInterval = TimeSpan.FromSeconds(1.0 / 2.0);

        private readonly Timer _timer;
        private readonly object _lockObject = new object();

        private bool _disposed = false;
        private string _title;
        private readonly bool _isActive;
        private DateTime _start;
        
        public Dictionary<string, ConsoleInfoItem> Items = new Dictionary<string,ConsoleInfoItem>();

        public ConsoleInformationPanel(string title = "Processing ...", bool isActive = true)
        {
            _title = title;
            _isActive = isActive;
            _timer = new Timer(TimerHandler);
            _start = DateTime.Now;

            // A progress bar is only for temporary display in a console window.
            // If the console output is redirected to a file, draw nothing.
            // Otherwise, we'll end up with a lot of garbage in the target file.
            if (isActive && !Console.IsOutputRedirected)
            {
                ResetTimer();
                Console.Clear();
            }
        }

        public ConsoleInformationPanelSnapshot GetSnapshot()
        {
            return new ConsoleInformationPanelSnapshot(this);
        }

        public static void TestInfoPanel()
        {
            using (var pb = new ConsoleInformationPanel("Testing ..."))
            {
                var r = new Random();
                pb.SetProgress("Current", 0, 1000);
                var unk = pb.SetUnknownProgress("Unknown waiting ...");

                using (var pb2 = pb.SetProgress("Test twice", max: 50))
                {
                    for (var i = 0; i < 50; i++)
                    {
                        Thread.Sleep(100);
                        pb2.Increment();
                    }
                }

                Thread.Sleep(1000);

                using (var pb2 = pb.SetProgress("Test twice", max: 50))
                {
                    for (var i = 0; i < 50; i++)
                    {
                        Thread.Sleep(100);
                        pb2.Increment();
                    }
                }

                for (var i = 0; i < 1000; i++)
                {
                    pb.Set("Route consumers", r.Next(20));
                    pb.Set("Result consumers", r.Next(40));
                    pb.Set("Failed routes", r.Next(20000));
                    pb.SetProgress("Processed", i, 1000);
                    pb.SetProgress("ProcessedFirst", i*10, 1000);
                    pb.SetProgress("Saved", i, i * 2);
                    pb.SetProgress("Not started", started: false);
                    pb.SetProgress("Finished", 0, 10, started: false);
                    pb.FinishProgress("Finished");

                    var now = DateTime.Now;
                    pb.Set("Avg process age", r.NextDouble() * 4000);
                    pb.Set("Avg insertion age", r.NextDouble() * 4000);
                    pb.Set("Max process age", r.NextDouble() * 4000);
                    pb.Set("Max insertion age", r.NextDouble() * 4000);
                    pb.Set("Processing waiting", r.NextDouble() * 4000);
                    pb.Set("Insertion waiting", r.NextDouble() * 4000);
                    pb.Set("Refused connections", r.NextDouble() * 4000);
                    pb.Set("Time", now.ToString("HH:mm:ss.fff"));
                    pb.Set("Exception", string.Join("", Enumerable.Range(0, r.Next(100)).Select(p => "A")), true);
                    Thread.Sleep(100);

                    if(DateTime.Now.Subtract(unk.StartTime).TotalSeconds > 50) unk.Finish();
                }
            }
        }

        public void Finish()
        {
            if (_disposed) return;

            foreach(var item in Items.Values)
                (item as ProgressInfoItem)?.Finish();

            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            TimerHandler(null);
            _disposed = true;
        }

        private void TimerHandler(object state)
        {
            if (_timer == null || !_isActive) return;
            lock (_lockObject)
            {
                if (_disposed) return;

                try
                {
                    var consoleWidth = Console.WindowWidth;

                    if (consoleWidth != _previousConsoleWidth || _hadError)
                    {
                        Console.Clear();
                        _currentText = "";
                        _hadError = false;
                    }

                    _previousConsoleWidth = consoleWidth;

                    var items = new List<string>();

                    foreach (var item in Items.Where(p => !(p.Value.FullWidth)).OrderBy(p => p.Value.Sequence))
                        items.Add(item.Key + ": " + item.Value.Format(consoleWidth - item.Key.Length - 2));

                    var sb = new StringBuilder();
                    sb.Append("".PadRight(consoleWidth, '='));
                    sb.Append(_title.PadCenter(consoleWidth));
                    sb.Append("".PadRight(consoleWidth, '='));
                    sb.Append("".PadRight(consoleWidth));

                    if (items.Any())
                    {
                        var maxWidth = items.Max(p => p.Length) + 6;
                        var lineWidth = 0;
                        var lastWasNewLine = false;
                        foreach (var item in items)
                        {
                            if (lineWidth + 2 * maxWidth >= consoleWidth || item == items.Last())
                            {
                                sb.AppendLine(item.PadRight(consoleWidth - lineWidth - 1));
                                lineWidth = 0;
                                lastWasNewLine = true;
                            }
                            else
                            {
                                sb.Append(item.PadRight(maxWidth));
                                lineWidth += maxWidth;
                                lastWasNewLine = false;
                            }
                        }

                        if (!lastWasNewLine)
                        {
                            sb.AppendLine();
                        }
                    }

                    sb.AppendLine("".PadRight(consoleWidth - 1));

                    foreach (var item in Items.Where(p => p.Value.FullWidth).OrderBy(p => p.Value.Sequence).ThenBy(p => p.GetType()).ThenBy(p => p.Key))
                    {
                        if (item.Value is ProgressInfoItem pii && pii.CanBeHidden) continue;
                        var key = item.Key;
                        if (key.Length > consoleWidth / 3)
                            key = key.Substring(0, consoleWidth / 3) + " [...]";
                        var value = item.Value.Format(consoleWidth - key.Length - 2);
                        AppendLines(sb, key + ": " + value, consoleWidth - 1);
                    }

                    UpdateText(sb);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to update console information. Retrying in a second. (" + ex.Message + ")");
                    _hadError = true;
                }

                ResetTimer();
            }
        }

        private void AppendLines(StringBuilder sb, string value, int padTo)
        {
            var lines = value.Split(Environment.NewLine);
            foreach (var line in lines)
                sb.AppendLine(line.PadRight(padTo));
        }

        private string _currentText = "";
        private int _previousConsoleWidth;
        private bool _hadError = false;

        private void UpdateText(StringBuilder sb)
        {
            Console.SetCursorPosition(0, 0);
            
            // If the new text is shorter than the old one: delete overlapping characters
            var overlapCount = _currentText.Length - sb.Length;
            if (overlapCount > 0)
            {
                sb.Append(' ', overlapCount);
            }

            Console.Write(sb);
            _currentText = sb.ToString();
        }

        private void ResetTimer()
        {
            _timer.Change(_animationInterval, TimeSpan.FromMilliseconds(-1));
        }

        public void Dispose()
        {
            lock (_lockObject)
            {
                Finish();
                _disposed = true;
            }
        }

        public void Increment(string key)
        {
            Increment(key, 1);
        }

        public void Increment(string key, int inc)
        {
            lock (_lockObject)
                if (!Items.ContainsKey(key))
                {
                    Items.Add(key, new IntInfoItem() {Value = inc});
                }
                else
                {
                    if (Items[key] is IntInfoItem iii)
                        iii.Value += inc;
                    else if (Items[key] is ProgressInfoItem pii)
                        pii.Increment(inc);
                }
        }

        public void Increment(string key, double inc)
        {
            lock(_lockObject)
                if (!Items.ContainsKey(key))
                {
                    Items.Add(key, new DoubleInfoItem() {Value = inc});
                }
                else
                    ((DoubleInfoItem) Items[key]).Value += inc;
        }

        public void Set(string key, int value, int? sequence = null)
        {
            lock (_lockObject)
                if (!Items.ContainsKey(key))
                    Items.Add(key, new IntInfoItem() {Value = value, Sequence = sequence ?? 0});
                else
                    ((IntInfoItem) Items[key]).Value = value;
        }

        public void Set(string key, double value, int? sequence = null)
        {
            lock (_lockObject)
                if (!Items.ContainsKey(key))
                    Items.Add(key, new DoubleInfoItem() {Value = value, Sequence = sequence ?? 0});
                else
                    ((DoubleInfoItem) Items[key]).Value = value;
        }

        public void Set(string key, string value, bool fullWidth = false, int? sequence = null)
        {
            lock (_lockObject)
                if (!Items.ContainsKey(key))
                    Items.Add(key, new StringInfoItem() {Value = value, FullWidth = fullWidth, Sequence = sequence ?? 0});
                else
                    ((StringInfoItem) Items[key]).Value = value;
        }

        public AppendableStringInfoItem Log(string key, string value, int? sequence = null)
        {
            lock (_lockObject)
            {
                if (!Items.ContainsKey(key))
                    Items.Add(key, new AppendableStringInfoItem() {Sequence = sequence ?? 0}.AppendLine(value));
                else
                    ((AppendableStringInfoItem) Items[key]).AppendLine(value);
                return (AppendableStringInfoItem) Items[key];
            }
        }

        public UnknownProgressInfoItem SetUnknownProgress(string key, int? sequence = null)
        {
            lock (_lockObject)
                if (!Items.ContainsKey(key))
                {
                    var pii = new UnknownProgressInfoItem();
                    Items.Add(key, pii);
                    pii.Sequence = sequence ?? (Items.Values.Count + 1);
                    return pii;
                }
                else
                {
                    var pii = ((UnknownProgressInfoItem)Items[key]);
                    if (sequence.HasValue) pii.Sequence = sequence.Value;
                    return pii;
                }
        }

        public ProgressInfoItem SetProgress(string key, long? current = null, long? max = null, bool? started = null, int? sequence = null)
        {
            lock (_lockObject)
                if (!Items.ContainsKey(key))
                {
                    var pii = new ProgressInfoItem();
                    Items.Add(key, pii);
                    pii.Set(current, max, started);
                    pii.Sequence = sequence ?? (Items.Values.Count + 1);
                    return pii;
                }
                else
                {
                    var pii = ((ProgressInfoItem) Items[key]);
                    if (pii.EndTime.HasValue)
                    {
                        pii.Current = current ?? 0;
                        pii.EndTime = null;
                        pii.StartTime = DateTime.Now;
                    }
                    pii.Set(current, max, started);
                    if (sequence.HasValue) pii.Sequence = sequence.Value;
                    return pii;
                }
        }

        public void FinishProgress(string key)
        {
            if(Items.TryGetValue(key, out var pii) && pii is ProgressInfoItem item)
                item.Finish();
        }

        public void StartProgress(string key)
        {
            var pii = ((ProgressInfoItem)Items[key]);
            pii.Start();
        }

        public void Remove(string key)
        {
            lock (_lockObject)
                Items.Remove(key);
        }
    }

    public class ConsoleInformationPanelSnapshot
    {
        public Dictionary<string, string> Info { get; set; }
        public Dictionary<string, ProgressSnapshot> Progress { get; set; }

        public ConsoleInformationPanelSnapshot(ConsoleInformationPanel cip)
        {
            Progress = cip.Items
                .Where(p => p.Value is ProgressInfoItem)
                .OrderBy(p => p.Value.Sequence).ToDictionary(k => k.Key, p => new ProgressSnapshot(p.Value as ProgressInfoItem));
            
            Info = cip.Items
                .Where(p => !(p.Value is ProgressInfoItem)).OrderBy(p => p.Value.Sequence)
                .ToDictionary(k => k.Key, v => v.Value.Format(80));
        }
    }

    public class ProgressSnapshot
    {
        public double DurationRemainingS { get; set; }
        public double DurationS { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? StartTime { get; set; }
        public double Percentage { get; set; }
        public long Max { get; set; }
        public long Current { get; set; }
        public string Visualization { get; set; }

        public ProgressSnapshot(ProgressInfoItem pii)
        {
            Current = pii.Current;
            Max = pii.Max;
            Percentage = Current / (double) Max * 100d;
            StartTime = pii.StartTime;
            EndTime = pii.EndTime;

            if (StartTime.HasValue)
                DurationS = (EndTime ?? DateTime.Now).Subtract(StartTime.Value).TotalSeconds;

            DurationRemainingS = EndTime.HasValue ? 0d : DurationS / Current * Max;
            Visualization = pii.Format(80);
        }
    }
}