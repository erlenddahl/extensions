using System;

namespace ConsoleUtilities.ConsoleInfoPanel
{
    public class AppendableStringInfoItem : ConsoleInfoItem
    {
        public string Value;

        public AppendableStringInfoItem()
        {
            FullWidth = true;
        }

        public AppendableStringInfoItem AppendLine(string msg, bool prependTimestamp = true, string timestampFormat = "yyyy-MM-dd HH:mm:ss.fff")
        {
            Value += Environment.NewLine + (prependTimestamp ? DateTime.Now.ToString(timestampFormat) + ": " : "") + msg;
            return this;
        }

        public override string Format(int consoleWidth)
        {
            return Value;
        }
    }
}