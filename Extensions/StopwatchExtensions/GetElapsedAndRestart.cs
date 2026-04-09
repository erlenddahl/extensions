using System.Diagnostics;

namespace net.erlenddahl.Extensions.StopwatchExtensions
{
    public static class StopwatchExtensions
    {
        public static Stopwatch StartAndReturn(this Stopwatch stopwatch)
        {
            stopwatch.Start();
            return stopwatch;
        }

        public static long GetElapsedAndRestart(this Stopwatch watch)
        {
            var elapsed = watch.ElapsedMilliseconds;
            watch.Restart();
            return elapsed;
        }

        public static long GetElapsedTicksAndRestart(this Stopwatch watch)
        {
            var elapsed = watch.ElapsedTicks;
            watch.Restart();
            return elapsed;
        }
    }
}
