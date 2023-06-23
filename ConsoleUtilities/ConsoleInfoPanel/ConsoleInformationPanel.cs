using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public bool HideOldProgressBars { get; set; } = true;

        public ConsoleInformationPanel(string title = "Processing ...", bool isActive = true)
        {
            _title = title;
            _isActive = isActive;
            _timer = new Timer(DrawPanel);
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

        public void Finish()
        {
            if (_disposed) return;

            foreach(var item in Items.Values)
                (item as ProgressInfoItem)?.Finish();

            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            DrawPanel(null);
            _disposed = true;
        }

        private void DrawPanel(object state)
        {
            if (_timer == null || !_isActive) return;
            lock (_lockObject)
            {
                if (_disposed) return;

                try
                {
                    var consoleWidth = Console.WindowWidth;
                    var availableRows = Console.WindowHeight - 1;

                    if (consoleWidth != _previousConsoleWidth || availableRows != _previousConsoleHeight || _hadError)
                    {
                        Console.Clear();
                        _currentText = "";
                        _hadError = false;
                    }

                    _previousConsoleWidth = consoleWidth;
                    _previousConsoleHeight = availableRows;

                    var infoItems = Items
                        .Where(p => !(p.Value.FullWidth))
                        .OrderBy(p => p.Value.Sequence)
                        .Select(p => p.Key + ": " + p.Value.Format(consoleWidth - p.Key.Length - 2))
                        .ToArray();

                    var progressItems = Items
                        .Where(p => p.Value.FullWidth)
                        .Where(p => !(HideOldProgressBars && p.Value is ProgressInfoItem pii && pii.CanBeHidden))
                        .OrderBy(p => p.Value.Sequence)
                        .ThenBy(p => p.GetType())
                        .ThenBy(p => p.Key)
                        .ToArray();

                    var sb = new StringBuilder();
                    sb.Append("".PadRight(consoleWidth, '='));
                    sb.Append(_title.PadCenter(consoleWidth));
                    sb.Append("".PadRight(consoleWidth, '='));
                    sb.Append("".PadRight(consoleWidth));

                    availableRows -= 4;
                    var removedItems = 0;
                    var infoItemCount = infoItems.Any() ? infoItems.Length + 1 : 0;
                    var hiddenCompleted = HideOldProgressBars ? Items.Count(p => p.Value is ProgressInfoItem pii && pii.CanBeHidden) : 0;

                    if (availableRows < infoItemCount + progressItems.Length)
                    {
                        var progressCount = progressItems.Length;
                        progressItems = progressItems.Take(availableRows - infoItemCount - 1).ToArray();
                        removedItems = progressCount - progressItems.Length;
                    }

                    if (infoItems.Any())
                    {
                        var maxWidth = infoItems.Max(p => p.Length) + 6;
                        var lineWidth = 0;
                        var lastWasNewLine = false;
                        foreach (var item in infoItems)
                        {
                            if (lineWidth + 2 * maxWidth >= consoleWidth || item == infoItems.Last())
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

                        sb.AppendLine("".PadRight(consoleWidth - 1));
                    }

                    foreach (var item in progressItems)
                    {
                        var key = item.Key;
                        if (key.Length > consoleWidth / 3)
                            key = key.Substring(0, consoleWidth / 3) + " [...]";
                        var value = item.Value.Format(consoleWidth - key.Length - 2);
                        AppendLines(sb, key + ": " + value, consoleWidth - 1);
                    }

                    if (hiddenCompleted > 0 || removedItems > 0)
                    {
                        sb.AppendLine(("[ " +
                                      (hiddenCompleted > 0 ? $"{hiddenCompleted:n0} completed progress bar(s)" : "") +
                                      (hiddenCompleted > 0 && removedItems > 0 ? "; " : "") +
                                      (removedItems > 0 ? $"{removedItems:n0} overflowing items" : "") +
                                      " ]").PadCenter(consoleWidth - 1));
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


        private int AppendLines(StringBuilder sb, string value, int padTo)
        {
            var lines = value.Split(Environment.NewLine);
            foreach (var line in lines)
                sb.AppendLine(line.PadRight(padTo));
            return lines.Length;
        }

        private string _currentText = "";
        private int _previousConsoleWidth;
        private int _previousConsoleHeight;
        private bool _hadError = false;

        private void UpdateText(StringBuilder sb)
        {
            Console.SetCursorPosition(0, 0);
            
            // If the new text is shorter than the old one: delete overlapping characters
            var overlapCount = _currentText.Length - sb.Length;
            _currentText = sb.ToString();
            if (overlapCount > 0)
            {
                sb.Append(' ', overlapCount);
            }

            Console.Write(sb);
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

        public IntInfoItem GetOrCreate(string key, int defaultValue)
        {
            lock (_lockObject)
            {
                if (!Items.TryGetValue(key, out var item))
                {
                    item = new IntInfoItem() { Value = defaultValue };
                    Items.Add(key, item);
                }
                return (IntInfoItem)item;
            }
        }

        public LongInfoItem GetOrCreate(string key, long defaultValue)
        {
            lock (_lockObject)
            {
                if (!Items.TryGetValue(key, out var item))
                {
                    item = new LongInfoItem() { Value = defaultValue };
                    Items.Add(key, item);
                }
                return (LongInfoItem)item;
            }
        }

        public DoubleInfoItem GetOrCreate(string key, double defaultValue)
        {
            lock (_lockObject)
            {
                if (!Items.TryGetValue(key, out var item))
                {
                    item = new DoubleInfoItem() { Value = defaultValue };
                    Items.Add(key, item);
                }
                return (DoubleInfoItem)item;
            }
        }

        public void Increment(string key, int inc)
        {
            lock (_lockObject)
                if (Items.TryGetValue(key, out var value))
                {
                    switch (value)
                    {
                        case IntInfoItem item:
                            item.Value += inc;
                            break;
                        case LongInfoItem item:
                            item.Value += inc;
                            break;
                        case DoubleInfoItem item:
                            item.Value += inc;
                            break;
                        case ProgressInfoItem item:
                            item.Increment(inc);
                            break;
                    }
                }
                else
                {
                    Items.Add(key, new IntInfoItem() { Value = inc });
                }
        }

        public void Increment(string key, long inc)
        {
            lock (_lockObject)
                if (Items.TryGetValue(key, out var value))
                {
                    switch (value)
                    {
                        case LongInfoItem item:
                            item.Value += inc;
                            break;
                        case DoubleInfoItem item:
                            item.Value += inc;
                            break;
                    }
                }
                else
                {
                    Items.Add(key, new LongInfoItem() { Value = inc });
                }
        }

        public void Increment(string key, double inc)
        {
            lock (_lockObject)
                if (Items.TryGetValue(key, out var value))
                {
                    switch (value)
                    {
                        case DoubleInfoItem item:
                            item.Value += inc;
                            break;
                    }
                }
                else
                {
                    Items.Add(key, new DoubleInfoItem() { Value = inc });
                }
        }

        public void Max(string key, int newValue)
        {
            lock (_lockObject)
                if (Items.TryGetValue(key, out var value))
                {
                    switch (value)
                    {
                        case IntInfoItem item:
                            item.Value = Math.Max(item.Value, newValue);
                            break;
                        case LongInfoItem item:
                            item.Value = Math.Max(item.Value, newValue);
                            break;
                        case DoubleInfoItem item:
                            item.Value = Math.Max(item.Value, newValue);
                            break;
                    }
                }
                else
                {
                    Items.Add(key, new IntInfoItem() { Value = newValue });
                }
        }

        public void Max(string key, long newValue)
        {
            lock (_lockObject)
                if (Items.TryGetValue(key, out var value))
                {
                    switch (value)
                    {
                        case LongInfoItem item:
                            item.Value = Math.Max(item.Value, newValue);
                            break;
                        case DoubleInfoItem item:
                            item.Value = Math.Max(item.Value, newValue);
                            break;
                    }
                }
                else
                {
                    Items.Add(key, new LongInfoItem() { Value = newValue });
                }
        }

        public void Max(string key, double newValue)
        {
            lock (_lockObject)
                if (Items.TryGetValue(key, out var value))
                {
                    switch (value)
                    {
                        case DoubleInfoItem item:
                            item.Value = Math.Max(item.Value, newValue);
                            break;
                    }
                }
                else
                {
                    Items.Add(key, new DoubleInfoItem() { Value = newValue });
                }
        }

        public void Set(string key, int value, int? sequence = null)
        {
            lock (_lockObject)
                if (Items.TryGetValue(key, out var item))
                {
                    ((IntInfoItem)item).Value = value;
                }
                else
                {
                    Items.Add(key, new IntInfoItem() { Value = value, Sequence = sequence ?? 0 });
                }
        }

        public void Set(string key, long value, int? sequence = null)
        {
            lock (_lockObject)
                if (Items.TryGetValue(key, out var item))
                {
                    ((LongInfoItem)item).Value = value;
                }
                else
                {
                    Items.Add(key, new LongInfoItem() { Value = value, Sequence = sequence ?? 0 });
                }
        }

        public void Set(string key, double value, int? sequence = null)
        {
            lock (_lockObject)
                if (Items.TryGetValue(key, out var item))
                {
                    ((DoubleInfoItem)item).Value = value;
                }
                else
                {
                    Items.Add(key, new DoubleInfoItem() { Value = value, Sequence = sequence ?? 0 });
                }
        }

        public void Set(string key, string value, bool fullWidth = false, int? sequence = null)
        {
            lock (_lockObject)
                if (Items.TryGetValue(key, out var item))
                {
                    ((StringInfoItem)item).Value = value;
                }
                else
                {
                    Items.Add(key, new StringInfoItem() { Value = value, FullWidth = fullWidth, Sequence = sequence ?? 0 });
                }
        }

        public AppendableStringInfoItem Log(string key, string value, int? sequence = null)
        {
            lock (_lockObject)
            {
                if (Items.TryGetValue(key, out var item))
                {
                    var casted = (AppendableStringInfoItem)item;
                    casted.AppendLine(value);
                    return casted;
                }
                else
                {
                    var casted = new AppendableStringInfoItem() { Sequence = sequence ?? 0 }.AppendLine(value);
                    Items.Add(key, casted);
                    return casted;
                }
            }
        }

        public UnknownProgressInfoItem SetUnknownProgress(string key, int? sequence = null)
        {
            lock (_lockObject)
                if (Items.TryGetValue(key, out var progressItem))
                {
                    var pii = ((UnknownProgressInfoItem)progressItem);
                    if (sequence.HasValue) pii.Sequence = sequence.Value;
                    pii.EndTime = null;
                    return pii;
                }
                else
                {
                    var pii = new UnknownProgressInfoItem();
                    Items.Add(key, pii);
                    pii.Sequence = sequence ?? (Items.Values.Count + 1);
                    return pii;
                }
        }

        public ProgressInfoItem SetProgress(string key, long? current = null, long? max = null, bool? started = null, int? sequence = null)
        {
            lock (_lockObject)
                if (Items.TryGetValue(key, out var progressItem))
                {
                    var pii = ((ProgressInfoItem)progressItem);
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
                else
                {
                    var pii = new ProgressInfoItem();
                    Items.Add(key, pii);
                    pii.Set(current, max, started);
                    pii.Sequence = sequence ?? (Items.Values.Count + 1);
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

        public CipTimer Time(string key)
        {
            return new CipTimer(this, key);
        }
    }

    public class CipTimer : IDisposable
    {
        private readonly ConsoleInformationPanel _cip;
        private readonly string _key;
        private readonly Stopwatch _sw;

        public CipTimer(ConsoleInformationPanel cip, string key)
        {
            _cip = cip;
            _key = key;
            _sw = new Stopwatch();
        }

        public void Dispose()
        {
            var elapsed = _sw.ElapsedMilliseconds;
            _cip.Increment(_key, elapsed);
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