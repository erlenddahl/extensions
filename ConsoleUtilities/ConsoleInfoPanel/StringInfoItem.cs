namespace ConsoleUtilities.ConsoleInfoPanel
{
    public class StringInfoItem : ConsoleInfoItem
    {
        public string Value;

        public override string Format(int consoleWidth)
        {
            return Value;
        }
    }
}