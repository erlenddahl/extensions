using System.Text.RegularExpressions;

namespace net.erlenddahl.ConsoleUtilities.ConsoleCommand
{
    public abstract class ConsoleCommand
    {
        public string Command;
        public string Description;

        private List<ConsoleCommandParameter> _parameters = new List<ConsoleCommandParameter>();

        public ConsoleCommand(string command)
        {
            Command = command;
        }

        protected void AddParameter(string name, ConsoleCommandParameterType type)
        {
            _parameters.Add(new ConsoleCommandParameter(_parameters.Count, name, type));
        }

        private int FindParameter(List<string> pars, string name)
        {
            var par = _parameters.Single(p => p.Name == name);
            if (par.Index >= pars.Count) throw new MissingArgumentException("Missing argument: " + name);
            return par.Index;
        }

        public int GetInt(List<string> pars, string name)
        {
            var index = FindParameter(pars, name);
            if (int.TryParse(pars[index], out var res))
                return res;
            throw new InvalidArgumentException("Argument " + name + " must be an integer.");
        }

        public double GetDouble(List<string> pars, string name)
        {
            var index = FindParameter(pars, name);
            if (double.TryParse(pars[index], out var res))
                return res;
            throw new InvalidArgumentException("Argument " + name + " must be a double.");
        }

        public string GetString(List<string> pars, string name)
        {
            var index = FindParameter(pars, name);
            return pars[index];
        }

        public bool GetBoolean(List<string> pars, string name)
        {
            var s = GetString(pars, name).ToLower();
            return s == "t" || s == "y" || s == "yes" || s == "true";
        }

        public int[] GetIntArray(List<string> pars, string name)
        {
            var index = FindParameter(pars, name);
            var array = pars[index].Split(',');
            var list = new List<int>();
            foreach (var item in array)
            {
                if (int.TryParse(item, out var res))
                    list.Add(res);
                else
                    throw new InvalidArgumentException("Argument " + name + " must be an array of integers (comma separated without spaces).");
            }

            return list.ToArray();
        }

        public abstract void Run(List<string> parameters);

        public virtual void PrintHelp(bool details = false)
        {
            if (!details)
            {
                Console.WriteLine("    " + Command + " " + string.Join(" ", _parameters.Select(p => p.Name + ":" + p.Type)));
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Command:");
            Console.WriteLine("    " + Command);
            Console.WriteLine();

            Console.WriteLine("Parameters:");
            foreach (var par in _parameters)
            {
                Console.WriteLine("    " + par.Name + ", " + par.Type);
                ConsoleEx.WritePaddedLine(par.TypeDescription, 8);
            }
            if (!_parameters.Any())
                Console.WriteLine("    [No parameters]");

            Console.WriteLine();

            if (!string.IsNullOrWhiteSpace(Description))
            {
                Console.WriteLine("Description:");
                ConsoleEx.WritePaddedLine(Description, 4);
                Console.WriteLine();
            }
        }

        public static List<string> ParseCommand(string command)
        {
            return Regex.Matches(command, @"[\""].+?[\""]|[^ ]+")
                .Cast<Match>()
                .Select(m => m.Value.Trim('"'))
                .ToList();
        }
    }
}