using net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel.ItemBases;

namespace net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel.Items
{
    public class LongInfoItem : ConsoleInfoItem
    {
        public long Value;
        public string FormatString = "n0";

        public override string Format(int consoleWidth)
        {
            return Value.ToString(FormatString);
        }

        private object _locker = new object();
        public void Increment(long inc = 1)
        {
            Interlocked.Add(ref Value, inc);
        }
    }
}