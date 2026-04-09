using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class MinByExtensionTests
    {
        [TestMethod]
        public void SimpleTest()
        {
            var items = new[]
            {
                new {v = 4, t = "Hei4"},
                new {v = 2, t = "Hei2"},
                new {v = 1, t = "Hei1"},
                new {v = 4, t = "Hei4"},
                new {v = 5, t = "Hei5"},
                new {v = 4, t = "Hei4"},
            };

            Assert.AreEqual("Hei1", items.MinBy(p => p.v).t);
        }
    }
}