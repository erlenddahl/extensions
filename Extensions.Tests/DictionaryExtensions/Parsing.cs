using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.DictionaryExtensions
{
    [TestClass]
    public class Parsing
    {
        [TestMethod]
        public void GetValueOrDefaultTests()
        {
            var d = new Dictionary<string, double>()
            {
                {"a", 4},
                {"b", 2}
            };

            Assert.AreEqual(10, d.GetValueOrDefault("c", 10));
            Assert.AreEqual(0, d.GetValueOrDefault("c", 0));
            Assert.AreEqual(0, d.GetValueOrDefault("c"));
            Assert.AreEqual(4, d.GetValueOrDefault("a"));
            Assert.AreEqual(2, d.GetValueOrDefault("b"));
        }
    }
}