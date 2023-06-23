using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleUtilities.ConsoleInfoPanel
{
    public class ConsoleInformationPanelTests
    {
        public static void TestInfoPanelLargeNumberOfProgressbars()
        {
            var r = new Random();
            using (var cip = new ConsoleInformationPanel())
            {
                Enumerable.Range(0, 80).AsParallel().WithDegreeOfParallelism(100).ForAll(p =>
                {
                    Thread.Sleep(r.Next(10_000));
                    using (var pb = cip.SetProgress("Progressbar " + p, max: 100))
                    {
                        for (var i = 0; i < 100; i++)
                        {
                            Thread.Sleep(r.Next(500));
                            pb.Increment();
                        }
                    }
                });
            }
        }

        public static void TestInfoPanelLargeNumberOfProgressbarsAndInfoItems()
        {
            var r = new Random();
            using (var cip = new ConsoleInformationPanel())
            {
                Enumerable.Range(0, 160).AsParallel().WithDegreeOfParallelism(100).ForAll(p =>
                {
                    Thread.Sleep(r.Next(10_000));
                    using (var pb = cip.SetProgress("Progressbar " + p, max: 100))
                    {
                        for (var i = 0; i < 100; i++)
                        {
                            Thread.Sleep(r.Next(500));
                            pb.Increment();
                            cip.Set("Info item " + p, i);
                        }
                    }
                });
            }
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
                    pb.SetProgress("ProcessedFirst", i * 10, 1000);
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

                    if (DateTime.Now.Subtract(unk.StartTime).TotalSeconds > 50) unk.Finish();
                }
            }
        }
    }
}
