using System;
using ConsoleUtilities.ConsoleInfoPanel.ItemBases;

namespace ConsoleUtilities.ConsoleInfoPanel.Items
{
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
}