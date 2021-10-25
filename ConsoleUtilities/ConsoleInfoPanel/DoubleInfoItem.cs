namespace ConsoleUtilities.ConsoleInfoPanel
{
    public class DoubleInfoItem : ConsoleInfoItem
    {
        public double Value;
        public string FormatString = "n3";

        public override string Format(int consoleWidth)
        {
            return Value.ToString(FormatString);
        }
    }
}