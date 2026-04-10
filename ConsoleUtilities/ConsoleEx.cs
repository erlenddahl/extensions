namespace net.erlenddahl.ConsoleUtilities
{
    public static class ConsoleEx
    {
        /// <summary>
        /// Will write the given text to the Console (using Console.WriteLine) with the given padding. If the
        /// text is too long for a single line (measured using Console.WindowWidth - padding), it will be broken
        /// at a space, and written in multiple (padded) lines.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="padding"></param>
        public static void WritePaddedLine(string text, int padding)
        {
            var desc = text.Split(' ');
            var availableWidth = Console.WindowWidth - padding;
            var paddingString = string.Join("", Enumerable.Range(0, padding).Select(p => " "));
            while (desc.Any())
            {
                var sub = "";
                while (desc.Any())
                {
                    var word = desc.First();
                    if ((sub + word + " ").Length > availableWidth) break;
                    sub += word + " ";
                    desc = desc.Skip(1).ToArray();
                }
                Console.WriteLine(paddingString + sub.Trim());
            }
        }
    }
}
