using System;
using ConsoleUtilities.ConsoleInfoPanel.ItemBases;

namespace ConsoleUtilities.ConsoleInfoPanel.Items
{
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

        public bool CanBeHidden => EndTime != null && DateTime.Now.Subtract(EndTime.Value).TotalSeconds > 5;

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
            return ConsoleProgressBar.ConsoleProgressBar.GetAsciiProgress(StartTime, Current / (double)Max, Current, Max, consoleWidth, _animationIndex++, EndTime);
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
}