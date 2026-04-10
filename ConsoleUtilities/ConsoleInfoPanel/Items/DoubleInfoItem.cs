using net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel.ItemBases;

namespace net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel.Items
{
    public class DoubleInfoItem : ConsoleInfoItem
    {
        public double Value;
        public string FormatString = "n3";

        public override string Format(int consoleWidth)
        {
            return Value.ToString(FormatString);
        }

        private object _locker = new object();
        public void Increment(double inc = 1)
        {
            lock (_locker)
            {
                Value += inc;
            }
        }
    }
}