namespace ConsoleUtilities.ConsoleInfoPanel
{
    public class LongInfoItem : ConsoleInfoItem
    {
        public long Value;
        public string FormatString = "n0";

        public override string Format(int consoleWidth)
        {
            return Value.ToString(FormatString);
        }
    }
}