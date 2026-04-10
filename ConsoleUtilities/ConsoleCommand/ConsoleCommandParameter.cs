namespace net.erlenddahl.ConsoleUtilities.ConsoleCommand
{
    internal class ConsoleCommandParameter
    {
        public int Index { get; }
        public string Name { get; }
        public ConsoleCommandParameterType Type { get; }

        public string TypeDescription
        {
            get
            {
                switch (Type)
                {
                    case ConsoleCommandParameterType.Boolean:
                        return "A true/false value. Enter 'true', 't', 'yes', or 'y' for true, and anything else for false, for example: yes";
                    case ConsoleCommandParameterType.String:
                        return "A simple string value. Enter the string, enclosed in double quotes (\") if it contains spaces, for example: \"hello, I am a string\"";
                    case ConsoleCommandParameterType.Integer:
                        return "A simple integer value. Enter the number with no whitespace or decimal points, for example:  712";
                    case ConsoleCommandParameterType.IntegerArray:
                        return "Multiple integer values. Enter the numbers separated by a comma, with no other whitespace or decimal points, for example: 1,2,5,10,100";
                    case ConsoleCommandParameterType.Double:
                        return "A simple decimal value. Enter the number with a dot as a decimal separator, and no other whitespace, for example: 3.14";
                }

                return "[Unknown parameter type]";
            }
        }

        public ConsoleCommandParameter(int index, string name, ConsoleCommandParameterType type)
        {
            Index = index;
            Name = name;
            Type = type;
        }
    }
}