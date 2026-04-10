namespace net.erlenddahl.ConsoleUtilities.ConsoleCommand
{
    public class ConsoleCommandInterpreter
    {
        public Action<Exception> CommandException { get; set; }
        public List<ConsoleCommand> Commands { get; set; } = new List<ConsoleCommand>();

        public ConsoleCommandInterpreter(params ConsoleCommand[] commands)
        {
            Commands.AddRange(commands);
        }

        /// <summary>
        /// Will auto detect and add all ConsoleCommand implementations from the calling assembly using reflection.
        /// </summary>
        public void AutoDetect()
        {
            var types = System.Reflection.Assembly.GetCallingAssembly().GetTypes()
                .Where(mytype => mytype.IsSubclassOf(typeof(ConsoleCommand)))
                .Select(p => (ConsoleCommand)Activator.CreateInstance(p));
            Commands.AddRange(types.Cast<ConsoleCommand>());
        }

        public void Listen()
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (!RunCommand(line)) break;
            }
        }

        public bool RunCommand(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd))
            {
                Console.WriteLine("Command cannot be empty.");
                return true;
            }

            var arguments = ConsoleCommand.ParseCommand(cmd);
            while (arguments.Any(p => p == "|"))
            {
                var firstArguments = arguments.TakeWhile(p => p != "|").ToList();
                arguments = arguments.Skip(firstArguments.Count + 1).ToList();
                HandleCommand(firstArguments);
            }

            if (HandleCommand(arguments))
                return false;

            return true;
        }

        private bool HandleCommand(List<string> arguments)
        {
            var cmd = arguments[0];
            arguments = arguments.Skip(1).ToList();

            if (cmd == "exit") return true;
            if (cmd == "help")
            {
                if (!arguments.Any())
                    PrintCommands();
                else
                    PrintHelp(arguments.First());
                return false;
            }

            var match = Commands.SingleOrDefault(p => p.Command == cmd);

            if (match != null)
            {
                try
                {
                    match.Run(arguments);
                }
                catch (Exception ex)
                {
                    if (ex is MissingArgumentException || ex is InvalidArgumentException || CommandException == null)
                        Console.WriteLine(ex.Message);
                    else
                        CommandException(ex);
                }
            }
            else Console.WriteLine("Invalid command: " + cmd);

            return false;
        }

        private void PrintHelp(string cmd)
        {
            if (Commands.Count(p => p.Command == cmd) != 1)
            {
                Console.WriteLine("Could not find the command '" + cmd + "'.");
                return;
            }
            Commands.First(p => p.Command == cmd).PrintHelp(true);
        }

        public void PrintCommands()
        {
            Console.WriteLine("--------------------");
            Console.WriteLine("Available commands");
            Console.WriteLine("--------------------");
            foreach (var cmd in Commands)
                cmd.PrintHelp();
            Console.WriteLine("--------------------");
            Console.WriteLine("Type 'help some-command' for more.");
            Console.WriteLine("--------------------");
            Console.WriteLine();
        }
    }
}