using Extensions.IListExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IListExtensions
{
    [TestClass]
    public class IsValidIndexExtensions
    {
        [TestMethod]
        public void IsValidIndexTests()
        {
            var list = new[] {1, 2, 3, 4, 5};
            for (var i = -1; i > -1000; i--)
                Assert.AreEqual(false, list.IsValidIndex(i));
            for (var i = 0; i < 5; i++)
                Assert.AreEqual(true, list.IsValidIndex(i));
            for (var i = 5; i < 1000; i++)
                Assert.AreEqual(false, list.IsValidIndex(i));
            Assert.AreEqual(false, list.IsValidIndex(int.MaxValue));
            Assert.AreEqual(false, list.IsValidIndex(int.MinValue));
        }
    }
}
