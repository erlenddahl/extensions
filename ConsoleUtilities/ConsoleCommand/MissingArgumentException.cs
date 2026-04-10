namespace net.erlenddahl.ConsoleUtilities.ConsoleCommand
{
    public class MissingArgumentException : Exception
    {
        public MissingArgumentException(string s) : base(s)
        {
        }
    }
}