using net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel.ItemBases;

namespace net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel.Items
{
    public class IntInfoItem : ConsoleInfoItem
    {
        public int Value;
        public string FormatString = "n0";

        public override string Format(int consoleWidth)
        {
            return Value.ToString(FormatString);
        }

        public void Increment(int inc = 1)
        {
            Interlocked.Add(ref Value, inc);
        }
    }
}