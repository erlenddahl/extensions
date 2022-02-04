using System;
using System.Linq;

namespace ConsoleUtilities
{
    public class ConsoleConfigHelper
    {
        public string[] Files { get; private set; }
        public IRunnable[] Configs { get; private set; }
        public Exception[] Exceptions { get; private set; }
        public bool HasExceptions => Exceptions.Any(p => p != null);
        public string EmptyMessage { get; set; } = "The application must be run with one or more config files (.json) as command line arguments, which will invoke the relevant command for each file.";

        public ConsoleConfigHelper(string[] args)
        {
            Files = args;
        }

        public ConsoleConfigHelper AutoResize()
        {
            SetWindowSize(180, 40);
            return this;
        }

        public static void SetWindowSize(int width, int height, bool throwException = false)
        {
            try
            {
                Console.BufferWidth = Math.Min(Console.LargestWindowWidth, width);
                Console.WindowWidth = Math.Min(Console.LargestWindowWidth, width);
                Console.BufferHeight = Math.Min(Console.LargestWindowHeight, 40);
                Console.WindowHeight = Math.Min(Console.LargestWindowHeight, 40);
            }
            catch (Exception ex)
            {
                if (throwException) throw;
            }
        }

        public ConsoleConfigHelper Run(string title, Func<string, IRunnable> parseFunc)
        {
            if (!Files.Any())
            {
                Console.WriteLine(EmptyMessage);
            }
            else
            {
                Configs = Files.Select(parseFunc).ToArray();
                Exceptions = new Exception[Files.Length];

                for (var i = 0; i < Files.Length; i++)
                {
                    Console.Title = GetRunPrefix(i) + title;
                    try
                    {
                        Configs[i].Run();
                    }
                    catch (Exception ex)
                    {
                        Exceptions[i] = ex;
                    }
                }
            }

            return this;
        }

        private string GetRunPrefix(int i)
        {
            if (Files.Length < 2) return "";
            return "[ " + i + " / " + Files.Length + " ] ";
        }

        public ConsoleConfigHelper PrintSummary()
        {
            for (var i = 0; i < Files.Length; i++)
            {
                Console.WriteLine(Files[i]);
                if (Exceptions[i] == null)
                    Console.WriteLine("    > Ran to completion");
                else
                    Console.WriteLine("    > Failed: " + Exceptions[i].Message);
            }

            return this;
        }
    }
}