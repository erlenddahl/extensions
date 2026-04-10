using net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel.ItemBases;

namespace net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel.Items
{
    public class DateTimeInfoItem : ConsoleInfoItem
    {
        public DateTime Value;
        public string FormatString = "yyyy-MM-dd HH:mm:ss.fff";

        public override string Format(int consoleWidth)
        {
            return Value.ToString(FormatString);
        }
    }
}