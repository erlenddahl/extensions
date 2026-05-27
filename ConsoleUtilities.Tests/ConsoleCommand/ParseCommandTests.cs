using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.ConsoleUtilities;

namespace ConsoleUtilities.Tests.ConsoleCommand
{
    [TestClass]
    public class ParseCommandTests
    {
        [TestMethod]
        public void SimpleCommand()
        {
            var res = net.erlenddahl.ConsoleUtilities.ConsoleCommand.ConsoleCommand.ParseCommand("cmd 1 2");
            CollectionAssert.AreEqual(new List<string>() { "cmd", "1", "2" }, res);
        }

        [TestMethod]
        public void WithQuotes()
        {
            var res = net.erlenddahl.ConsoleUtilities.ConsoleCommand.ConsoleCommand.ParseCommand("cmd \"1\" \"2\"");
            CollectionAssert.AreEqual(new List<string>() { "cmd", "1", "2" }, res);
        }

        [TestMethod]
        public void WithQuotesAndSpaces()
        {
            var res = net.erlenddahl.ConsoleUtilities.ConsoleCommand.ConsoleCommand.ParseCommand("cmd \"C:\\Hei\\Halla\\File.cs\" \"2\"");
            CollectionAssert.AreEqual(new List<string>() { "cmd", "C:\\Hei\\Halla\\File.cs", "2" }, res);
        }

        [TestMethod]
        public void WithPipesInQuotes()
        {
            var res = net.erlenddahl.ConsoleUtilities.ConsoleCommand.ConsoleCommand.ParseCommand("cmd \"|| am |a| pipe | test\" \"2\" \"|\"");
            CollectionAssert.AreEqual(new List<string>() { "cmd", "|| am |a| pipe | test", "2", "|" }, res);
        }

        [TestMethod]
        public void WithPipes()
        {
            var res = net.erlenddahl.ConsoleUtilities.ConsoleCommand.ConsoleCommand.ParseCommand("cmd \"one\" | cmd \"two\"");
            CollectionAssert.AreEqual(new List<string>() { "cmd", "one", "|", "cmd", "two" }, res);
        }
    }
}
