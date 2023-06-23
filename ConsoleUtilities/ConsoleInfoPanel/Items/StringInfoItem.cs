using ConsoleUtilities.ConsoleInfoPanel.ItemBases;

namespace ConsoleUtilities.ConsoleInfoPanel.Items
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