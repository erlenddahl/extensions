namespace net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel.ItemBases
{
    public abstract class ConsoleInfoItem
    {
        public bool FullWidth { get; set; }
        public abstract string Format(int consoleWidth);
        public int Sequence { get; set; }
    }
}