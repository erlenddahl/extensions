using System;
using System.Text;
using System.Threading;
using Extensions;
using Extensions.TimeSpanExtensions;

//Source: https://gist.github.com/DanielSWolf/0ab6a96899cc5377bf54
namespace ConsoleUtilities.ConsoleProgressBar
{
    /// <summary>
    /// An ASCII progress bar
    /// </summary>
    public class ConsoleProgressBar : IDisposable, IProgress<double>
    {
        private readonly TimeSpan _animationInterval = TimeSpan.FromSeconds(1.0 / 5);
        private const string Animation = @"|/-\";

        private readonly Timer _timer;

        private double _currentProgress = 0;
        private string _currentText = string.Empty;
        private bool _disposed = false;
        private int _animationIndex = 0;
        private long _max;
        private long _count;
        private int _consoleWidth;
        private DateTime _start;

        public long Max
        {
            get { return _max; }
            set
            {
                Interlocked.Exchange(ref _max, value);
                Report(_count / (double) _max);
            }
        }

        public long Count
        {
            get => _count;
            set
            {
                Interlocked.Exchange(ref _count, value);
                Report(_count / (double) _max);
            }
        }

        public ConsoleProgressBar(string message, long max = -1)
        {
            Console.WriteLine(message);
            _timer = new Timer(TimerHandler);
            _max = max;
            _start = DateTime.Now;

            // A progress bar is only for temporary display in a console window.
            // If the console output is redirected to a file, draw nothing.
            // Otherwise, we'll end up with a lot of garbage in the target file.
            if (!Console.IsOutputRedirected)
            {
                _consoleWidth = Console.WindowWidth;
                ResetTimer();
            }
        }

        public static void TestProgressBar()
        {
            using (var pb = new ConsoleProgressBar("Testing ...", 1000))
            {
                for (var i = 0; i < 1000; i++)
                {
                    Thread.Sleep(10);
                    pb.Increment();
                }
            }
        }

        public void Report(double value)
        {
            // Make sure value is in [0..1] range
            value = Math.Max(0, Math.Min(1, value));
            Interlocked.Exchange(ref _currentProgress, value);

            if (value >= 1.0)
            {
                Finish();
            }
        }

        public void Finish()
        {
            if (_disposed) return;

            if (_currentProgress < 1.0)
            {
                if (_max >= 0)
                    Count = _max;
                else
                    Report(1.0);
            }

            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            TimerHandler(null);
            _disposed = true;
            Console.WriteLine();
        }

        private void TimerHandler(object state)
        {
            if (_timer == null) return;
            lock (_timer)
            {
                if (_disposed) return;

                UpdateText(GetAsciiProgress(_start, _currentProgress, _count, _max, _consoleWidth, _animationIndex++));

                ResetTimer();
            }
        }

        public static string GetAsciiProgress(DateTime? start, double currentProgress, long current = 0, long max = 0, int consoleWidth = 80, int animationIndex = 0, DateTime? end = null)
        {
            var isFinished = end != null;
            if (isFinished)
            {
                currentProgress = 1;
                current = max;
            }
            if (double.IsInfinity(currentProgress) || double.IsNaN(currentProgress)) currentProgress = 0d;
            var percent = (int)(currentProgress * 100);

            var text = $" {percent:n0}%";

            if (max >= 0)
            {
                var progress = current.ToString("n0");
                if (percent < 100)
                    progress += $" / {max:n0}";

                var endTime = isFinished ? end.Value : DateTime.Now;
                var time = start == null ? TimeSpan.Zero : endTime.Subtract(start.Value);

                string timeInfo;
                if(time == TimeSpan.Zero)
                {
                    timeInfo = "";
                }
                else if (current > 0)
                {
                    var timeLeft = new TimeSpan(time.Ticks / current * (max - current));

                    if (percent < 100)
                        timeInfo = $"{time.ToShortPrettyFormat()} / {timeLeft.ToShortPrettyFormat()}";
                    else
                        timeInfo = time.ToShortPrettyFormat();
                }
                else
                    timeInfo = $"{time.ToShortPrettyFormat()}";

                text += $" :: {progress}";
                if(!string.IsNullOrWhiteSpace(timeInfo)) text += $" :: {timeInfo}";
            }

            var blockCount = consoleWidth - text.Length - 5;
            var doneCount = (int)(Math.Min(currentProgress, 1) * blockCount);
            var remainsCount = blockCount - doneCount;

            if (start == null)
                text = $"[{new string(' ', Math.Max(0, doneCount + 1 + remainsCount))}] {text}";
            else if (isFinished)
                text = $"[{new string('#', Math.Max(0, doneCount))}] {text}"; 
            else
                text = $"[{new string('#', Math.Max(0, doneCount))}{Animation[animationIndex % Animation.Length]}{new string('-', Math.Max(remainsCount - 1, 0))}] {text}";

            return text;
        }

        private void UpdateText(string text)
        {
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
                if (_currentProgress < 1.0)
                    Finish();
                _disposed = true;
            }
        }

        public void Increment(int inc = 1)
        {
            Interlocked.Add(ref _count, inc);
            Report(_count / (double)_max);
        }
    }
}