using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ConsoleUtilities.ConsoleInfoPanel.ItemBases;
using ConsoleUtilities.ConsoleInfoPanel.Items;
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

                    var fullWidthItems = Items
                        .Where(p => p.Value.FullWidth)
                        .Where(p => !(HideOldProgressBars && p.Value is IHideableItem pii && pii.CanBeHidden))
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

                    var hiddenCompleted = HideOldProgressBars ? Items.Count(p => p.Value is IHideableItem pii && pii.CanBeHidden) : 0;
                    var infoItemLineCount = 0;
                    var maxInfoItemLineCount = Math.Max(availableRows - fullWidthItems.Length - (hiddenCompleted > 0 ? 1 : 0) - (fullWidthItems.Any() ? 1 : 0) - 1, availableRows / 2);
                    if (infoItems.Any())
                    {
                        // Calculate the number of characters in the widest item (including a margin)
                        var itemWidth = infoItems.Max(p => p.Length) + 6;
                        var printedItems = 0;

                        var currentLineWidth = 0;
                        var lastItem = infoItems.Last();

                        // Enumerate all info items, and print them in as many columns as possible.
                        foreach (var item in infoItems)
                        {
                            // If this item is the last item that can fit on the current line (or the
                            // last item in the list), add it to the end of the current line and add
                            // a line break.
                            var thisIsTheLastItem = item == lastItem;
                            if (currentLineWidth + 2 * itemWidth >= consoleWidth || thisIsTheLastItem)
                            {
                                // If the current number of info lines are equal to the maximum number of
                                // info lines, the rest of the info items must be hidden (unless this is
                                // actually the very last info item).
                                if (infoItemLineCount >= maxInfoItemLineCount && !thisIsTheLastItem)
                                {
                                    // Count how many items are remaining, and add a short message about hidden items in the last place.
                                    sb.AppendLine($"[ + {infoItems.Length - printedItems:n0} hidden ]".PadRight(consoleWidth - currentLineWidth - 1));
                                    infoItemLineCount++;
                                    break;
                                }

                                // This is either the very last item, or the last item that can fit on
                                // the current line. Print it, and move to the next line.
                                sb.AppendLine(item.PadRight(consoleWidth - currentLineWidth - 1));
                                currentLineWidth = 0;
                                infoItemLineCount++;
                                printedItems++;
                            }

                            // Otherwise, print this item on the current line, and pad until the
                            // next item should start.
                            else
                            {
                                sb.Append(item.PadRight(itemWidth));
                                currentLineWidth += itemWidth;
                                printedItems++;
                            }
                        }

                        // Add a final empty line to separate the info items from any FullWidth items
                        if (fullWidthItems.Any())
                        {
                            sb.AppendLine("".PadRight(consoleWidth - 1));
                            infoItemLineCount++;
                        }
                    }

                    var removedItems = 0;

                    // If there are any hidden progress bars, there will be a line at the bottom
                    // reporting how many. Subtract this line from the available rows.
                    if (hiddenCompleted > 0) availableRows -= 1;

                    // If there isn't room for all fullWidth items, hide the overflowing items.
                    if (availableRows < infoItemLineCount + fullWidthItems.Length)
                    {
                        var progressCount = fullWidthItems.Length;
                        fullWidthItems = fullWidthItems.Take(availableRows - infoItemLineCount - (hiddenCompleted > 0 ? 0 : 1)).ToArray();
                        removedItems = progressCount - fullWidthItems.Length;
                    }

                    // Print all full width items.
                    foreach (var item in fullWidthItems)
                    {
                        var key = item.Key;
                        if (key.Length > consoleWidth / 3)
                            key = key.Substring(0, consoleWidth / 3) + " [...]";
                        var value = item.Value.Format(consoleWidth - key.Length - 2);
                        AppendLines(sb, key + ": " + value, consoleWidth - 1);
                    }

                    // Print a report line at the bottom summarizing hidden and overflowing full width items.
                    if (hiddenCompleted > 0 || removedItems > 0)
                    {
                        sb.AppendLine(("[ " +
                                      (hiddenCompleted > 0 ? ($"{hiddenCompleted:n0} completed progress bar" + (hiddenCompleted > 1 ? "s" : "")) : "") +
                                      (hiddenCompleted > 0 && removedItems > 0 ? "; " : "") +
                                      (removedItems > 0 ? $"{removedItems:n0} overflowing items" : "") +
                                      " ]").PadCenter(consoleWidth - 1));
                    }

                    // Update the text in the console
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
}