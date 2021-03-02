using Extensions.DictionaryExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.DictionaryExtensions
{
    [TestClass]
    public class ToDictionary
    {
        [TestMethod]
        public void ToDictionarySafeTests()
        {
            var arr = new int[] { 1, 2, 3, 4, 5 };
            var d = arr.ToDictionarySafe(k => k.ToString(), v => v);

            Assert.AreEqual(5, d.Count);
            for (var i = 1; i < 6; i++)
                Assert.AreEqual(i, d[i.ToString()]);


            arr = new int[] { 1, 2, 3, 4, 5, 5 };
            d = arr.ToDictionarySafe(k => k.ToString(), v => v);

            Assert.AreEqual(6, d.Count);
            for (var i = 1; i < 6; i++)
                Assert.AreEqual(i, d[i.ToString()]);

            Assert.AreEqual(5, d["5 (1)"]);
        }
    }
}