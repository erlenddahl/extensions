using System;

namespace ConsoleUtilities.ConsoleCommand
{
    public class MissingArgumentException : Exception
    {
        public MissingArgumentException(string s) : base(s)
        {
        }
    }
}