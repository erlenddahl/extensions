using net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel.ItemBases;

namespace net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel.Items
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