using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Extensions;

namespace ConsoleUtilities.ConsoleProgressBar
{
    public interface ConsoleInfoItem
    {
        string Format(int consoleWidth);
    }

    public class StringInfoItem : ConsoleInfoItem
    {
        public string Value;

        public string Format(int consoleWidth)
        {
            return Value;
        }
    }

    public class IntInfoItem : ConsoleInfoItem
    {
        public int Value;
        public string FormatString = "n0";

        public string Format(int consoleWidth)
        {
            return Value.ToString(FormatString);
        }
    }

    public class DoubleInfoItem : ConsoleInfoItem
    {
        public double Value;
        public string FormatString = "n3";

        public string Format(int consoleWidth)
        {
            return Value.ToString(FormatString);
        }
    }

    public class DateTimeInfoItem : ConsoleInfoItem
    {
        public DateTime Value;
        public string FormatString = "yyyy-MM-dd HH:mm:ss.fff";

        public string Format(int consoleWidth)
        {
            return Value.ToString(FormatString);
        }
    }

    public class ProgressInfoItem : ConsoleInfoItem
    {
        public long Max;
        public long Current;
        private DateTime _start;
        private int _animationIndex = 0;

        public ProgressInfoItem()
        {
            _start = DateTime.Now;
        }

        public string Format(int consoleWidth)
        {
            return ConsoleProgressBar.GetAsciiProgress(_start, Current / (double)Max, Current, Max, consoleWidth, _animationIndex++);
        }
    }

    public class ConsoleInformationPanel : IDisposable
    {

        private readonly TimeSpan _animationInterval = TimeSpan.FromSeconds(1.0 / 2.0);

        private readonly Timer _timer;

        private bool _disposed = false;
        private string _title;
        private int _consoleWidth;
        private DateTime _start;
        
        public Dictionary<string, ConsoleInfoItem> Items = new Dictionary<string,ConsoleInfoItem>();

        public ConsoleInformationPanel(string title)
        {
            _title = title;
            _timer = new Timer(TimerHandler);
            _start = DateTime.Now;

            // A progress bar is only for temporary display in a console window.
            // If the console output is redirected to a file, draw nothing.
            // Otherwise, we'll end up with a lot of garbage in the target file.
            if (!Console.IsOutputRedirected)
            {
                _consoleWidth = Console.WindowWidth;
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
                for (var i = 0; i < 1000; i++)
                {
                    pb.Set("Route consumers", r.Next(20));
                    pb.Set("Result consumers", r.Next(40));
                    pb.Set("Failed routes", r.Next(20000));
                    pb.SetProgress("Processed", i, 1000);
                    pb.SetProgress("Saved", i, i * 2);

                    var now = DateTime.Now;
                    pb.Set("Avg process age", r.NextDouble() * 4000);
                    pb.Set("Avg insertion age", r.NextDouble() * 4000);
                    pb.Set("Max process age", r.NextDouble() * 4000);
                    pb.Set("Max insertion age", r.NextDouble() * 4000);
                    pb.Set("Processing waiting", r.NextDouble() * 4000);
                    pb.Set("Insertion waiting", r.NextDouble() * 4000);
                    pb.Set("Refused connections", r.NextDouble() * 4000);
                    pb.Set("Time", now.ToString("HH:mm:ss.fff"));
                    Thread.Sleep(100);
                }
            }
        }

        public void Finish()
        {
            if (_disposed) return;

            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            TimerHandler(null);
            _disposed = true;
        }

        private void TimerHandler(object state)
        {
            if (_timer == null) return;
            lock (_timer)
            {
                if (_disposed) return;

                var items = new List<string>();

                foreach (var item in Items.Where(p => !(p.Value is ProgressInfoItem)))
                    items.Add(item.Key + ": " + item.Value.Format(_consoleWidth - item.Key.Length - 2));
                
                var sb = new StringBuilder();
                sb.Append("".PadRight(_consoleWidth, '='));
                sb.Append(_title.PadCenter(_consoleWidth));
                sb.Append("".PadRight(_consoleWidth, '='));
                sb.Append("".PadRight(_consoleWidth));

                if (items.Any())
                {
                    var maxWidth = items.Max(p => p.Length) + 6;
                    var lineWidth = 0;
                    var lastWasNewLine = false;
                    foreach (var item in items)
                    {
                        if (lineWidth + 2 * maxWidth >= _consoleWidth)
                        {
                            sb.AppendLine(item);
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
                        sb.AppendLine();
                    sb.Append("".PadRight(_consoleWidth));
                }

                foreach (var item in Items.Where(p => p.Value is ProgressInfoItem))
                    sb.AppendLine(item.Key + ": " + item.Value.Format(_consoleWidth - item.Key.Length - 2));

                UpdateText(sb.ToString());

                ResetTimer();
            }
        }

        private string _currentText = "";
        private void UpdateText(string text)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(text);
            return;//TODO: Fix below
            // Get length of common portion
            var commonPrefixLength = 0;
            var commonLength = Math.Min(_currentText.Length, text.Length);
            while (commonPrefixLength < commonLength && text[commonPrefixLength] == _currentText[commonPrefixLength])
            {
                commonPrefixLength++;
            }

            // Backtrack to the first differing character
            var outputBuilder = new StringBuilder();
            outputBuilder.Append('\b', _currentText.Length - commonPrefixLength);

            // Output new suffix
            outputBuilder.Append(text.Substring(commonPrefixLength));

            // If the new text is shorter than the old one: delete overlapping characters
            int overlapCount = _currentText.Length - text.Length;
            if (overlapCount > 0)
            {
                outputBuilder.Append(' ', overlapCount);
                outputBuilder.Append('\b', overlapCount);
            }

            Console.Write(outputBuilder);
            _currentText = text;
        }

        private void ResetTimer()
        {
            _timer.Change(_animationInterval, TimeSpan.FromMilliseconds(-1));
        }

        public void Dispose()
        {
            lock (_timer)
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
            if (!Items.ContainsKey(key))
            {
                lock(_timer)
                    Items.Add(key, new IntInfoItem() {Value = inc});
            }
            else
            {
                if (Items[key] is IntInfoItem iii)
                    iii.Value += inc;
                else if (Items[key] is ProgressInfoItem pii)
                    pii.Current += inc;
            }
        }

        public void Increment(string key, double inc)
        {
            if (!Items.ContainsKey(key))
            {
                lock (_timer)
                    Items.Add(key, new DoubleInfoItem() {Value = inc});
            }
            else
                ((DoubleInfoItem)Items[key]).Value += inc;
        }

        public void Set(string key, int value)
        {
            if (!Items.ContainsKey(key))
            {
                lock (_timer)
                    Items.Add(key, new IntInfoItem() {Value = value});
            }
            else
                ((IntInfoItem)Items[key]).Value = value;
        }

        public void Set(string key, double value)
        {
            if (!Items.ContainsKey(key))
            {
                lock (_timer)
                    Items.Add(key, new DoubleInfoItem() {Value = value});
            }
            else
                ((DoubleInfoItem)Items[key]).Value = value;
        }

        public void Set(string key, string value)
        {
            if (!Items.ContainsKey(key))
            {
                lock (_timer)
                    Items.Add(key, new StringInfoItem() {Value = value});
            }
            else
                ((StringInfoItem)Items[key]).Value = value;
        }

        public void SetProgress(string key, long? current = null, long? max = null)
        {
            if (!Items.ContainsKey(key))
            {
                var pii = new ProgressInfoItem();
                lock (_timer)
                    Items.Add(key, pii);
                if (current.HasValue) pii.Current = current.Value;
                if (max.HasValue) pii.Max = max.Value;
            }
            else
            {
                var pii = ((ProgressInfoItem) Items[key]);
                if (current.HasValue) pii.Current = current.Value;
                if (max.HasValue) pii.Max = max.Value;
            }
        }
    }
}