using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Markup;
using Extensions;

namespace ConsoleUtilities.ConsoleProgressBar
{
    public abstract class ConsoleInfoItem
    {
        public bool FullWidth { get; set; }
        public abstract string Format(int consoleWidth);
        public int Sequence { get; set; }
    }

    public interface IHideableItem
    {
        bool CanBeHidden { get; }
    }

    public class StringInfoItem : ConsoleInfoItem
    {
        public string Value;

        public override string Format(int consoleWidth)
        {
            return Value;
        }
    }

    public class AppendableStringInfoItem : ConsoleInfoItem
    {
        public string Value;

        public AppendableStringInfoItem()
        {
            FullWidth = true;
        }

        public AppendableStringInfoItem AppendLine(string msg, bool prependTimestamp = true, string timestampFormat = "yyyy-MM-dd HH:mm:ss.fff")
        {
            Value += Environment.NewLine + (prependTimestamp ? DateTime.Now.ToString(timestampFormat) + ": " : "") + msg;
            return this;
        }

        public override string Format(int consoleWidth)
        {
            return Value;
        }
    }

    public class IntInfoItem : ConsoleInfoItem
    {
        public int Value;
        public string FormatString = "n0";

        public override string Format(int consoleWidth)
        {
            return Value.ToString(FormatString);
        }
    }

    public class DoubleInfoItem : ConsoleInfoItem
    {
        public double Value;
        public string FormatString = "n3";

        public override string Format(int consoleWidth)
        {
            return Value.ToString(FormatString);
        }
    }

    public class DateTimeInfoItem : ConsoleInfoItem
    {
        public DateTime Value;
        public string FormatString = "yyyy-MM-dd HH:mm:ss.fff";

        public override string Format(int consoleWidth)
        {
            return Value.ToString(FormatString);
        }
    }

    public class ProgressInfoItem : ConsoleInfoItem, IDisposable, IHideableItem
    {
        public long Max;
        public long Current;

        public DateTime? StartTime
        {
            get => _start;
            set
            {
                _start = value;
                if (!_end.HasValue)
                    _end = null;
            }
        }

        public DateTime? EndTime
        {
            get => _end;
            set
            {
                _end = value;
                if (!_start.HasValue)
                    _start = value;
            }
        }

        public bool CanBeHidden => EndTime != null && DateTime.Now.Subtract(EndTime.Value).TotalSeconds > 15;

        private int _animationIndex = 0;
        private DateTime? _end;
        private DateTime? _start;

        public ProgressInfoItem()
        {
            StartTime = DateTime.Now;
            FullWidth = true;
        }

        public override string Format(int consoleWidth)
        {
            return ConsoleProgressBar.GetAsciiProgress(StartTime, Current / (double)Max, Current, Max, consoleWidth, _animationIndex++, EndTime);
        }

        public void Set(long? current = null, long? max = null, bool? started = null)
        {
            if (current.HasValue) Current = current.Value;
            if (max.HasValue) Max = max.Value;
            if (started.HasValue) StartTime = started.Value ? DateTime.Now : (DateTime?)null;
        }

        public void Start()
        {
            StartTime = DateTime.Now;
        }

        public void Finish()
        {
            if (EndTime.HasValue) return;
            EndTime = DateTime.Now;
            Current = Max;
        }

        private object _lock = new object();
        public void Increment(int inc = 1)
        {
            lock (_lock)
            {
                Current += inc;
            }
        }
        public void IncrementMax(int inc = 1)
        {
            lock (_lock)
            {
                Max += inc;
            }
        }

        public void Dispose()
        {
            Finish();
        }
    }

    public class UnknownProgressInfoItem : ConsoleInfoItem, IDisposable, IHideableItem
    {
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public bool CanBeHidden => EndTime != null && DateTime.Now.Subtract(EndTime.Value).TotalSeconds > 15;

        private int _animationIndex = 0;
        private int _animationDirection = 1;

        public UnknownProgressInfoItem()
        {
            StartTime = DateTime.Now;
            FullWidth = true;
        }

        public override string Format(int consoleWidth)
        {
            var isFinished = EndTime != null;

            var endTime = isFinished ? EndTime.Value : DateTime.Now;
            var time = endTime.Subtract(StartTime);

            var text = $" :: {time.ToShortPrettyFormat()}";

            var blockCount = consoleWidth - text.Length - 5;

            var animationWidth = Math.Min(1, blockCount / 2);

            if (isFinished)
                text = $"[{new string('#', blockCount)}] {text}";
            else
            {
                _animationIndex += _animationDirection * 2;
                if (_animationIndex + animationWidth >= blockCount)
                {
                    _animationDirection = -1;
                    _animationIndex = blockCount - animationWidth;
                }else if (_animationIndex <= 0)
                {
                    _animationDirection = 1;
                    _animationIndex = 0;
                }

                text = $"[{new string(' ', _animationIndex)}{new string('-', animationWidth)}{new string(' ', blockCount - (_animationIndex + animationWidth))}] {text}";
            }

            return text;
        }

        public void Finish()
        {
            if (EndTime.HasValue) return;
            EndTime = DateTime.Now;
        }

        public void Dispose()
        {
            Finish();
        }
    }

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

        public static void TestInfoPanel()
        {
            using (var pb = new ConsoleInformationPanel("Testing ..."))
            {
                var r = new Random();
                pb.SetProgress("Current", 0, 1000);
                var unk = pb.SetUnknownProgress("Unknown waiting ...");
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
                        var value = item.Value.Format(consoleWidth - item.Key.Length - 2);
                        AppendLines(sb, item.Key + ": " + value, consoleWidth - 1);
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
}