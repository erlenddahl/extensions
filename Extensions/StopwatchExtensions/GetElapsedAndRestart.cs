using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Extensions.StopwatchExtensions
{
    public static class StopwatchExtensions
    {
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
