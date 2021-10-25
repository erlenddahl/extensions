namespace ConsoleUtilities.ConsoleInfoPanel
{
    public class IntInfoItem : ConsoleInfoItem
    {
        public int Value;
        public string FormatString = "n0";

        public override string Format(int consoleWidth)
        {
            return Value.ToString(FormatString);
        }
    }
}