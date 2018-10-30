using System;

namespace ConsoleUtilities.ConsoleCommand
{
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException(string s):base(s)
        {
        }
    }
}